
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestCodeCS
{
    class indoor
    {
        static MarvelmindHedge hedge;
        public indoor()
        {
            SerialPort mySerialPort = new SerialPort("COM6");

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;

            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            mySerialPort.Open();

            hedge = new MarvelmindHedge();

            Console.WriteLine("Press any key to continue...");
            Console.WriteLine();
            Console.ReadKey();
            mySerialPort.Close();
        }
        static UInt16 CalcCrcModbus_(byte[] buf, int len)
        {
            UInt16 crc = 0xFFFF;
            int pos;
            for (pos = 0; pos < len; pos++)
            {
                crc ^= buf[pos]; // XOR byte into least sig. byte of crc
                int i;
                for (i = 8; i != 0; i--) // Loop over each bit
                {
                    if ((crc & 0x0001) != 0) // If the LSB is set
                    {
                        crc >>= 1; // Shift right and XOR 0xA001
                        crc ^= 0xA001;
                    }
                    else  // Else LSB is not set
                        crc >>= 1; // Just shift right
                }
            }
            return crc;
        }


        static PositionValue process_position_datagram(MarvelmindHedge hedge, byte[] buffer)
        {
            byte ind = hedge.lastValues_next;

            hedge.positionBuffer[ind].address = buffer[16];
            hedge.positionBuffer[ind].timestamp = buffer[5] |
        (((UInt32)buffer[6]) << 8) |
        (((UInt32)buffer[7]) << 16) |
        (((UInt32)buffer[8]) << 24);

            Int16 vx = (Int16)(buffer[9] |
                    (((UInt16)buffer[10]) << 8));
            hedge.positionBuffer[ind].x = vx * 10;// millimeters

            Int16 vy = (Int16)(buffer[11] |
                        (((UInt16)buffer[12]) << 8));
            hedge.positionBuffer[ind].y = vy * 10;// millimeters

            Int16 vz = (Int16)(buffer[13] |
                        (((UInt16)buffer[14]) << 8));
            hedge.positionBuffer[ind].z = vz * 10;// millimeters

            UInt16 vang = (UInt16)(buffer[17] |
                           (((UInt16)buffer[18]) << 8));
            hedge.positionBuffer[ind].angle = ((float)(vang & 0x0fff)) / 10.0f;

            hedge.positionBuffer[ind].highResolution = false;

            ind = markPositionReady(hedge);

            return hedge.positionBuffer[ind];
        }
        static byte markPositionReady(MarvelmindHedge hedge)
        {
            byte ind = hedge.lastValues_next;
            byte indCur = ind;

            hedge.positionBuffer[ind].ready = true;
            hedge.positionBuffer[ind].processed = false;
            ind++;
            if (ind >= (byte)Command.MAX_BUFFERED_POSITIONS)
                ind = 0;
            if (hedge.lastValuesCount_ < (byte)Command.MAX_BUFFERED_POSITIONS)
                hedge.lastValuesCount_++;
            hedge.haveNewValues_ = true;

            hedge.lastValues_next = ind;

            return indCur;
        }

        static PositionValue process_position_highres_datagram(MarvelmindHedge hedge, byte[] buffer)
        {
            byte ind = hedge.lastValues_next;

            hedge.positionBuffer[ind].address = buffer[22];
            hedge.positionBuffer[ind].timestamp = buffer[5] |
                        (((UInt32)buffer[6]) << 8) |
                        (((UInt32)buffer[7]) << 16) |
                        (((UInt32)buffer[8]) << 24);

            Int32 vx = (Int32)(buffer[9] |
                        (((UInt32)buffer[10]) << 8) |
                        (((UInt32)buffer[11]) << 16) |
                        (((UInt32)buffer[12]) << 24));
            hedge.positionBuffer[ind].x = vx;

            Int32 vy = (Int32)(buffer[13] |
                        (((UInt32)buffer[14]) << 8) |
                        (((UInt32)buffer[15]) << 16) |
                        (((UInt32)buffer[16]) << 24));
            hedge.positionBuffer[ind].y = vy;

            Int32 vz = (Int32)(buffer[17] |
                        (((UInt32)buffer[18]) << 8) |
                        (((UInt32)buffer[19]) << 16) |
                        (((UInt32)buffer[20]) << 24));
            hedge.positionBuffer[ind].z = vz;

            UInt16 vang = (UInt16)(buffer[23] |
                           ((buffer[24]) << 8));
            hedge.positionBuffer[ind].angle = ((float)(vang & 0x0fff)) / 10.0f;

            hedge.positionBuffer[ind].highResolution = true;

            ind = markPositionReady(hedge);

            return hedge.positionBuffer[ind];
        }


        static void process_beacons_positions_datagram(MarvelmindHedge hedge, byte[] buffer)
        {
            byte n = buffer[5];// number of beacons in packet
            byte i, ofs;
            byte address;
            Int16 x, y, z;
            StationaryBeaconPosition b;

            if ((1 + n * 8) != buffer[4])
                return;// incorrect size

            for (i = 0; i < n; i++)
            {
                ofs = Convert.ToByte(6 + i * 8);

                address = buffer[ofs + 0];
                x = (Int16)(buffer[ofs + 1] |
                    ((buffer[ofs + 2]) << 8));
                y = (Int16)(buffer[ofs + 3] |
                    ((buffer[ofs + 4]) << 8));
                z = (Int16)(buffer[ofs + 5] |
                    ((buffer[ofs + 6]) << 8));

                b = getOrAllocBeacon(hedge, address);
                if (b != null)
                {
                    b.address = address;
                    b.x = x * 10;// millimeters
                    b.y = y * 10;// millimeters
                    b.z = z * 10;// millimeters

                    b.highResolution = false;

                    hedge.positionsBeacons.updated = true;
                }
            }
        }

        static StationaryBeaconPosition getOrAllocBeacon(MarvelmindHedge hedge, byte address)
        {
            byte i;
            byte n_used = hedge.positionsBeacons.numBeacons;

            if (n_used != 0)
            {
                for (i = 0; i < n_used; i++)
                {
                    if (hedge.positionsBeacons.beacons[i] != null && hedge.positionsBeacons.beacons[i].address == address)
                    {
                        return hedge.positionsBeacons.beacons[i];
                    }
                }
            }

            if (n_used >= (Convert.ToByte(Command.MAX_STATIONARY_BEACONS) - 1))
                return null;

            hedge.positionsBeacons.numBeacons = Convert.ToByte(n_used + Convert.ToByte(1));
            return hedge.positionsBeacons.beacons[n_used];
        }

        static void process_beacons_positions_highres_datagram(MarvelmindHedge hedge, byte[] buffer)
        {
            byte n = buffer[5];// number of beacons in packet
            byte i, ofs;
            byte address;
            Int32 x, y, z;
            StationaryBeaconPosition b;

            if ((1 + n * 14) != buffer[4])
                return;// incorrect size

            for (i = 0; i < n; i++)
            {
                ofs = Convert.ToByte(6 + i * 14);

                address = buffer[ofs + 0];
                x = Convert.ToInt32(buffer[ofs + 1] |
                    (((UInt32)buffer[ofs + 2]) << 8) |
                    (((UInt32)buffer[ofs + 3]) << 16) |
                    (((UInt32)buffer[ofs + 4]) << 24));
                y = Convert.ToInt32(buffer[ofs + 5] |
                    (((UInt32)buffer[ofs + 6]) << 8) |
                    (((UInt32)buffer[ofs + 7]) << 16) |
                    (((UInt32)buffer[ofs + 8]) << 24));
                z = Convert.ToInt32(buffer[ofs + 9] |
                    (((UInt32)buffer[ofs + 10]) << 8) |
                    (((UInt32)buffer[ofs + 11]) << 16) |
                    (((UInt32)buffer[ofs + 12]) << 24));

                b = getOrAllocBeacon(hedge, address);
                if (b != null)
                {
                    b.address = address;
                    b.x = x;
                    b.y = y;
                    b.z = z;

                    b.highResolution = true;

                    hedge.positionsBeacons.updated = true;
                }
            }
        }

        public static StationaryBeaconsPositions getStationaryBeaconsPositionsFromMarvelmindHedge(MarvelmindHedge hedge)
        {

            return hedge.positionsBeacons;
        }

        public static void printStationaryBeaconsPositionsFromMarvelmindHedge(MarvelmindHedge hedge, bool onlyNew)
        {
            StationaryBeaconsPositions positions = new StationaryBeaconsPositions();
            double xm, ym, zm;

            positions = getStationaryBeaconsPositionsFromMarvelmindHedge(hedge);

            if (positions.updated || (!onlyNew))
            {
                byte i;
                byte n = hedge.positionsBeacons.numBeacons;
                StationaryBeaconPosition b;

                for (i = 0; i < n; i++)
                {
                    b = positions.beacons[i];
                    xm = ((double)b.x) / 1000.0;
                    ym = ((double)b.y) / 1000.0;
                    zm = ((double)b.z) / 1000.0;
                    if (positions.beacons[i].highResolution)
                    {
                        Console.WriteLine("Stationary beacon: address: " + b.address + ", X: " + xm + ", Y: " + ym + ", Z: " + zm + " \n");
                    }
                    else
                    {
                        Console.WriteLine("Stationary beacon: address: " + b.address + ", X: " + xm + ", Y: " + ym + ", Z: " + zm + " \n");
                    }
                }

                hedge.positionsBeacons.updated = false;
            }
        }

        private static void DataReceivedHandler(
        object sender,
        SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            //string indata = sp.ReadExisting();
            sp.DtrEnable = true;
            sp.RtsEnable = true;

            int bytes = sp.BytesToRead;
            byte[] buffer = new byte[bytes];
            sp.Read(buffer, 0, bytes);

            RECV recvState = RECV.RECV_HDR; // current state of receive data
            UInt16 dataId = 0;
            byte nBytesInBlockReceived = 0; // bytes received
            byte[] input_buffer = new byte[256];
            PositionValue curPosition;

            Console.WriteLine("Data Received:");


            for (int i = 0; i < bytes; i++)
            {
                byte receivedChar = buffer[i];
                //Console.WriteLine("So phan tu : " + bytes + " thu " + i + " Gia tri : " + receivedChar + " ----------- "+ nBytesInBlockReceived);
                bool goodByte = false;
                input_buffer[nBytesInBlockReceived] = receivedChar;
                switch (recvState)
                {
                    case (byte)RECV.RECV_HDR:
                        {
                            switch (nBytesInBlockReceived)
                            {
                                case 0:
                                    goodByte = (receivedChar == 0xff);
                                    break;
                                case 1:
                                    goodByte = (receivedChar == 0x47);
                                    break;
                                case 2:
                                    goodByte = true;
                                    break;
                                case 3:
                                    dataId = Convert.ToUInt16((((UInt16)receivedChar) << 8) + input_buffer[2]);
                                    goodByte = (dataId == Convert.ToUInt16(Command.POSITION_DATAGRAM_ID)) ||
                                                (dataId == Convert.ToUInt16(Command.BEACONS_POSITIONS_DATAGRAM_ID)) ||
                                                (dataId == Convert.ToUInt16(Command.POSITION_DATAGRAM_HIGHRES_ID)) ||
                                                (dataId == Convert.ToUInt16(Command.BEACONS_POSITIONS_DATAGRAM_HIGHRES_ID)) ||
                                                (dataId == Convert.ToUInt16(Command.IMU_RAW_DATAGRAM_ID)) ||
                                                (dataId == Convert.ToUInt16(Command.IMU_FUSION_DATAGRAM_ID)) ||
                                                (dataId == Convert.ToUInt16(Command.BEACON_RAW_DISTANCE_DATAGRAM_ID));
                                    break;
                                case 4:
                                    switch (dataId)
                                    {
                                        case (UInt16)Command.POSITION_DATAGRAM_ID:
                                            goodByte = (receivedChar == 0x10);
                                            break;
                                        case (UInt16)Command.BEACONS_POSITIONS_DATAGRAM_ID:
                                        case (UInt16)Command.BEACONS_POSITIONS_DATAGRAM_HIGHRES_ID:
                                            goodByte = true;
                                            break;
                                        case (UInt16)Command.POSITION_DATAGRAM_HIGHRES_ID:
                                            goodByte = (receivedChar == 0x16);
                                            break;
                                        case (UInt16)Command.IMU_RAW_DATAGRAM_ID:
                                            goodByte = (receivedChar == 0x20);
                                            break;
                                        case (UInt16)Command.IMU_FUSION_DATAGRAM_ID:
                                            goodByte = (receivedChar == 0x2a);
                                            break;
                                        case (UInt16)Command.BEACON_RAW_DISTANCE_DATAGRAM_ID:
                                            goodByte = (receivedChar == 0x20);
                                            break;
                                    }
                                    if (goodByte)
                                        recvState = RECV.RECV_DGRAM;
                                    break;
                            }
                            if (goodByte)
                            {
                                // correct header byte
                                nBytesInBlockReceived++;
                            }
                            else
                            {
                                // ...or incorrect
                                recvState = (byte)RECV.RECV_HDR;
                                nBytesInBlockReceived = 0;
                            }
                        }
                        break;
                    case RECV.RECV_DGRAM:
                        {
                            nBytesInBlockReceived++;
                            if (nBytesInBlockReceived >= 7 + input_buffer[4])
                            {
                                // parse dgram
                                UInt16 blockCrc = CalcCrcModbus_(input_buffer, nBytesInBlockReceived);
                                if (blockCrc == 0)
                                {
                                    switch (dataId)
                                    {
                                        case (UInt16)Command.POSITION_DATAGRAM_ID:
                                            // add to positionBuffer
                                            curPosition = process_position_datagram(hedge, input_buffer);
                                            Console.WriteLine("X : "+curPosition.x+" ------- Y : "+curPosition.y+"-------- Z : "+curPosition.z+"---------Angle : "+curPosition.angle);
                                            break;
                                        case (UInt16)Command.BEACONS_POSITIONS_DATAGRAM_ID:
                                            process_beacons_positions_datagram(hedge, input_buffer);
                                            Console.WriteLine("process_beacons_positions_datagram");
                                            printStationaryBeaconsPositionsFromMarvelmindHedge(hedge, true);
                                            break;
                                        case (UInt16)Command.POSITION_DATAGRAM_HIGHRES_ID:
                                            // add to positionBuffer
                                            //curPosition = process_position_highres_datagram(this, input_buffer);
                                            Console.WriteLine("process_position_highres_datagram");
                                            break;
                                        case (UInt16)Command.BEACONS_POSITIONS_DATAGRAM_HIGHRES_ID:
                                            //process_beacons_positions_highres_datagram(this, input_buffer);
                                            Console.WriteLine("process_beacons_positions_highres_datagram");
                                            break;
                                        case (UInt16)Command.IMU_RAW_DATAGRAM_ID:
                                            //process_imu_raw_datagram(this, input_buffer);
                                            Console.WriteLine("process_imu_raw_datagram");
                                            break;
                                        case (UInt16)Command.IMU_FUSION_DATAGRAM_ID:
                                            //process_imu_fusion_datagram(this, input_buffer);
                                            Console.WriteLine("process_imu_fusion_datagram");
                                            break;
                                        case (UInt16)Command.BEACON_RAW_DISTANCE_DATAGRAM_ID:
                                            //process_raw_distances_datagram(this, input_buffer);
                                            Console.WriteLine("process_raw_distances_datagram");
                                            break;
                                    }
                                }
                                // and repeat
                                recvState = (byte)RECV.RECV_HDR;
                                nBytesInBlockReceived = 0;
                            }
                        }
                        break;
                }

            }


        }
    }
}
