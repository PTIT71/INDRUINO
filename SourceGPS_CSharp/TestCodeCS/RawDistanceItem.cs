using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCodeCS
{
    public class RawDistanceItem
    {
        public byte address_beacon;
        public float distance;// distance, mm

        public RawDistanceItem()
        {
            address_beacon = new byte();
            distance = new float();
        }
    }
}
