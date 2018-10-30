using AGVSOFTWARE.RPLidar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSOFTWARE.StructureData
{
    public class Frame
    {
        //Properties
        public double AngleDegrees { get;}
        public double Distance { get;}

        public double X { get;}
        public double Y { get;}
        public bool CheckBit { get; }

        private SolidBrush brushDot;

        public static double MmToPixel = 3.7795275591;

        //Constructor
        public Frame()
        {
            
        }
        public Frame(double x, double y,double dist,double angle,bool checkbit)
        {
            this.AngleDegrees = angle;
            this.Distance = dist;
            this.X = x*MmToPixel;
            this.Y = y*MmToPixel;
            this.CheckBit = checkbit;
            brushDot = new SolidBrush(Color.LightGreen);
        }
        //Operator
        public static bool operator ==(Frame a, Frame b)
        {
            if ((int)a.X == (int)b.X && (int)a.Y == (int)b.Y)
                return true;
            return false;
        }

        public static bool operator !=(Frame a, Frame b)
        {
            return !(a==b);
        }
        
        //Method
        public void Show()
        {
            Console.WriteLine("Angle: " + this.AngleDegrees + "      Dist: " + Distance + "        X: " + this.X + "     y: " + this.Y);
        }

        public void DrawDot(Graphics g,float scale)
        {
            //Console.WriteLine("Draww");
            g.FillRectangle(brushDot, (float)(600 - this.X * scale), (float)(400 - this.Y * scale), 1, 1);
            g.DrawLine(new Pen(Color.Yellow), 600, 400, (float)(600 - this.X * scale), (float)(400 - this.Y * scale));
        }

        public static Frame Convert(Response_PointFormat frame)
        {
            return new Frame(frame.X, frame.Y, frame.Distance, frame.AngleDegrees, frame.CheckBit);
        }

        public void sWriter(StreamWriter sw)
        {
            if(CheckBit)
            {
                sw.WriteLine("-------------------------------------------------++++++++++++++++++++++----------------------------------");
                sw.WriteLine("-------------------------------------------------+ New 360 +----------------------------------------");
                sw.WriteLine("-------------------------------------------------Frame--------------------------------------------");
                sw.WriteLine("Distance: " + Distance + " Angle: " + AngleDegrees);
                sw.WriteLine("X: " + X + " Y: " + Y);
                sw.WriteLine("-------------------------------------------------++++++++++++++++++++++----------------------------------");
            }
            else
            {
                sw.WriteLine("-------------------------------------------------Frame--------------------------------------------");
                sw.WriteLine("Distance: " + Distance + " Angle: " + AngleDegrees);
                sw.WriteLine("X: " + X + " Y: " + Y);
            }
        }
    }
}
