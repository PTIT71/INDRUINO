using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCodeCS
{
    public class RawDistances
    {
        public byte address_hedge;
        public RawDistanceItem[] distances; // 4

        public bool updated;

        public RawDistances()
        {
            address_hedge = new byte();
            distances = new RawDistanceItem[4];
            updated = new bool();
        }
    }
}
