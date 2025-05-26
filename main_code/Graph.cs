using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_cat_balatro
{
    class Graph
    {
        private readonly Dictionary<int, Node> nodes = new Dictionary<int, Node>();
        public IReadOnlyCollection<Node> Nodes => nodes.Values;

        public Graph(Dictionary<int, Node> nodes)
        {
            this.nodes = nodes;
        }
        public Graph() { }
        public Node addNode(int id)
        {
            if(!nodes.ContainsKey(id))
            {
                nodes[id] = new Node(id);
            }
            return nodes[id];
        }
        public void addEdge(int ida, int idb)
        {
            Node a = addNode(ida);
            Node b = addNode(idb);
            if(!a.neighbors.Contains(b)) a.neighbors.Add(b);
            if (!b.neighbors.Contains(a)) b.neighbors.Add(a);
        }
        public Node getNode(int id) => nodes.TryGetValue(id, out Node node) ? node : null;
    }
}
