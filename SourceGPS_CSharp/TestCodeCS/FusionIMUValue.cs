using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCodeCS
{
    public class FusionIMUValue
    {
        public Int32 x;
        public Int32 y;
        public Int32 z;// coordinates in mm

        public Int16 qw;
        public Int16 qx;
        public Int16 qy;
        public Int16 qz;// quaternion, normalized to 10000

        public Int16 vx;
        public Int16 vy;
        public Int16 vz;// velocity, mm/s

        public Int16 ax;
        public Int16 ay;
        public Int16 az;// acceleration, mm/s^2

        public float timestamp;

        public bool updated;

        public FusionIMUValue()
        {
            this.x = new Int32();
            this.y = new Int32();
            this.z = new Int32();// coordinates in mm

            this.qw = new Int16();
            this.qx = new Int16();
            this.qy = new Int16();
            this.qz = new Int16();// quaternion, normalized to 10000

            this.vx = new Int16();
            this.vy = new Int16();
            this.vz = new Int16();// velocity, mm/s

            this.ax = new Int16();
            this.ay = new Int16();
            this.az = new Int16();// acceleration, mm/s^2
        }
    }
}
