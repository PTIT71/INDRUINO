using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSOFTWARE.StructureData
{
    public class Node
    {
        public Frame frame { get; set; }
        public Node nextNode { get; set; }
        public Node previousNode { get; set; }

        public Node()
        {
            this.frame = new Frame();
            this.nextNode = null;
            this.previousNode = null;
        }
        public Node(Frame frame)
        {
            this.frame = frame;
            this.nextNode = null;
            this.previousNode = null;
        }
    }
}
