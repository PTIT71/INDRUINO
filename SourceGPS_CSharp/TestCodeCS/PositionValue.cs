using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCodeCS
{
    class PositionValue
    {
        public byte address;
        public UInt32 timestamp;
        public Int32 x, y, z;// coordinates in millimeters

        public double angle;
        public bool highResolution;

        public bool ready;
        public bool processed;

        public PositionValue()
        {
            address = new byte();
            timestamp = new UInt32();
            x = new Int32(); y = new Int32(); z = new Int32();
            angle = new double();
            highResolution = new bool();
            ready = new bool();
            processed = new bool();
        }
    }
}
