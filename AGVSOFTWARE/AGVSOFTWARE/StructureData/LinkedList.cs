using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSOFTWARE.StructureData
{
    public class LinkedList
    {
        public Node pHead, pTail;
        public Node currentNode;
        public bool isNew;

        public LinkedList()
        {
            this.pHead = new Node();
            this.pTail = new Node();
            this.currentNode = new Node();
            isNew = true;
        }
        public void addNodeHead(Node p)
        {
            if(this.isEmpty())
            {
                this.pHead = this.pTail = p;
                isNew = false;
            }
            else
            {
                p.nextNode = this.pHead;
                this.pHead.previousNode = p;
                this.pHead = p;
            }
        }

        public void addNodeTail(Node p)
        {
            if (this.isEmpty())
            {
                this.pHead = this.pTail = p;
                isNew = false;
            }
            else
            {
                this.pTail.nextNode = p;
                p.previousNode = this.pTail;
                this.pTail = p;
            }
        }
        public void addNode(Node p)
        {
            if (isEmpty())
            {
                this.pHead = this.pTail = p;
                this.currentNode = pHead;
                isNew = false;
            }
            else
            {
                this.pTail.nextNode = p;
                p.previousNode = this.pTail;
                this.pTail = p;
            }
        }


        public bool isEmpty()
        {
            if(isNew)
            {
                isNew = false;
                return true;
            }
            return false;
        }
    }
}
