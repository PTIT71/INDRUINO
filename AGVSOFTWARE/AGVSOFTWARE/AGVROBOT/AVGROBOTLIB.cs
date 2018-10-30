
using AGVSOFTWARE.StructureData;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Media;

namespace AGVSOFTWARE.AGVROBOT
{
    public class AVGROBOTLIB
    {
        public enum direction
        {
            HEAD, LEFT, RIGHT
        }

        public struct WARING
        {
            public double distance;
            public direction direction;
        }

        public struct STATEWARNING
        {
            public WARING WRN;

            public bool State;

        }

        public static Point POSITION = new Point(0, 0);

        public STATEWARNING WARNINGS;

        public static SerialPort  Port1 = new SerialPort();
        public AVGROBOTLIB()
        {
            
            Port1.PortName = "com6";
         //   Port1.Open();

        }
        Stopwatch st = new Stopwatch();

        public static void CheckWarning(Node newNode)
        {
            if(newNode.frame.Distance<300)
            {
              //  Port1.Write("3");
                return;
            }
            else
            {
                if (newNode.frame.Distance < 700)
                {
                 //   Port1.Write("2");
                    return;
                }
            }

          //  Port1.Write("1");
            return;
        }

        public void CheckGeneralWarning()
        {
            st.Stop();
            long timeelapsed = st.ElapsedMilliseconds;

            if (timeelapsed > 1900)
            {
                if (WARNINGS.State == true)
                {
                    if (WARNINGS.WRN.direction == direction.HEAD)
                    {
                        st.Reset();
                        st.Start();
                        SoundPlayer simpleSound = new SoundPlayer("Truoc.wav");
                        simpleSound.Play();
                        WARNINGS.State = false;
                    }
                    //if (WARNINGS.WRN.direction == direction.RIGHT)
                    //{
                    //    st.Reset();
                    //    st.Start();
                    //    SoundPlayer simpleSound = new SoundPlayer("Phai.wav");
                    //    simpleSound.Play();
                    //    WARNINGS.State = false;
                    //}
                    //if (WARNINGS.WRN.direction == direction.LEFT)
                    //{
                    //    st.Reset();
                    //    st.Start();
                    //    SoundPlayer simpleSound = new SoundPlayer("Trai.wav");
                    //    simpleSound.Play();
                    //    WARNINGS.State = false;
                    //}

                    if (WARNINGS.WRN.distance < 500)
                    {
                       // Port1.Write("2");
                    }
                    else
                    {
                        if (WARNINGS.WRN.distance < 200)
                        {
                           // Port1.Write("3");
                        }
                    }




                }
                else
                {
                  //  Port1.Write("1");
                }
            }
            else
            {
               
                st.Start();
            }

        }

        public void CheckHEAD(Node NodePoint)
        {
            if(WARNINGS.State==true)
            {
                //if(WARNINGS.WRN.distance >NodePoint.frame.Distance)
                //{
                //    WARNINGS.WRN.direction = direction.HEAD;
                //    WARNINGS.WRN.distance = NodePoint.frame.Distance;
                //}
            }
            else
            {
                WARNINGS.WRN.direction = direction.HEAD;
                WARNINGS.WRN.distance = NodePoint.frame.Distance;

                WARNINGS.State = true;
            }
        }

        public void CheckRIGHT(Node NodePoint)
        {
            if (WARNINGS.State == true)
            {
                if (WARNINGS.WRN.distance > NodePoint.frame.Distance)
                {
                    WARNINGS.WRN.direction = direction.RIGHT;
                    WARNINGS.WRN.distance = NodePoint.frame.Distance;
                }
            }
            else
            {
                WARNINGS.WRN.direction = direction.RIGHT;
                WARNINGS.WRN.distance = NodePoint.frame.Distance;

                WARNINGS.State = true;
            }


        }

        public void CheckLEFT(Node NodePoint)
        {
            if (WARNINGS.State == true)
            {
                if (WARNINGS.WRN.distance > NodePoint.frame.Distance)
                {
                    WARNINGS.WRN.direction = direction.LEFT;
                    WARNINGS.WRN.distance = NodePoint.frame.Distance;
                }
            }
            else
            {
                WARNINGS.WRN.direction = direction.LEFT;
                WARNINGS.WRN.distance = NodePoint.frame.Distance;

                WARNINGS.State = true;
            }


        }
    }
}
