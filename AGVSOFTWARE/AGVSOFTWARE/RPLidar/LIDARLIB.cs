
using AGVSOFTWARE.AGVROBOT;
using AGVSOFTWARE.StructureData;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSOFTWARE.RPLidar
{
    class LIDARLIB
    {
        public static LinkedList DataLIDARLL = new LinkedList();
        public static int countNode = 0;
        static RPLidarSerialDevice RPLidar;
        public LIDARLIB()
        {
            //New RPLidar object
            RPLidar = new RPLidarSerialDevice();
            //Set output parameters
            RPLidar.Verbose = false;
            RPLidar.WriteOutCoordinates = false;
        }

        public void GetData()
        {
            try
            {
                //Connect RPLidar
                RPLidar.Connect();
                //Reset - Not really sure how this is supposed to work, reconnecting USB works too
                //RPLidar.Reset();
                //Stop motor
                RPLidar.StopMotor();
                //Get Device Information
                RPLidar.GetDeviceInfo();
                //Get Device Health
                RPLidar.GetDeviceHealth();
                //Get Data Event
                RPLidar.Data += RPLidar_Data;
                //Start Scan Thread
                RPLidar.StartScan();
            }
            catch (System.IO.IOException ex)
            {
                HandleError("Serial connection failed: " + ex.Message);
            }
            catch (Exception ex)
            {
                HandleError("Something bad happend \n\t:" + ex.Source + "\n\t:" + ex.Message);
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

       
        }

        public void Stop()
        {
            //Stop Scanning
            RPLidar.StopScan();
            //Disconnect
            RPLidar.Disconnect();
            //Dispose Object
            RPLidar.Dispose();

          SerialPort  _serialPort = new SerialPort();
            _serialPort.PortName = "com4";
            _serialPort.Close();
        }

        static void RPLidar_Data(List<Response_PointFormat> Frames)
        {
            //Handle data here
            foreach (Response_PointFormat _frame in Frames)
            {
                //  Console.WriteLine("Distance: " + _frame.Distance.ToString() + " Angle: " + _frame.AngleDegrees.ToString());
                // Console.WriteLine("X: " + _frame.X.ToString() + " Y: " + _frame.Y.ToString());
                if (_frame.AngleDegrees > 340 && _frame.AngleDegrees < 360 || _frame.AngleDegrees > 0 && _frame.AngleDegrees < 20)
                {
                    Frame f = Frame.Convert(_frame);
                    //list.Add(f);
                    Node newNode = new Node(f);
                    DataLIDARLL.addNode(newNode);
                    // countNode++;
                   
                    AVGROBOTLIB.CheckWarning(newNode);
                   
                }
            }
        }

        private static void HandleError(string Message)
        {
            Console.WriteLine(Message);
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}
