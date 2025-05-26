// project-cat-balatro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// project_cat_balatro.Card
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using project_cat_balatro;

internal class Card : Common_for_all_cards
{
	private int amount;
    public int baseAmount = 0;
	private int type;

	private Card[] choosen_cards;
    DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

    private Label show_combo;
	private Label lb_count_point;
	private Label lb_count;

    public Card(string path, int amount, int type)
		: base(path)
	{
		this.amount = amount;
		this.type = type;
        baseAmount = amount;
        timer.Tick += this.tick;

        photo.AddHandler(Mouse.MouseEnterEvent, new MouseEventHandler(mouse_enter));
        photo.AddHandler(Mouse.MouseLeaveEvent, new MouseEventHandler(mouse_leave));
        photo.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(button_left_click));
	}
    private void mouse_enter(object sender, MouseEventArgs e)
    {
        timer.Start();
    }
    private void mouse_leave(object sender, MouseEventArgs e)
    {
        timer.Stop();
    }
    //
    // МЕТОДЫ ДЛЯ ПОДСКАЗКИ
    //
    //
    private void mouseleave(object sender, MouseEventArgs e)
    {
        TextBlock tb = sender as TextBlock;
        deleteFromGrid((Grid)tb.Parent, tb);
    }
    //
    // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
    //
    //
    private void tick(object sender, EventArgs e)
    {
        timer.Stop();
        spawnOnGrid((Grid)photo.Parent);
    }
    public void spawnOnGrid(Grid grid)
    {
        TextBlock textBox = new TextBlock();
        textBox.Background = Brushes.White;
        textBox.Text = this.ToString();

        textBox.Width = this.photo.Width;
        textBox.Height = this.photo.Height;

        textBox.AddHandler(Mouse.MouseLeaveEvent, new MouseEventHandler(mouseleave));
        textBox.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(rect_click));
        grid.Children.Add(textBox);
        Grid.SetColumn(textBox, Grid.GetColumn(this.photo));

    }
    public void rect_click(object sender, MouseButtonEventArgs e)
    {
        TextBlock tb = sender as TextBlock;
        deleteFromGrid((Grid)tb.Parent, tb);
    }
    public void deleteFromGrid(Grid grid, TextBlock tb)
    {
        if (grid == null) return;
        grid.Children.Remove(tb);
        tb = null;
    }
    public Card(string path, int amount, int type, Card[] cards, Label show_combo, Label lb_count_point, Label lb_count)
		: base(path)
	{
		this.amount = amount;
		this.type = type;
		choosen_cards = cards;
		this.show_combo = show_combo;
		this.lb_count_point = lb_count_point;
        baseAmount = amount;
        this.lb_count = lb_count;
		photo.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(button_left_click));
	}

	public Card()
	{
	}

	private void button_left_click(object sender, MouseButtonEventArgs e)
	{
		if (choose)
		{
			photo.Width = 70.0;
			photo.Height = 120.0;
			choose = false;
			choosen_cards[Array.IndexOf(choosen_cards, this)] = null;
		}
		else if(Array.IndexOf(choosen_cards, null) != -1)
		{
			photo.Width = 100.0;
			photo.Height = 150.0;
			choose = true;
			choosen_cards[Array.IndexOf(choosen_cards, null)] = this;
		}

		int[] amount_combo = new int[9];
		int[] type_combo = new int[4];
		foreach (Card card in choosen_cards)
		{
			if (card != null)
			{
				amount_combo[card.getAmount()]++;
				type_combo[card.getType()]++;
			}
		}
        if (amount_combo.Contains(3) && amount_combo.Contains(2))
        {
            show_combo.Content = "Пара и трио";
			lb_count.Content = "40";
            lb_count_point.Content = "*5";
        }
        else if (amount_combo.Contains(3))
        {
            show_combo.Content = "Трио";
            lb_count.Content = "30";
            lb_count_point.Content = "*3";
        }
        else if (amount_combo.Count((int s) => s == 2) == 2)
        {
            show_combo.Content = "Две пары";
            lb_count.Content = "25";
            lb_count_point.Content = "*2";
        }
        else if (amount_combo.Contains(2))
        {
            show_combo.Content = "Пара";
            lb_count.Content = "20";
            lb_count_point.Content = "*2";
        }
        else if (amount_combo.Contains(1))
        {
            show_combo.Content = "Одиночка";
            lb_count.Content = "10";
            lb_count_point.Content = "*1";
        }
        else
        {
            lb_count.Content = "";
            show_combo.Content = "";
            lb_count_point.Content = "";
        }
		//show_combo.Content = "";
		//foreach (int i in amount_combo)
		//{
		//	show_combo.Content += i.ToString() + ".";
		//}
	}
	public void change_state()
	{
        if (choose)
        {
            photo.Width = 70.0;
            photo.Height = 120.0;
            choose = false;
            choosen_cards[Array.IndexOf(choosen_cards, this)] = null;
        }
        else
        {
            photo.Width = 100.0;
            photo.Height = 150.0;
            choose = true;
            choosen_cards[Array.IndexOf(choosen_cards, null)] = this;
        }
        int[] amount_combo = new int[9];
        int[] type_combo = new int[4];
        foreach (Card card in choosen_cards)
        {
            if (card != null)
            {
                amount_combo[card.getAmount()]++;
                type_combo[card.getType()]++;
            }
        }
        if (amount_combo.Contains(3) && amount_combo.Contains(2))
        {
            show_combo.Content = "Пара и трио";
			lb_count.Content = "40";
            lb_count_point.Content = "*5";
        }
        else if (amount_combo.Contains(3))
        {
            show_combo.Content = "Трио";
            lb_count.Content = "30";
            lb_count_point.Content = "*3";
        }
        else if (amount_combo.Count((int s) => s == 2) == 2)
        {
            show_combo.Content = "Две пары";
            lb_count.Content = "25";
            lb_count_point.Content = "*2";
        }
        else if (amount_combo.Contains(2))
        {
            show_combo.Content = "Пара";
            lb_count.Content = "20";
            lb_count_point.Content = "*2";
        }
        else if (amount_combo.Contains(1))
        {
            show_combo.Content = "Одиночка";
            lb_count.Content = "10";
            lb_count_point.Content = "+10";
        }
        else
        {
            lb_count.Content = "";
            show_combo.Content = "";
            lb_count_point.Content = "";
        }
    }
	public void setAmount(int num)
	{
		amount = num;
	}

	public int getAmount()
	{
		return amount;
	}

	public void setType(int num)
	{
		type = num;
	}

	public int getType()
	{
		return type;
	}

	public override void print()
	{
		Console.WriteLine($"Card :: Photo: {getphoto() != null}, Amount: {amount}, Type: {type}");
	}

	public override string ToString()
	{
		return $"Photo: {getphoto() != null}\n Amount: {amount + 1}\n Type: {type + 1}";
	}
}
