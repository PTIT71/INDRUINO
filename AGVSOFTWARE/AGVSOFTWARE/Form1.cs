using AGVSOFTWARE.AGVROBOT;
using AGVSOFTWARE.IndoorGPS;
using AGVSOFTWARE.RPLidar;
using AGVSOFTWARE.StructureData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGVSOFTWARE
{
    public partial class Form1 : Form
    {
        private Graphics _GrapOnPnl;
        private Graphics _GrapOnBm;
        private Bitmap _BmMap;

        Thread ThreadRPLidar;
        Thread ThreadGPS;

        AVGROBOTLIB AGV = new AVGROBOTLIB();

        System.Windows.Forms.Timer ProcessPaint = new System.Windows.Forms.Timer();

        LIDARLIB RPLIDAR = new LIDARLIB();

        public Form1()
        {
            InitializeComponent();

            _GrapOnPnl = pnlPaint.CreateGraphics();

           

            _BmMap = new Bitmap(pnlPaint.Width,pnlPaint.Height);

            _GrapOnBm = Graphics.FromImage(_BmMap);

            //AntiLiasing
            _GrapOnBm.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _GrapOnBm.Clear(Color.Black);

            ProcessPaint.Interval = 10;
            ProcessPaint.Tick += ProcessPaint_Tick;
        }

        private void ProcessPaint_Tick(object sender, EventArgs e)
        {
            DrawMap();
        }

        private void btnScanLidar_Click(object sender, EventArgs e)
        {
            StartThreadRPLidar();
          

        }

        void StartThreadRPLidar()
        {
           ProcessPaint.Start();
           ThreadRPLidar = new Thread(new ThreadStart(ProcessRPLidar));
           ThreadRPLidar.Start();
          
        }

        void ProcessRPLidar()
        {
            RPLIDAR.GetData();
        }

      
        
        void DrawMap()
        {
            _GrapOnBm.Clear(Color.Black);

            while (LIDARLIB.DataLIDARLL.currentNode.nextNode != null)
            {
                if(LIDARLIB.DataLIDARLL.currentNode.frame.Distance<700)
                {
                   // if(LIDARLIB.DataLIDARLL.currentNode.frame.AngleDegrees>340 && LIDARLIB.DataLIDARLL.currentNode.frame.AngleDegrees > 360 || LIDARLIB.DataLIDARLL.currentNode.frame.AngleDegrees > 0 && LIDARLIB.DataLIDARLL.currentNode.frame.AngleDegrees <20)
                  //  {
                        AGV.CheckHEAD(LIDARLIB.DataLIDARLL.currentNode);
                  //  }
                }

                AGV.CheckGeneralWarning();


                LIDARLIB.DataLIDARLL.currentNode.frame.DrawDot(_GrapOnBm, (float)0.5);

                LIDARLIB.DataLIDARLL.currentNode = LIDARLIB.DataLIDARLL.currentNode.nextNode;

            }

            while(INDOORGPSLIB.DataGPSLL.currentNode.nextNode!=null)
            {
                INDOORGPSLIB.DataGPSLL.currentNode.frame.DrawDot(_GrapOnBm, (float)0.5);
            }

            _GrapOnPnl.DrawImage(_BmMap, 0, 0);
            Thread.Sleep(100);
        }

        private void btnGetGPS_Click(object sender, EventArgs e)
        {
            ProcessPaint.Start();
            StartThreadGPS();
        }

        private void btnCloseRpLidar_Click(object sender, EventArgs e)
        {
            RPLIDAR.Stop();
        }

        void StartThreadGPS()
        {
            ThreadGPS = new Thread(new ThreadStart(ProcessGPS));
            ThreadGPS.Start();

        }

        void ProcessGPS()
        {
            INDOORGPSLIB GPS = new INDOORGPSLIB();
        }
    }
}
