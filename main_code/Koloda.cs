using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace project_cat_balatro
{
    class Koloda
    {
        Card[] cards;
        Card[] usedcards;
        Card[] cards_to_play = new Card[5]; // for combo
        int[] amountoftypes = new int[4];
        int usablecard = 0;
        public Koloda(int num, int type, Label lb, Label lb_count_point, Label label_plus)
        {
            Random rnd = new Random();

            cards = new Card[num];
            usedcards = new Card[num];

            usablecard = num;

            if (type == 1) {
                for (int i = 0; i < num; i++)
                {
                    cards[i] = new Card($"Cards\\1\\{i % 9 +1}_{i%4+1}.png", i%9, i%4, cards_to_play, lb, lb_count_point, label_plus);
                    amountoftypes[cards[i].getType()]++;
                }
            }
            else if(type == 2)
            {
                for (int i = 0; i < num; i++)
                {
                    cards[i] = new Card($"Cards\\2\\{i % 9 + 1}_{i % 4 + 1}.png", i % 9, i % 4, cards_to_play, lb, lb_count_point, label_plus);
                    amountoftypes[cards[i].getType()]++;
                }
            }
        }
        public void use_card(Card card)
        {
            for (int i = 0; i < cards.Length; i++) {
                if (card == cards[i]) {
                    card.getphoto().Width = 70;
                    card.getphoto().Height = 120;
                    card.getphoto().Opacity = 0.5;
                    cards[i] = null; 
                    usedcards[i] = card;
                    usablecard--;
                }
            }
        }
        public void restartForNewRoom()
        {
            usablecard = cards.Length;
            for (int i = 0; i < cards.Length; i++)
            {
                if (usedcards[i] == null) continue;
                cards[i] = usedcards[i];
                cards[i].getphoto().Opacity = 1;
                cards[i].getphoto().IsEnabled = true;
                usedcards[i] = null;

            }
        }
        public void change_hand(Grid grid)
        {
            Array.Clear(cards_to_play, 0, cards_to_play.Length);
            Debug.WriteLine($"i={usablecard}: tried all cards, none fit (unused/filter).");
            if (usablecard < 6)
            {
                usablecard = cards.Length;
                for (int i = 0; i < cards.Length; i++)
                {
                    if (usedcards[i] == null) continue;
                    cards[i] = usedcards[i];
                    cards[i].getphoto().Opacity = 1;
                    usedcards[i] = null;
                }
            }
            Random rnd = new Random();
            for (int i = 0; i < 6; i++)
            {
                if (!gridhaselemnt(grid, i))
                {
                    int num = rnd.Next(0, 36);
                    while (true)
                    {
                        if (isunusedcard(num) && !(grid.Children.Contains(cards[num].getphoto())))
                        {
                            grid.Children.Add(cards[num].getphoto());
                            Grid.SetColumn(cards[num].getphoto(), i);
                            break;
                        }
                        num++;
                        num = num % 36;
                    }
                }
            }
        }
        bool gridhaselemnt(Grid grid, int id)
        {
            return grid.Children
               .OfType<UIElement>()
               .Any(child => Grid.GetColumn(child) == id);
        }
        bool isunusedcard(int id)
        {
            if (cards[id] == null) return false;
            return true;
        }
        public Image get_card_img(int id)
        {
            return cards[id].getphoto();
        }
        public Card[] get_cards()
        {
            return cards;
        }
        public Card[] get_usedcards()
        {
            return usedcards;
        }
    }
}
