using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCodeCS
{
    class MarvelmindHedge
    {
        // buffer of measurements
        public PositionValue[] positionBuffer;

        public StationaryBeaconsPositions positionsBeacons;

        public RawIMUValue rawIMU;
        public FusionIMUValue fusionIMU;
        public RawDistances rawDistances;

        // verbose flag which activate console output
        //		default: False
        public bool verbose;

        //	pause flag. If True, class would not read serial data
        public bool pause;

        //  If True, thread would exit from main loop and stop
        public bool terminationRequired;

        public byte lastValuesCount_;
        public byte lastValues_next;
        public bool haveNewValues_;

        private MarvelmindHedge()
        {


            positionBuffer = new PositionValue[(int)Command.MAX_BUFFERED_POSITIONS];
            for (int i = 0; i < (byte)Command.MAX_BUFFERED_POSITIONS; i++)
            {
                positionBuffer[i] = new PositionValue();
            }
            positionsBeacons = new StationaryBeaconsPositions();

            rawIMU = new RawIMUValue();
            fusionIMU = new FusionIMUValue();
            rawDistances = new RawDistances();


            verbose = new bool();


            pause = new bool();

            terminationRequired = new bool();
            lastValuesCount_ = new byte();
            lastValues_next = new byte();
            haveNewValues_ = new bool();
        }
    }
}
