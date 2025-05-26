using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_cat_balatro
{
    class Edge
    {
        public Node A {  get; set; }
        public Node B { get; set; }
        public Edge(Node a, Node b) {
            A = a;
            B = b;
        }
    }
}
