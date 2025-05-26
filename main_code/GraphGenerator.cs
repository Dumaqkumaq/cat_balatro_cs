using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_cat_balatro
{
    class GraphGenerator
    {
        public static Graph GenerateRandomGraph(int nodeCount, int edgeCount)
        {
            Random rnd = new Random();
            Graph graph = new Graph();

            for (int i = 0; i < nodeCount; i++) { 
                graph.addNode(i);
            }

            int added = 0;
            while (added < edgeCount) { 
                int a = rnd.Next(nodeCount);
                int b = rnd.Next(nodeCount);
                if (a == b) { continue; }
                Node nodeA = graph.getNode(a);
                Node nodeB = graph.getNode(b);

                if (!nodeA.neighbors.Contains(nodeB)) { 
                    graph.addEdge(a,b);
                    added++;
                }
            }

            return graph;
        }

        public static Graph GenerateLayeredMap(int[] nodesPerLayer, int pathCount, int mergeCount)
        {
            var rand = new Random();
            var graph = new Graph();
            int L = nodesPerLayer.Length;

            // 1) создаём все узлы по слоям
            var layers = new List<List<Node>>(L);
            for (int layer = 0; layer < L; layer++)
            {
                var list = new List<Node>(nodesPerLayer[layer]);
                for (int idx = 0; idx < nodesPerLayer[layer]; idx++)
                {
                    int nodeId = layer * 1000 + idx;   // например, уникальный id
                    var node = graph.addNode(nodeId);
                    node.layer = layer;
                    node.idInLayer = idx;
                    list.Add(node);
                }
                layers.Add(list);
            }

            // 2) выбираем mergeCount слоёв (кроме 0 и L-1), где пути пересекутся
            var mergeLayers = new HashSet<int>();
            while (mergeLayers.Count < Math.Min(mergeCount, L - 2))
            {
                int pick = rand.Next(1, L - 1);
                mergeLayers.Add(pick);
            }

            // 3) для каждого merge-слоя заранее выбираем «общий» узел
            var sharedNodeAt = new Dictionary<int, Node>();
            foreach (int ml in mergeLayers)
                sharedNodeAt[ml] = layers[ml][rand.Next(layers[ml].Count)];

            // 4) теперь строим каждый из pathCount путей
            for (int p = 0; p < pathCount; p++)
            {
                // pathNodes[i] = тот узел в слое i, через который идёт путь p
                var pathNodes = new Node[L];

                // для каждого слоя выбираем либо общий, либо свой
                for (int i = 0; i < L; i++)
                {
                    if (sharedNodeAt.TryGetValue(i, out var shared))
                    {
                        pathNodes[i] = shared;
                    }
                    else
                    {
                        var choices = layers[i];
                        pathNodes[i] = choices[rand.Next(choices.Count)];
                    }
                }

                // 5) соединяем их рёбрами (н е о р и е н т и р о в а н н о)
                for (int i = 0; i < L - 1; i++)
                {
                    int idA = pathNodes[i].id;
                    int idB = pathNodes[i + 1].id;
                    graph.addEdge(idA, idB);
                }
            }

            return graph;
        }
    }
}
