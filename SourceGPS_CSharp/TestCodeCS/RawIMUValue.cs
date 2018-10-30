using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCodeCS
{
    public class RawIMUValue
    {
        public Int16 acc_x;
        public Int16 acc_y;
        public Int16 acc_z;

        public Int16 gyro_x;
        public Int16 gyro_y;
        public Int16 gyro_z;

        public Int16 compass_x;
        public Int16 compass_y;
        public Int16 compass_z;

        public float timestamp;

        public bool updated;

        public RawIMUValue()
        {
            acc_x = new Int16();
            acc_y = new Int16();
            acc_z = new Int16();

            gyro_x = new Int16();
            gyro_y = new Int16();
            gyro_z = new Int16();

            compass_x = new Int16();
            compass_y = new Int16();
            compass_z = new Int16();

            timestamp = new float();

            updated = new bool();
    }
    }
}
