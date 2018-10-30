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
        Thread printThread;
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


        static PositionValue process_position_datagram(byte[] buffer)
        {
            PositionValue pos = new PositionValue();

            pos.address = buffer[16];
            pos.timestamp = buffer[5] |
        (((UInt32)buffer[6]) << 8) |
        (((UInt32)buffer[7]) << 16) |
        (((UInt32)buffer[8]) << 24);

            Int16 vx = (Int16)(buffer[9] |
                    (((UInt16)buffer[10]) << 8));
            pos.x = vx * 10;// millimeters

            Int16 vy = (Int16)(buffer[11] |
                        (((UInt16)buffer[12]) << 8));
            pos.y = vy * 10;// millimeters

            Int16 vz = (Int16)(buffer[13] |
                        (((UInt16)buffer[14]) << 8));
            pos.z = vz * 10;// millimeters

            UInt16 vang = (UInt16)(buffer[17] |
                           (((UInt16)buffer[18]) << 8));
            pos.angle = ((float)(vang & 0x0fff)) / 10.0f;

            pos.highResolution = false;

            return pos;
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
                                            curPosition = process_position_datagram(input_buffer);
                                            Console.WriteLine("X : "+curPosition.x+" ------- Y : "+curPosition.y+"-------- Z : "+curPosition.z+"---------Angle : "+curPosition.angle);
                                            break;
                                        case (UInt16)Command.BEACONS_POSITIONS_DATAGRAM_ID:
                                            //process_beacons_positions_datagram(this, input_buffer);
                                            Console.WriteLine("process_beacons_positions_datagram");
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
