using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCodeCS
{
    public class StationaryBeaconPosition
    {
        public byte address;
        public Int32 x, y, z;// coordinates in millimeters

        public bool highResolution;

        public StationaryBeaconPosition()
        {
            address = new byte();
            x = new Int32();
            y = new Int32();
            z = new Int32();
            highResolution = new bool();
        }
    }
}
