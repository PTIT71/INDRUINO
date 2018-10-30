using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCodeCS
{
    public enum Command : UInt16
    {
        MAX_STATIONARY_BEACONS = 30,

        MAX_BUFFERED_POSITIONS = 3,

        POSITION_DATAGRAM_ID = 0x0001 ,
        BEACONS_POSITIONS_DATAGRAM_ID = 0x0002,
        POSITION_DATAGRAM_HIGHRES_ID = 0x0011,
        BEACONS_POSITIONS_DATAGRAM_HIGHRES_ID = 0x0012,
        IMU_RAW_DATAGRAM_ID = 0x0003,
        BEACON_RAW_DISTANCE_DATAGRAM_ID = 0x0004,
        IMU_FUSION_DATAGRAM_ID = 0x0005,
    }

    public enum RECV
    {
        RECV_HDR,
        RECV_DGRAM
    }
}
