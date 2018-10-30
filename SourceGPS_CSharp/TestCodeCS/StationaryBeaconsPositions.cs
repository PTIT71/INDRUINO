using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCodeCS
{
    public class StationaryBeaconsPositions
    {
        public byte numBeacons;
        public StationaryBeaconPosition[] beacons;

        public bool updated;

        public StationaryBeaconsPositions()
        {
            numBeacons = new byte();
            beacons = new StationaryBeaconPosition[30];
            updated = new bool();
        }


    }
}
