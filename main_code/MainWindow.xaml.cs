using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Threading;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Animation;
using System.Security.Cryptography;
using Label = System.Windows.Controls.Label;
using System.Timers;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using System.Windows.Threading;
using System.Reflection;

namespace project_cat_balatro
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //
        //
        //
        static int numbercards = 36;

        Card[] choosencards;
        static Koloda koloda;
        Card[] showcards;
        Label[] lbarr;
        Label[] bonuslabel;
        //BonusCard love = new BonusCard("love.png", 0, "forgotten love");
        BonusCard[] bonuses;
        BonusCard[] allBonuses;
        BonusCard[] forshop;
        int restart_amount;
        int play_amount;
        Enemy[] enemies;
        int basePlayCount;
        int baseRestartCount;
        int gold_player;
        bool beatedboss = false;
        int allenemydefeated;
        int allgoldcollected;
        int allhandplayed;
        int allrestartplayed;
        //graph
        Graph graph;
        int currentlayer = 0;

        bool boosfl = false;
        Label plus_gold = new Label();
        Node currentNode;
        System.Windows.Controls.Image currentNodePhoto = new System.Windows.Controls.Image();
        int global_debuff = 0;  // 0 -> without debuf
        backgroundsong meow = new backgroundsong();
        //
        //
        //
        public MainWindow()
        {
            InitializeComponent();
            backgroundsong song = new backgroundsong();
            song.InitializeBackgroundMusic("Res\\Music\\123.mp3");

        }
        private void start_game(int numKoloda)
        {
            allBonuses = new BonusCard[7];
            forshop = new BonusCard[7];
            choosencards = new Card[6];
            lbarr = new Label[6];
            enemies = new Enemy[5];
            bonuses = new BonusCard[4];
            bonuslabel = new Label[4];

            allenemydefeated = 0 ;
             allgoldcollected = 0;
             allhandplayed = 0;
             allrestartplayed = 0;
            boosfl = false;

            allBonuses[0] = new BonusCard("BonusCards\\love.png", 0, "additional points");
            allBonuses[1] = new BonusCard("BonusCards\\kittiboom.jpg", 1, "additional multiply");
            allBonuses[2] = new BonusCard("BonusCards\\hahah.jpg", 2, "additional restart");
            allBonuses[3] = new BonusCard("BonusCards\\darkbell.jpg", 3, "additional play");
            allBonuses[4] = new BonusCard("BonusCards\\sleepykitti.jpg", 4, "decrease hp");
            allBonuses[5] = new BonusCard("BonusCards\\titancloaud.jpg", 5, "more gold");
            allBonuses[6] = new BonusCard("BonusCards\\umbrellacats.jpg", 6, "additional points and multiply");


            canvas_map.Children.Add(plus_gold);
            Canvas.SetTop(plus_gold, Canvas.GetTop(label_moneyPlayer) - 20);
            Canvas.SetLeft(plus_gold, Canvas.GetLeft(label_moneyPlayer));
            BitmapImage bitmap = new BitmapImage();

            if (numKoloda == 1)
            {
                koloda = new Koloda(numbercards, 1, label_show_combo, label_count_point, label_plus);

                basePlayCount = 4;
                 baseRestartCount = 4;
                gold_player = 100;
            }
            else if (numKoloda == 2)
            {
                koloda = new Koloda(numbercards, 2, label_show_combo, label_count_point, label_plus);

                basePlayCount = 2;
                baseRestartCount = 6;
                 gold_player = 300;
            }
            else if (numKoloda == 3) {
                koloda = new Koloda(numbercards, 2, label_show_combo, label_count_point, label_plus);
                basePlayCount = 2;
                 baseRestartCount = 2;
                 gold_player = 1000;
            }

                try
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri($"E:\\cc\\project-cat-balatro\\Res\\Photo\\Node\\playerArrow.png");
                    bitmap.EndInit();
                    currentNodePhoto.Source = bitmap;
                    currentNodePhoto.Width = 50;
                    currentNodePhoto.Height = 50;
                    currentNodePhoto.Opacity = 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error happened!\nLoading photo caused an error: {ex.Message}");
                }
            canvas_map.Children.Add(currentNodePhoto);
            Canvas.SetZIndex(currentNodePhoto, 3);

            restart_amount = baseRestartCount;
            play_amount = basePlayCount;
            showcards = koloda.get_cards();
            //
            // PREPARED ENEMIES
            //
            enemies[0] = new Enemy("Enemies\\enem1.png", 300);
            enemies[1] = new Enemy("Enemies\\enem2.png", 400);
            enemies[2] = new Enemy("Enemies\\enem3.png", 600);
            enemies[3] = new Enemy("Enemies\\boss0.png", 1000);
            enemies[4] = new Enemy("Enemies\\boss1.png", 1000);
            //grid_bonus.Children.Add(love.getphoto());


            //initilization place
            for (int i = 0; i < 6; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                grid_hand.ColumnDefinitions.Add(col);

            }
            grid_checkcards.ShowGridLines = true;
            for (int i = 0; i < numbercards / 4; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                grid_checkcards.ColumnDefinitions.Add(col);
            }
            for (int i = 0; i < 4; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                grid_checkcards.RowDefinitions.Add(row);

                ColumnDefinition col1 = new ColumnDefinition();
                col1.Width = new GridLength(1, GridUnitType.Star);
                grid_bonus.ColumnDefinitions.Add(col1);

                ColumnDefinition col2 = new ColumnDefinition();
                col2.Width = new GridLength(1, GridUnitType.Star);
                grid_bonus_showamount.ColumnDefinitions.Add(col2);
               
                bonuslabel[i] = new Label();
                grid_bonus_showamount.Children.Add(bonuslabel[i]);
                Grid.SetColumn(bonuslabel[i], i);
                bonuslabel[i].Content = "";
            }
            for (int i = 0; i < 3; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                grid_market.RowDefinitions.Add(row);
                
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                grid_market.ColumnDefinitions.Add(col);
            }


            //ADDING BONUS
            refresh_bonuses();
            label_playamount.Content = play_amount;
            grid_bonus.IsEnabled = true;
            label_moneyPlayer.Content = gold_player;

            grid_Map.Visibility = Visibility.Visible;
            grid_chooseKoloda.Visibility = Visibility.Collapsed;

            restarthand();

            graph = GraphGenerator.GenerateLayeredMap(new[] { 1, 3, 4, 3, 5, 1 }, 4, 1);
            //List<List<Node>> layers = graph.Nodes // <-- конфликт с LayoutNodesOnCanvas()
            //                                .GroupBy(n => n.layer)
            //                                .OrderBy(g => g.Key)
            //                                .Select(g => g.ToList())
            //                                .ToList();
            layoutNodesOnCanvas();
            renderEdges();
            renderNodes();
            currentlayer = 0;
            var nodes = getActiveNodesAtLayer(activeNodes, 0);
            foreach (var node in nodes)
            {
                node.photo.IsEnabled = true;
                currentNode = node;
            }
            currentNodePhoto.Opacity = 1;
            Canvas.SetTop(currentNodePhoto, currentNode.Position.Y - 55);
            Canvas.SetLeft(currentNodePhoto, currentNode.Position.X - 20);

            
        }
        private void refresh_bonuses()
        {
            int k = 0;
            foreach (BonusCard bcard in bonuses)
            {
                if (bcard == null) continue;
                try
                {
                    grid_bonus.Children.Add(bcard.getphoto());
                    Grid.SetColumn(bcard.getphoto(), k);
                }
                catch (Exception e) { }
                
                k++;
            }
        }
        
        //
        // BUTTONS
        //
        private void button_restarthand_click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            allrestartplayed++;
            label_show_combo.Content = "";
            label_count_point.Content = "";
            label_plus.Content = "";
            bool anycard = false;
            if (restart_amount != 0)
            {
                foreach (Card card in koloda.get_cards())
                {
                    if(card == null) continue;
                    card.setAmount(card.baseAmount);
                    if (card.choose == true)
                    {
                        card.choose = false;
                        card.setAmount(card.baseAmount);
                        anycard = true;
                        int num = Grid.GetColumn(card.getphoto());
                        grid_hand.Children.Remove(card.getphoto());
                        koloda.use_card(card);
                    }
                }
                koloda.change_hand(grid_hand);
                if(anycard) restart_amount--;
            }
            
            label_restart.Content = restart_amount;
        }

        private void button_show_koloda_click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            
            if (grid_checkcards.Visibility == Visibility.Visible) {
                grid_bonus.Visibility = Visibility.Visible;
                grid_checkcards.Visibility = Visibility.Collapsed;
                label_debuff.Visibility = Visibility.Visible;
                button_sort_by_amount.IsEnabled = true;
                button_sort_by_color.IsEnabled = true;
                button_play_hand.IsEnabled = true;
                int j = 0;
                foreach (Card card in koloda.get_cards())
                {
                    if(card == null) continue;
                    try
                    {
                        grid_checkcards.Children.Remove(card.getphoto());
                    }
                    catch (Exception ex) { }
                    card.getphoto().IsEnabled = true;
                }
                foreach (Card card in koloda.get_usedcards())
                {
                    if (card == null) continue;
                    try
                    {
                        grid_checkcards.Children.Remove(card.getphoto());
                    }
                    catch (Exception ex) { }
                    
                    card.getphoto().IsEnabled = true;
                }
                foreach(Card card in choosencards)
                {
                    if(card == null) continue;
                    grid_hand.Children.Add(card.getphoto());
                    Grid.SetColumn(card.getphoto(), j);
                    j++;
                }
            } else
            {
                int j = 0;
                grid_bonus.Visibility = Visibility.Collapsed;
                label_debuff.Visibility = Visibility.Collapsed;
                grid_checkcards.Visibility = Visibility.Visible;
                button_sort_by_amount.IsEnabled = false;
                button_sort_by_color.IsEnabled = false;
                button_play_hand.IsEnabled = false ;
                foreach (Card card in koloda.get_cards())
                {
                    if (card == null) continue;
                    if (grid_hand.Children.Contains(card.getphoto()))
                    {
                        grid_hand.Children.Remove(card.getphoto());
                        choosencards[Grid.GetColumn(card.getphoto())] = card;
                    }
                    try
                    {
                        grid_hand.Children.Remove(card.getphoto());
                    }
                    catch (Exception ex) { }
                    grid_checkcards.Children.Add(card.getphoto());
                    Grid.SetColumn(card.getphoto(), card.getAmount());
                    Grid.SetRow(card.getphoto(), card.getType());
                    card.getphoto().IsEnabled = false;
                }
                foreach (Card card in koloda.get_usedcards())
                {
                    if (card == null) continue;
                    
                    grid_checkcards.Children.Add(card.getphoto());
                    Grid.SetColumn(card.getphoto(), card.getAmount());
                    Grid.SetRow(card.getphoto(), card.getType());
                    card.getphoto().IsEnabled = false;
                }
            }
        }
        private void button_play_hand_click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            allhandplayed++;
            button_play_hand.IsEnabled = false;
            button_sort_by_amount.IsEnabled = false;
            button_sort_by_color.IsEnabled = false;
            grid_hand.IsEnabled = false;

            int points = 0;
            int hp = int.Parse(label_enemy_hp.Content.ToString());
            int[] amountes = new int[9];
            int i = 0;
            bool anycard = false;
            int koef = 0;
            
            foreach (Card card in koloda.get_cards())
            {
                if (card == null) continue;
                if (card.choose == true)
                {
                    anycard = true;
                    
                    //if(global_debuff == 1)
                    //{
                    //    if((card.getType() == 1) || (card.getType() == 2)) {
                    //        card.setAmount(-1);
                    //    }
                    //}
                    //else if (global_debuff == 2)
                    //{
                    //    if ((card.getType() == 0) || (card.getType() == 3))
                    //    {
                    //        card.setAmount(-1);
                    //    }
                    //}
                    //else
                    //{
                    //    amountes[card.getAmount()]++;
                    //}
                    amountes[card.getAmount()]++;
                    i++;
                }
            }
            if (!anycard) {
                button_play_hand.IsEnabled = true;
                button_sort_by_amount.IsEnabled = true;
                button_sort_by_color.IsEnabled = true;
                grid_hand.IsEnabled = true;
                allhandplayed--;
            }
            move_cards_to_show(amountes);
            string str = label_show_combo.Content.ToString();
            if (str == "Пара и трио")
            {
                points = 40;
                koef = 5;
                points += (Array.IndexOf(amountes, 2) + 1) * 2 + (Array.IndexOf(amountes, 3) + 1) * 3;
                
            }
            else if (str == "Трио")
            {
                points = 30;
                koef = 3;
                points += (Array.IndexOf(amountes, 3) + 1) * 4;
            }
            else if (str == "Две пары")
            {
                points = 25;
                koef = 3;
                points += (Array.LastIndexOf(amountes, 2) + 1) * 2;
                amountes[Array.LastIndexOf(amountes, 2)] = 0;
                points += (Array.LastIndexOf(amountes, 2) + 1) * 2;

            }
            else if (str == "Пара")
            {
                points = 20;
                koef = 2;
                points += (Array.IndexOf(amountes, 2) + 1) * 2;
            }
            else if (str == "Одиночка")
            {
                points = 10;
                koef = 1;
                points += ((Array.LastIndexOf(amountes, 1)+1) * koef);
            }
            //calculating bonus
            
            int k = 0;
            foreach (BonusCard bcard in bonuses)
            {
                if (bcard == null) continue; 
                if (bcard.getEffect() == 0)
                {
                    points += 20;
                    bonuslabel[k].Content = "+20";
                }
                if(bcard.getEffect() == 1)
                {
                    koef += 2;
                    bonuslabel[k].Content = "*(+2)";
                }
                if (bcard.getEffect() == 6)
                {
                    koef += 2;
                    points += 10;
                    bonuslabel[k].Content = "*(+3)+10";
                }
                k++;

            }
            label_plus.Content = points;
            points *= koef;
            if (global_debuff == 1)
            {
                if(points < 150)
                {
                    points = 0;
                }
            }
            

            
            label_count_point.Content = " *"+koef.ToString();
            label_countedpoints.Content = points;
            label_countedpoints.Opacity = 0;
            if (anycard) play_amount--;

            label_playamount.Content = play_amount;

            //label_count_point.Content += "\n=" + points.ToString();
            label_enemy_hp.Content = hp - points;
        }
        //
        //  sort hand buttons
        //
        private void button_sort_by_amount_Click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            int j = 0;
            foreach (Card card in koloda.get_cards())
            {
                if (card == null) continue;
                if (card.choose == true)
                {
                    card.change_state();
                }
                if (grid_hand.Children.Contains(card.getphoto()))
                {
                    grid_hand.Children.Remove(card.getphoto());
                    choosencards[j] = card;
                    j++;
                }
            }
            Array.Sort(choosencards, (x, y) => x.getAmount().CompareTo(y.getAmount()));
            foreach (Card card in choosencards)
            {
                j--;
                grid_hand.Children.Add(card.getphoto());
                Grid.SetColumn(card.getphoto(), j);

            }
        }
        private void button_sort_by_color_Click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            int j = 0;
            foreach (Card card in koloda.get_cards())
            {
                if (card == null) continue;
                if (card.choose == true)
                {
                    card.change_state();
                }
                if (grid_hand.Children.Contains(card.getphoto()))
                {
                    grid_hand.Children.Remove(card.getphoto());
                    choosencards[j] = card;
                    j++;
                }
            }
            Array.Sort(choosencards, (x, y) => x.getType().CompareTo(y.getType()));
            foreach (Card card in choosencards)
            {
                j--;
                grid_hand.Children.Add(card.getphoto());
                Grid.SetColumn(card.getphoto(), j);

            }
        }
        //
        //  ANIMATION(and functions for it) FOR PLAYED CARDS
        //
        private void move_cards_to_show(int[] amountes)
        {
            int j = 0;
            bool fl1 = true;
            choosencards = new Card[6];
            foreach (Card card in koloda.get_cards())
            {
                if (card == null) continue;
                if (card.choose == true)
                {
                    choosencards[j] = card;
                    fl1 = false;
                    j++;
                    
                }
            }
             j = 0;
            foreach(Card card in choosencards)
            {
                if (card == null) continue;j++;
                int pos = Grid.GetColumn(card.getphoto());
                grid_hand.Children.Remove(card.getphoto());
                //Canvas.Left="230" Stroke="Black" Canvas.Top="160"
                canvas.Children.Add(card.getphoto());
                //Canvas.Left="239" Canvas.Top="293"
                Canvas.SetLeft(card.getphoto(), 239 + 40 * j);
                Canvas.SetTop(card.getphoto(), 293);

                DoubleAnimation dbah = new DoubleAnimation
                {
                    From = 293,
                    To = 160,
                    Duration = TimeSpan.FromSeconds(1)
                };
                dbah.Completed += complete_animation_card;
                //Canvas.Left="239" Canvas.Top="293" Width="500"
                DoubleAnimation dbaw = new DoubleAnimation
                {
                    From = 239 + 40 * j,
                    To = 230 + 40 * j,
                    Duration = TimeSpan.FromSeconds(1.5)
                };

                card.normphoto();
                card.getphoto().IsEnabled = false;

                card.getphoto().BeginAnimation(Canvas.TopProperty, dbah);
                card.getphoto().BeginAnimation(Canvas.LeftProperty, dbaw);

                bool fl = false;
                string str = label_show_combo.Content.ToString();
                if (str == "Пара и трио")
                {
                    fl = true;
                }
                else if (str == "Трио")
                {
                    if (amountes[card.baseAmount] == 3) fl = true;
                }
                else if (str == "Две пары")
                {
                    if (amountes[card.baseAmount] == 2) fl = true;
                }
                else if (str == "Пара")
                {
                    if (amountes[card.baseAmount] == 2) fl = true;
                }
                else if (str == "Одиночка") 
                {
                    if (card.baseAmount == Array.LastIndexOf(amountes, 1)) fl = true;
                }

                show_label_amount(card, fl);
                j++;
            }

        }
        private void show_label_amount(Card card, bool fl)
        {
            if (!fl) return;
            Label lb_amount = new Label();
            lbarr[Array.IndexOf(lbarr, null)] = lb_amount;
            canvas.Children.Add(lb_amount);
            lb_amount.Content = "+" + (card.getAmount() + 1).ToString();
            lb_amount.FontSize = 15;
            lb_amount.Opacity = 1;
            Canvas.SetTop(lb_amount, 160); // --> 140
            Canvas.SetLeft(lb_amount, Canvas.GetLeft(card.getphoto()));
            DoubleAnimation dba = new DoubleAnimation
            {
                From = 160,
                To = 140,
                Duration = TimeSpan.FromSeconds(1)
            };
            dba.Completed += plus_point;
            lb_amount.BeginAnimation(Canvas.TopProperty, dba);
        }
        private void plus_point(object sender, EventArgs e)
        {
            label_plus.Content = "";
            label_count_point.Content = "";
            label_countedpoints.Opacity = 1;
            DoubleAnimation dba = new DoubleAnimation
            {
                From = Canvas.GetTop(label_countedpoints),
                To = Canvas.GetTop(label_countedpoints),
                Duration = TimeSpan.FromSeconds(2)
            };
            dba.Completed += end;
            label_countedpoints.BeginAnimation(Canvas.TopProperty, dba);

            restarthand(true);

        }
        private void winedGame()
        {
            label_allgoldcollected.Content = allgoldcollected.ToString();
            label_enemydefeated.Content = allenemydefeated.ToString();
            label_handplayed.Content = allhandplayed.ToString();
            label_restarthandmade.Content = allrestartplayed.ToString();
            grid_PlayRoom.Opacity = 0.8;
            grid_PlayRoom.IsEnabled = false;
            grid_EndGame.Visibility = Visibility.Visible;
            global_debuff = 0;

            if (allgoldcollected > 300)
            {
                rectangle_koloda3.Visibility = Visibility.Collapsed;
            }

        }
        private void check_gamestatus()
        {
            if ((int.Parse(label_enemy_hp.Content.ToString())) <= 0)
            {
                allenemydefeated++;
                if (boosfl)
                {
                    winedGame();
                    if (!beatedboss)
                    {
                        rectangle_koloda2.Visibility = Visibility.Collapsed;
                    }
                    global_debuff = 0;
                    return;
                }

                grid_PlayRoom.Visibility = Visibility.Collapsed;
                grid_Map.Visibility = Visibility.Visible;
                grid_bonus.Visibility = Visibility.Collapsed;
                nextLayer();
                global_debuff = 0;
                fl1 = false;
                label_debuff.Content = "";
                grid_enemy.Children.Clear();
                grid_enemy.Children.Add(label_enemy_hp);
                label_moneyPlayer.Content = gold_player;
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(4);
                timer.Tick += erase_plusgold;
                timer.Start();
                //return 1;
            }
            else if (play_amount == 0)
            {
                winedGame();
                //return 2;
            }
            //return 0;
        }
        bool fl1 = true;
        private void end(object sender, EventArgs e)
        {
            label_countedpoints.Content = "";
            if (fl && fl1)
            {
                check_gamestatus();
            }
            grid_hand.IsEnabled = true;
            button_play_hand.IsEnabled = true;
            button_sort_by_amount.IsEnabled = true;
            button_sort_by_color.IsEnabled = true;
        }
        private void complete_animation_card(object sender, EventArgs e)
        {
            fl = false;
            Thread.Sleep(300); // <-- change on smt better
            foreach (Label lb in lbarr) {
                if (lb != null) {
                    lb.BeginAnimation(Canvas.TopProperty, null);
                    canvas.Children.Remove(lb);
                }
            }
           lbarr = new Label[6];
           foreach(Card card in choosencards)
            {
                if (card == null) continue;
                card.getphoto().BeginAnimation(Canvas.TopProperty, null);
                card.getphoto().BeginAnimation(Canvas.LeftProperty, null);
                canvas.Children.Remove(card.getphoto());
            }
           foreach(Label lb in bonuslabel)
            {
                lb.Content = "";
            }
            fl = true;
        }
        bool fl = false;

        //
        // ADDITIONAL RESTART HAND(without changing amount of restarting)
        //
        private void restarthand(bool fl = false)
        {
            label_show_combo.Content = "";
            if ((restart_amount != 0) || fl)
            {
                foreach (Card card in koloda.get_cards())
                {
                    if (card == null) continue;
                    card.setAmount(card.baseAmount);
                    if (card.choose == true)
                    {
                        card.choose = false;
                        card.setAmount(card.baseAmount);
                        int num = Grid.GetColumn(card.getphoto());
                        grid_hand.Children.Remove(card.getphoto());
                        koloda.use_card(card);
                    }
                }
                koloda.change_hand(grid_hand);
            }

            label_restart.Content = restart_amount;
        }

        private void button_settingsinplayroom_Click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            grid_PlayRoom.IsEnabled = false;
            grid_SettingsWhileInPlayRoom.Visibility = Visibility.Visible;
            
        }

        private void button_backFromSetINTOplayroom_Click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            grid_PlayRoom.IsEnabled = true;
            grid_SettingsWhileInPlayRoom.Visibility = Visibility.Collapsed;
        }

        private void erase_plusgold(object sender, EventArgs e)
        {
            plus_gold.Content = "";
            DispatcherTimer timer = sender as DispatcherTimer;
            timer.Stop();
        }
        void restart_playrrom(int hp, int Getgol, int id)
        {
            fl1 = true;

            restart_amount = baseRestartCount;
            play_amount = basePlayCount;

            //allBonuses[2] = new BonusCard("BonusCards\\hahah.jpg", 2, "additional restart");
            //allBonuses[3] = new BonusCard("BonusCards\\darkbell.jpg", 3, "additional play"); 
            //allBonuses[4] = new BonusCard("BonusCards\\sleepykitti.jpg", 4, "decrease hp"); 
            //allBonuses[5] = new BonusCard("BonusCards\\titancloaud.jpg", 5, "more gold");
            int k = 0;
            foreach (BonusCard bcard in bonuses)
            {
                if (bcard == null) continue;
                if (bcard.getEffect() == 2)
                {
                    bonuslabel[k].Content = "+СБРОС";
                    restart_amount++;
                }
                if (bcard.getEffect() == 3)
                {
                    bonuslabel[k].Content = "+PLAY";
                    play_amount++;
                }
                if (bcard.getEffect() == 4)
                {
                    bonuslabel[k].Content = "-HP";
                    hp -= (int)Math.Floor(hp*0.2);
                }
                if (bcard.getEffect() == 5)
                {
                    bonuslabel[k].Content = "+GOLD";
                    Getgol += Getgol / 2;
                }
                k++;

            }

            gold_player += Getgol;
            allgoldcollected += Getgol;

            plus_gold.Content = "+" + Getgol.ToString();

            if (enemies[id].buff == 1)
            {
                label_debuff.Content = "-2 СБРОСА";
                restart_amount -= 2;
            }
            else if (enemies[id].buff == 2)
            {
                label_debuff.Content = "-1 РУКА";
                play_amount--;
            }
            else if (enemies[id].buff == 3)
            {
                label_debuff.Content = "БЕЗ СБРОСОВ\n+2 РУКИ";
                restart_amount = 0;
                play_amount += 2;
            }
            else if(id == 3)
            {
                label_debuff.Content = "УРОН >150";
                global_debuff = 1;
            }
            else if (id == 4)
            {
                label_debuff.Content = "РУКИ=СБРОС";
                play_amount = restart_amount;
            }

            enemies[id].buff = 0;
            enemies[id].set_hp(hp);
            koloda.restartForNewRoom();

            
            foreach (Card card in koloda.get_cards()) {
                card.choose = false;
                try
                {
                    grid_hand.Children.Remove(card.getphoto());
                }
                catch (Exception ex) { }
                try
                {
                    canvas.Children.Remove(card.getphoto());
                }
                catch (Exception ex) { }

            }
            label_playamount.Content = play_amount;
            label_restart.Content = restart_amount;
            if(restart_amount == 0) { restarthand(true); }
            else { restarthand(); }
            
        }
        /// 
        /// ОТРИСОВКА КАРТЫ
        /// 
        private void window_loaded(object sender, RoutedEventArgs e)
        {
            

        }

        private void layoutNodesOnCanvas()
        {
            double canvasWidth = 600;
            double canvasHeight = 300;
            //если канвас не отрисован
            if (canvasWidth == 0 || canvasHeight == 0)
            {
                canvas_map.Loaded += (s, e) => { layoutNodesOnCanvas(); renderEdges(); renderNodes(); };
                return;
            }

            var layers = graph.Nodes.GroupBy(x => x.layer)
                                    .OrderBy(g => g.Key)
                                    .Select(g => g.ToList())
                                    .ToList();
            reorderLayers(layers, 2);

            int L = layers.Count;
            double vSpacing = (L > 1) ? canvasWidth / (L - 1) : 0;

            for (int i = 0; i < L; i++)
            {
                var layer = layers[i];
                int count = layer.Count;
                double hSpacing = (count > 1) ? canvasHeight / (count - 1) : canvasHeight / 2;

                for (int j = 0; j < count; j++)
                {
                    Node node = layer[j];
                    node.Position = new Point(40 + vSpacing * i, 40 + hSpacing * j + 20);

                    node.idInLayer = j;
                }
            }
        }

        private void renderEdges()
        {
            //что
            HashSet<(int, int)> drawn = new HashSet<(int, int)>();

            foreach (Node node in graph.Nodes)
            {
                foreach (Node nei in node.neighbors)
                {
                    var key = node.id < nei.id ? (node.id, nei.id) : (nei.id, node.id);
                    if (drawn.Contains(key)) continue;
                    drawn.Add(key);
                    var line = new Line
                    {
                        X1 = node.Position.X,
                        Y1 = node.Position.Y,
                        X2 = nei.Position.X,
                        Y2 = nei.Position.Y,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };
                    canvas_map.Children.Add(line);
                }
            }
        }
        /// <summary>
        /// Возвращает все узлы из activeNodes, у которых Layer == layerIndex.
        /// </summary>
        private static List<Node> getActiveNodesAtLayer(
            IEnumerable<Node> activeNodes, int layerIndex)
        {
            return activeNodes
                .Where(n => n.layer == layerIndex)
                .ToList();
        }
        HashSet<Node> activeNodes;
        private void renderNodes()
        {
            Random rnd = new Random();

            int minLayer = graph.Nodes.Min(n => n.layer);
            int maxLayer = graph.Nodes.Max(n => n.layer);

            Node startNode = graph.Nodes.Single(n => n.layer == minLayer);
            Node endNode = graph.Nodes.Single(n => n.layer == maxLayer);

            var reachableFromEnd = getReachable(endNode);
            var reachableFromStart = getReachable(startNode);

            activeNodes = reachableFromStart.Intersect(reachableFromEnd).ToHashSet();

            var activeEdges = new HashSet<(Node u, Node v)>();

            // enemy = 1
            // shop = 2
            // unknown = 3
            // elite enemy = 4
            // finalboss = 5
            //{ 1, 3, 4, 3, 5, 1 }
            int[] level = {1, 3, 3, 4, 2, 5};
            
            foreach (Node node in activeNodes)
            {

                int num = rnd.Next(level[node.id / 1000]) + 1;
                node.type = num;
                if (node.id / 1000 == maxLayer) node.type = 5;

                BitmapImage bitmap = new BitmapImage();
                
                try
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri($"E:\\cc\\project-cat-balatro\\Res\\Photo\\Node\\{node.type}.png");
                    bitmap.EndInit();
                    node.photo.Source = bitmap;
                    node.photo.Width = 50;
                    node.photo.Height = 50;
                    node.photo.Tag = node;
                }
                catch (Exception ex) {
                    Console.WriteLine($"Error happened!\nLoading photo caused an error: {ex.Message}");
                }
                if (node.type == 1)
                {
                    node.photo.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(map_icon_toenemy));
                }
                else if (node.type == 2)
                {
                    node.photo.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(map_icon_toshop));
                }
                else if (node.type == 3) {
                    node.photo.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(map_icon_tounknown));
                }
                else if (node.type == 4)
                {
                    node.photo.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(map_icon_toelite));
                }
                else if (node.type == 5)
                {
                    node.photo.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(map_icon_toboss));
                }
                Canvas.SetLeft(node.photo, node.Position.X - 20);
                Canvas.SetTop(node.photo, node.Position.Y - 20);
                canvas_map.Children.Add(node.photo);

                node.photo.IsEnabled = false;
            }
        }
        private void map_icon_toenemy(object sender, MouseButtonEventArgs e)
        {
            meow.Play();
            grid_bonus.Visibility = Visibility.Visible;
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;

            grid_Map.Visibility = Visibility.Collapsed;
            grid_PlayRoom.Visibility = Visibility.Visible;
            grid_hand.IsEnabled = true;
            
            currentNode = img.Tag as Node;
            Canvas.SetTop(currentNodePhoto, currentNode.Position.Y - 55);
            Canvas.SetLeft(currentNodePhoto, currentNode.Position.X - 20);
            //ADDING ENEMY
            Random rnd = new Random();
            int id = rnd.Next(0, 3);
            
            grid_enemy.Children.Add(enemies[id].get_photo());
            enemies[id].buff = 0;
            Grid.SetColumn(enemies[id].get_photo(), 0);

            restart_playrrom(100+rnd.Next(3)*40, 100, id);
            label_enemy_hp.Content = enemies[id].get_hp();
        }
        //Margin="33,34,567,0" <- cat shop
        System.Windows.Controls.Image shopphoto = new System.Windows.Controls.Image();
        bool bought_smt = false;

       
        private void map_icon_toshop(object sender, MouseButtonEventArgs e)
        {
            meow.Play();
            Random rnd = new Random();
            canvas.Children.Remove(grid_bonus);
            canvas_shop.Children.Add(grid_bonus);
            Canvas.SetTop(grid_bonus, 10);
            Canvas.SetLeft(grid_bonus, 429);

            grid_bonus.Visibility = Visibility.Visible;
            


            grid_Map.Visibility= Visibility.Collapsed;
            grid_Shop.Visibility = Visibility.Visible;
            label_moneyPlayer1.Content = gold_player;
            label_moneyPlayer.Content = label_moneyPlayer1.Content;
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            currentNode = img.Tag as Node;
            Canvas.SetTop(currentNodePhoto, currentNode.Position.Y - 55);
            Canvas.SetLeft(currentNodePhoto, currentNode.Position.X - 20);
            BitmapImage bitmap1 = new BitmapImage();
            try
            {
                bitmap1.BeginInit();
                bitmap1.CacheOption = BitmapCacheOption.OnLoad;
                int id = rnd.Next(5) + 1;
                bitmap1.UriSource = new Uri($"E:\\cc\\project-cat-balatro\\Res\\Photo\\shop\\{id}.png");
                bitmap1.EndInit();
                shopphoto.Source = bitmap1;
                shopphoto.Tag = id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error happened!\nLoading photo caused an error: {ex.Message}");
            }
        
            grid_Shop.Children.Add(shopphoto);
            shopphoto.Margin = new Thickness(33, 34, 567, 0);
            fulling_shop();
        }
        //forshop[0] = new BonusCard("BonusCards\\love.png", 0, "additional points");
        //forshop[1] = new BonusCard("BonusCards\\kittiboom.jpg", 1, "additional multiply");
        //forshop[2] = new BonusCard("BonusCards\\hahah.jpg", 2, "additional restart");
        //forshop[3] = new BonusCard("BonusCards\\darkbell.jpg", 3, "additional play");
        //forshop[4] = new BonusCard("BonusCards\\sleepykitti.jpg", 4, "decrease hp");
        //forshop[5] = new BonusCard("BonusCards\\titancloaud.jpg", 5, "more gold");
        //forshop[6] = new BonusCard("BonusCards\\umbrellacats.jpg", 6, "additional points and multiply");
        private void fulling_shop()
        {
            Random rnd = new Random();
            bought_smt = false;
            for (int i = 0; i < 3; i++)
            {
                BonusCard bs1 = null;
                int id = rnd.Next(7);
                if(id == 0) { bs1 = new BonusCard("BonusCards\\love.png", 0, "additional points"); }
                else if (id == 1) { bs1 = new BonusCard("BonusCards\\kittiboom.jpg", 1, "additional multiply"); }
                else if (id == 2) { bs1 = new BonusCard("BonusCards\\hahah.jpg", 2, "additional restart"); }
                else if (id == 3) { bs1 = new BonusCard("BonusCards\\darkbell.jpg", 3, "additional play"); }
                else if (id == 4) { bs1 = new BonusCard("BonusCards\\sleepykitti.jpg", 4, "decrease hp"); }
                else if (id == 5) { bs1 = new BonusCard("BonusCards\\titancloaud.jpg", 5, "more gold"); }
                else if (id == 6) { bs1 = new BonusCard("BonusCards\\umbrellacats.jpg", 6, "additional points and multiply"); }


                bs1.cost = rnd.Next(50, 400);
                bs1.status = 1;
                grid_market.Children.Add(bs1.photo);
                bs1.photo.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(press_bonus_card));
                bs1.photo.Tag = bs1;
                Grid.SetColumn(bs1.photo, i);
                Grid.SetRow(bs1.photo, 0);
            }
            for (int i = 0; i < 3; i++)
            {
                BonusCard bs1 = null;
                int id = rnd.Next(7);
                if (id == 0) { bs1 = new BonusCard("BonusCards\\love.png", 0, "additional points"); }
                else if (id == 1) { bs1 = new BonusCard("BonusCards\\kittiboom.jpg", 1, "additional multiply"); }
                else if (id == 2) { bs1 = new BonusCard("BonusCards\\hahah.jpg", 2, "additional restart"); }
                else if (id == 3) { bs1 = new BonusCard("BonusCards\\darkbell.jpg", 3, "additional play"); }
                else if (id == 4) { bs1 = new BonusCard("BonusCards\\sleepykitti.jpg", 4, "decrease hp"); }
                else if (id == 5) { bs1 = new BonusCard("BonusCards\\titancloaud.jpg", 5, "more gold"); }
                else if (id == 6) { bs1 = new BonusCard("BonusCards\\umbrellacats.jpg", 6, "additional points and multiply"); }


                bs1.cost = rnd.Next(50, 400);
                bs1.status = 1;
                grid_market.Children.Add(bs1.photo);
                bs1.photo.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(press_bonus_card));
                bs1.photo.Tag = bs1;
                Grid.SetColumn(bs1.photo, i);
                Grid.SetRow(bs1.photo, 1);
            }
        }
        private void press_bonus_card(object sender,MouseButtonEventArgs e )
        {
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;

            BonusCard bs = img.Tag as BonusCard;
            Grid gr = (Grid)bs.photo.Parent;

            if (bs.status == 1) {
                TextBlock textBox = new TextBlock();
                gr.Children.Add(textBox);
                Grid.SetColumn(textBox, Grid.GetColumn(img));
                Grid.SetRow(textBox, Grid.GetRow(img));
                textBox.Tag = bs;
                textBox.AddHandler(Mouse.MouseLeaveEvent, new MouseEventHandler(press_bonus_card_MOUSELEAVE));
                textBox.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(rect_click));
                textBox.Background = Brushes.White;
                textBox.Text = bs.cost.ToString();
                textBox.Text += "\nbuy?\n";
                textBox.Text += bs.description;

            } else
            {
                TextBlock textBox = new TextBlock();
                gr.Children.Add(textBox);
                Grid.SetColumn(textBox, Grid.GetColumn(img));
                Grid.SetRow(textBox, Grid.GetRow(img));
                textBox.Tag = bs;
                textBox.AddHandler(Mouse.MouseLeaveEvent, new MouseEventHandler(press_bonus_card_MOUSELEAVE));
                textBox.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(rect_click));
                textBox.Background = Brushes.White;
                textBox.Text = "del?\n";
                textBox.Text += bs.description;
            }
        }
        
        public void rect_click(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            BonusCard bs = tb.Tag as BonusCard;
            if (bs.status == 1)
            {
                if (Array.IndexOf(bonuses, null) == -1)
                {
                    tb.Text = "нет места";
                    
                }
                else if (gold_player >= bs.cost)
                {
                    gold_player -= bs.cost;
                    bonuses[Array.IndexOf(bonuses, null)] = bs;
                    deleteFromGrid((Grid)tb.Parent, tb);
                    grid_market.Children.Remove(bs.photo);
                    bs.status = 0;
                    refresh_bonuses();
                    bought_smt = true;
                    label_moneyPlayer1.Content = gold_player;
                    label_moneyPlayer.Content = label_moneyPlayer1.Content;

                    if((int)shopphoto.Tag == 5) {
                        BitmapImage bitmap1 = new BitmapImage();
                        try
                        {
                            bitmap1.BeginInit();
                            bitmap1.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap1.UriSource = new Uri($"E:\\cc\\project-cat-balatro\\Res\\Photo\\shop\\7.png");
                            bitmap1.EndInit();
                            shopphoto.Source = bitmap1;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error happened!\nLoading photo caused an error: {ex.Message}");
                        }
                    }
                    else {
                        BitmapImage bitmap1 = new BitmapImage();
                        try
                        {
                            bitmap1.BeginInit();
                            bitmap1.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap1.UriSource = new Uri($"E:\\cc\\project-cat-balatro\\Res\\Photo\\shop\\6.png");
                            bitmap1.EndInit();
                            shopphoto.Source = bitmap1;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error happened!\nLoading photo caused an error: {ex.Message}");
                        }
                    }
                }
                else
                {
                    tb.Text = "no gold";
                }
            }
            else
            {
                bonuses[Array.IndexOf(bonuses, bs)] = null;
                deleteFromGrid((Grid)tb.Parent, tb);
                grid_bonus.Children.Remove(bs.photo);
                refresh_bonuses();

            }
            label_moneyPlayer1.Content = label_moneyPlayer.Content;
            refresh_bonuses();
        }
        public void deleteFromGrid(Grid grid, TextBlock tb)
        {
            if (grid == null) return;
            grid.Children.Remove(tb);
            tb = null;
        }
        private void press_bonus_card_MOUSELEAVE(object sender, MouseEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            deleteFromGrid((Grid)tb.Parent, tb);
        }

        private void map_icon_tounknown(object sender, MouseButtonEventArgs e)
        {
            meow.Play();
            Random rnd = new Random();
            grid_Map.Visibility = Visibility.Collapsed;
            grid_Unknown.Visibility = Visibility.Visible;
            grid_bonus.Visibility = Visibility.Collapsed;

            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            currentNode = img.Tag as Node;
            Canvas.SetTop(currentNodePhoto, currentNode.Position.Y - 55);
            Canvas.SetLeft(currentNodePhoto, currentNode.Position.X - 20);
            Events ev = new Events(rnd.Next(5));
            label_text_unknown.Content = ev.text;
            label_text_unknown.Tag = ev;

            bt1.Content = ev.bt1;
            bt2.Content = ev.bt2;

        }
        private void map_icon_toelite(object sender, MouseButtonEventArgs e)
        {
            meow.Play();
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            grid_bonus.Visibility = Visibility.Visible;
            grid_Map.Visibility = Visibility.Collapsed;
            grid_PlayRoom.Visibility = Visibility.Visible;
            grid_hand.IsEnabled = true;

            currentNode = img.Tag as Node;
            Canvas.SetTop(currentNodePhoto, currentNode.Position.Y - 55);
            Canvas.SetLeft(currentNodePhoto, currentNode.Position.X - 20);
            //ADDING ENEMY
            Random rnd = new Random();
            int id = rnd.Next(0, 3);

            grid_enemy.Children.Add(enemies[id].get_photo());

            Grid.SetColumn(enemies[id].get_photo(), 0);
            enemies[id].buff = rnd.Next(1,4);

            restart_playrrom(300+rnd.Next(3)*100, 200, id);
            label_enemy_hp.Content = enemies[id].get_hp();

        }
        private void map_icon_toboss(object sender, MouseButtonEventArgs e)
        {
            meow.Play();
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            grid_bonus.Visibility = Visibility.Visible;
            grid_Map.Visibility = Visibility.Collapsed;
            grid_PlayRoom.Visibility = Visibility.Visible;
            grid_hand.IsEnabled = true;
            boosfl = true;
            currentNode = img.Tag as Node;
            Canvas.SetTop(currentNodePhoto, currentNode.Position.Y - 55);
            Canvas.SetLeft(currentNodePhoto, currentNode.Position.X - 20);
            //ADDING ENEMY
            Random rnd = new Random();
            int id = rnd.Next(3,5);

            grid_enemy.Children.Add(enemies[id].get_photo());

            Grid.SetColumn(enemies[id].get_photo(), 0);

            restart_playrrom(1000+0, 400, id);
            label_enemy_hp.Content = enemies[id].get_hp();

        }
        HashSet<Node> getReachable(Node s)
        {
            HashSet<Node> seen = new HashSet<Node>();
            Stack<Node> stack = new Stack<Node>();
            stack.Push(s);
            seen.Add(s);

            while (stack.Count > 0)
            {
                Node u = stack.Pop();
                foreach (Node v in u.neighbors)
                {
                    if (!seen.Contains(v))
                    {
                        seen.Add(v);
                        stack.Push(v);
                    }
                }
            }
            return seen;
        }


        /// <summary>
        /// Упорядочивает узлы внутри каждого слоя для минимазации перекрестков
        /// </summary>
        /// <param name="layers"></param>
        /// <param name="iterators"></param>
        void reorderLayers(List<List<Node>> layers, int iterators = 2)
        {
            int L = layers.Count;
            //проход вперед
            for (int i = 1; i < L; i++)
            {
                sortLayerByBaryCenter(layers, i, layers[i - 1]);
            }
            //проход назад
            for (int i = L - 2; i >= 0; i--)
            {
                sortLayerByBaryCenter(layers, i, layers[i + 1]);
            }
        }


        /// <summary>
        /// Сортирует слой по среднему IndexInLayer соседей из соседнего слоя (bary)
        /// </summary>
        /// <param name="layers"></param>
        /// <param name="layerIndex"></param>
        /// <param name="refLayer"></param>
        void sortLayerByBaryCenter(List<List<Node>> layers, int layerIndex, List<Node> refLayer)
        {
            List<Node> layer = layers[layerIndex];
            // node -> позиция соседа в reflayer
            var indexInRef = refLayer.Select((n, idx) => (n, idx)).ToDictionary(p => p.n, p => (double)p.idx);
            //для каждого узла считаем bary
            var baryList = layer.Select(n => {
                var nbrs = n.neighbors
                                    .Where(nei => indexInRef.ContainsKey(nei))
                                    .Select(nei => indexInRef[nei])
                                    .ToList();
                double bary = nbrs.Any() ? nbrs.Average() : double.PositiveInfinity;
                return (node: n, bary);
            })
                .OrderBy(t => t.bary)
                .Select(t => t.node)
                .ToList();

            layers[layerIndex] = baryList;
        }
        private void oneSec_tick(object sender, EventArgs e)
        {
            grid_Shop.Children.Remove(shopphoto);
            grid_Shop.Visibility = Visibility.Collapsed;
            grid_Map.Visibility = Visibility.Visible;

            DispatcherTimer timer = sender as DispatcherTimer;
            timer.Stop();
        }
        bool flshop = true;
        private void click_arrowShop(object sender, MouseButtonEventArgs e)
        {
            meow.Play();
            
            grid_bonus.Visibility = Visibility.Collapsed;
            canvas_shop.Children.Remove(grid_bonus);
            try
            {
                canvas.Children.Add(grid_bonus);
                nextLayer();

            }
            catch (Exception ex) { }
            if (!bought_smt)
            {
                BitmapImage bitmap1 = new BitmapImage();
                try
                {
                    bitmap1.BeginInit();
                    bitmap1.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap1.UriSource = new Uri($"E:\\cc\\project-cat-balatro\\Res\\Photo\\shop\\8.png");
                    bitmap1.EndInit();
                    shopphoto.Source = bitmap1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error happened!\nLoading photo caused an error: {ex.Message}");
                }
            }


            DispatcherTimer timerOneSec = new DispatcherTimer();
            timerOneSec.Interval = TimeSpan.FromSeconds(1);
            timerOneSec.Tick += oneSec_tick;
            timerOneSec.Start();
        }

        private void resultShowOneSec(object sender, EventArgs e)
        {
            label_result_unknown.Content = "";
            DispatcherTimer timer = sender as DispatcherTimer;
            timer.Stop();
        }
        //event1
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            grid_Unknown.Visibility = Visibility.Collapsed;
            grid_Map.Visibility = Visibility.Visible;

            Events ev = label_text_unknown.Tag as Events;

            label_result_unknown.Content = ev.ans1;
            if (ev.ans1 == "+1 СБРОС +1 РУКА") { basePlayCount++; baseRestartCount++; }
            else if (ev.ans1 == "+1 РУКА") { basePlayCount++; }
            else if (ev.ans1 == "+1 РУКА") { basePlayCount++; }
            else if (ev.ans1 == "+1 СБРОС") { baseRestartCount++; }
            else if (ev.ans1 == "-1 РУКА") { basePlayCount--; }
            
            DispatcherTimer oneSec = new DispatcherTimer();
            oneSec.Interval = TimeSpan.FromSeconds(1);
            oneSec.Tick += resultShowOneSec;
            oneSec.Start();
            nextLayer();
        }
        //event2
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            meow.Play();
            grid_Unknown.Visibility = Visibility.Collapsed;
            grid_Map.Visibility = Visibility.Visible;

            Events ev = label_text_unknown.Tag as Events;

            label_result_unknown.Content = ev.ans2;
            if (ev.ans2 == "-1 СБРОС") { baseRestartCount--; }
            else if (ev.ans2 == "-1 РУКА") { basePlayCount--; }
            else if (ev.ans2 == "-1 РУКА") { basePlayCount--; }
            else if (ev.ans2 == "-1 РУКА") { basePlayCount--; }
            else if (ev.ans2 == "-1 СБРОС") { baseRestartCount--; }

            DispatcherTimer oneSec = new DispatcherTimer();
            oneSec.Interval = TimeSpan.FromSeconds(1);
            oneSec.Tick += resultShowOneSec;
            oneSec.Start();
            nextLayer();
        }



        private void nextLayer()
        {
            var nodes = getActiveNodesAtLayer(activeNodes, currentlayer);

            foreach (var node in nodes)
            {
                
                BitmapImage bitmap = new BitmapImage();
                try
                {
                    System.Windows.Controls.Image photo = new System.Windows.Controls.Image();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri($"E:\\cc\\project-cat-balatro\\Res\\Photo\\Node\\cross.png");
                    bitmap.EndInit();
                    photo.Source = bitmap;
                    photo.Width = 50;
                    photo.Height = 50;
                    Canvas.SetLeft(photo, node.Position.X - 20);
                    Canvas.SetTop(photo, node.Position.Y - 20);
                    canvas_map.Children.Add(photo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error happened!\nLoading photo caused an error: {ex.Message}");
                }
                node.photo.IsEnabled = false;
                node.photo.Opacity = 0.5;
            }

            currentlayer++;
            nodes = getActiveNodesAtLayer(activeNodes, currentlayer);
            foreach (var node in nodes)
            {
                if (currentNode.neighbors.Contains(node))
                {
                    node.photo.IsEnabled = true;
                }
            }
        }

        private void button_exit_Click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            window.Close();
        }

        private void button_start_Click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            grid_chooseKoloda.Visibility = Visibility.Visible;
            grid_MainMenu.Visibility = Visibility.Collapsed;
        }

        private void button_koloda1_Click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            start_game(1);
        }

        private void button_koloda2_Click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            start_game(2);
        }

        private void button_koloda3_Click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            start_game(3);
        }

        private void button_exitintomenu_Click(object sender, RoutedEventArgs e)
        {
            grid_PlayRoom.Visibility = Visibility.Collapsed;
            grid_EndGame.Visibility = Visibility.Collapsed;
            grid_MainMenu.Visibility = Visibility.Visible;
            meow.Play();
            grid_hand.Children.Clear();
            grid_hand.ColumnDefinitions.Clear();
            grid_bonus.Children.Clear();
            grid_bonus.ColumnDefinitions.Clear();
            grid_checkcards.Children.Clear();
            grid_checkcards.ColumnDefinitions.Clear();
            grid_checkcards.RowDefinitions.Clear();
            canvas_map.Children.Clear();
            //Canvas.Left="10" Canvas.Top="356" Canvas.Left="55" Canvas.Top="356"
            plus_gold.Content = "";
            canvas_map.Children.Add(label_money);
            canvas_map.Children.Add(label_moneyPlayer);
            Canvas.SetTop(label_money, 356);
            Canvas.SetLeft(label_money, 10);
            Canvas.SetTop(label_moneyPlayer, 356);
            Canvas.SetLeft(label_moneyPlayer, 55);
            grid_market.Children.Clear();
            grid_market.ColumnDefinitions.Clear();
            grid_market.RowDefinitions.Clear();
            grid_bonus_showamount.Children.Clear();
            grid_bonus_showamount.ColumnDefinitions.Clear();
            canvas_map.Children.Add(button_closemap);
            grid_PlayRoom.Opacity = 1;
            grid_PlayRoom.IsEnabled = true ;

            global_debuff = 0;
            label_debuff.Content = "";
        }

        private void button_leavefromplayroom_Click(object sender, RoutedEventArgs e)
        {
            meow.Play();
            grid_SettingsWhileInPlayRoom.Visibility = Visibility.Collapsed;
            button_exitintomenu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            meow.Play();
            grid_Map.Visibility = Visibility.Collapsed;
            grid_MainMenu.Visibility = Visibility.Visible;

            grid_hand.Children.Clear();
            grid_hand.ColumnDefinitions.Clear();
            grid_bonus.Children.Clear();
            grid_bonus.ColumnDefinitions.Clear();
            grid_checkcards.Children.Clear();
            grid_checkcards.ColumnDefinitions.Clear();
            grid_checkcards.RowDefinitions.Clear();
            canvas_map.Children.Clear();
            //Canvas.Left="10" Canvas.Top="356" Canvas.Left="55" Canvas.Top="356"
            plus_gold.Content = "";
            canvas_map.Children.Add(label_money);
            canvas_map.Children.Add(label_moneyPlayer);
            Canvas.SetTop(label_money, 356);
            Canvas.SetLeft(label_money, 10);
            Canvas.SetTop(label_moneyPlayer, 356);
            Canvas.SetLeft(label_moneyPlayer, 55);
            grid_market.Children.Clear();
            grid_market.ColumnDefinitions.Clear();
            grid_market.RowDefinitions.Clear();
            grid_bonus_showamount.Children.Clear();
            grid_bonus_showamount.ColumnDefinitions.Clear();

            canvas_map.Children.Add(button_closemap);
            grid_PlayRoom.Opacity = 1;
            grid_PlayRoom.IsEnabled = true;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            meow.Play();
            grid_MainMenu.Visibility = Visibility.Visible;
            grid_chooseKoloda.Visibility = Visibility.Collapsed;
        }
    }
}
