using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace project_cat_balatro
{
    class Node
    {
        public int id { get; }
        public int type { get; set; }
        public int layer { get; set; }
        public int idInLayer { get; set; }
        public int nodeType { get; set; }
        public System.Windows.Point Position { get; set; }
        public List<Node> neighbors { get;  } = new List<Node>();
        public System.Windows.Controls.Image photo = new System.Windows.Controls.Image();
        public Label lb;
        public Node(int id)
        {
            this.id = id;
            type = 0;
        }
        
    }
}
