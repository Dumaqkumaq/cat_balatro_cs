using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace project_cat_balatro
{
    class Common_for_all_cards
    {
        public System.Windows.Controls.Image photo = new System.Windows.Controls.Image();
        public bool choose = false;
        //
        // КОНСТРУКТОРЫ
        //
        //
        public Common_for_all_cards(string path_to_photo) {
            BitmapImage bitmap = new BitmapImage();
            try
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri($"E:\\cc\\project-cat-balatro\\Res\\Photo\\{path_to_photo}");
                bitmap.EndInit();
                photo.Source = bitmap;
                photo.IsEnabled = true;
                photo.Width = 70;
                photo.Height = 120;


                
            } catch(Exception ex)
            {
                Console.WriteLine($"Error happened!\nLoading photo caused an error: {ex.Message}");
            }
        }
        public Common_for_all_cards()
        {
            photo = null;
        }
        //
        // МЕТОДЫ ИЗОБРАЖЕНИЯ
        //
        //
        
       
        public void normphoto()
        {
            photo.Width = 70;
            photo.Height = 120;
        }
        
        virtual public void print()
        {
            System.Console.WriteLine($"\nPhoto: {!(photo == null)}");
        }
        //
        // ГЕТТЕРЫ И СЕТТЕРЫ
        //
        //
        public Image getphoto()
        {
            return photo;
        }
        public void setphoto(Image photo)
        {
            this.photo = photo;
        }
    }
}
