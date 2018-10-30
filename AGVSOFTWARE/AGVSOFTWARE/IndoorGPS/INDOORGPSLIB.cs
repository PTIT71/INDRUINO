using AGVSOFTWARE.StructureData;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGVSOFTWARE.IndoorGPS
{
    class INDOORGPSLIB
    {
        SerialPort mySerialPort;
        Timer tm = new Timer();

        public static LinkedList DataGPSLL = new LinkedList(); 


        public INDOORGPSLIB()
        {
            mySerialPort = new SerialPort("COM3");

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;
            mySerialPort.Open();
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

           // mySerialPort.Open();

            Console.WriteLine("Press any key to continue...");
            Console.WriteLine();
            Console.ReadKey();
            mySerialPort.Close();
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
            Console.WriteLine("Data Received:");
            // Console.WriteLine(indata);
            if (buffer.Length >= 3)
            {
                if (buffer[2] == 2)
                {
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                }

            }

            for (int i = 0; i < buffer.Length; i++)
            {
                //  Console.WriteLine("buffer size : " + buffer.Length);
                //  Console.WriteLine("byte size : " + bytes.ToString());
                // Console.Write(buffer[i].ToString());
                //  Console.Write(" ");



            }
            try
            {
                int vx = buffer[9] | (((int)buffer[10]) << 8);


                int vy = buffer[11] | (((int)buffer[12]) << 8);

                int vz = buffer[13] | (((int)buffer[14]) << 8);

                int vang = buffer[17] |
                    (((int)buffer[18]) << 8);

                float angle = ((float)(vang & 0x0fff)) / 10.0f;

              

                Console.Write("\n");
                Console.WriteLine("X= " + (double)vx / 100.0 + "        Y= " + (double)vy / 100.0 + "        Z= " + (double)vz / 100.0 + "        Angle:  " + (double)vz / 100.0);// millimeters
                Console.Write("\n");

                Frame _f = new Frame(vx, vy, 0, 0, false);

                DataGPSLL.addNode(new Node(_f));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }


    }
}
