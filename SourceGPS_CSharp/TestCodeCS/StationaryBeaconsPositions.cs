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
            updated = new bool();
            beacons = new StationaryBeaconPosition[30];
            for(int i = 0; i < beacons.Length ;i++)
            {
                beacons[i] = new StationaryBeaconPosition();
            }
        }


    }
}
