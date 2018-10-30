using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AGVSOFTWARE.StructureData
{
    public class Map
    {
        //độ dịch chuyển so với map trước đó
        public float displacement { get; set; }
        public LinkedList nodeFrames { get; set; }

    

        public Map()
        {
            this.nodeFrames = new LinkedList();
        }

        public void addFrame(Frame frame)
        {
            this.nodeFrames.addNode(new Node(frame));
        }

    }
}
