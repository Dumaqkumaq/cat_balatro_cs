using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace project_cat_balatro
{
    class BonusCard : Common_for_all_cards
    {
        public int effect;
        public string description { get; set; }
        public int status = 0;
        public int cost = 0;
        public BonusCard()
        {
            effect = 0;
        }
        public BonusCard(string path, int number_effect, string desc) : base(path)
        {
            description = desc;
            effect = number_effect;
        }
 
       
        public int getEffect() { return effect; }
        public void setEffect(int num) { effect = num; }

        public override void print()
        {
            Console.WriteLine($"Card :: Photo: {base.getphoto() != null}, Effect: {this.effect}, Desc: {this.description}");
        }
        public override string ToString()
        {
            return $"Photo: {base.getphoto() != null}\n Effect: {this.effect}\n Desc: {this.description}";
        }
    }
}
