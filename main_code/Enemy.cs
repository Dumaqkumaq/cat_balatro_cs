using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace project_cat_balatro
{
    class Enemy
    {
        Image photo = new Image();
        int Hp = 0;
        public int buff {  get; set; }
        public Enemy(string path, int hp)
        {
            BitmapImage bitmap = new BitmapImage();
            buff = 0;
            Hp = hp;
            try
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri($"E:\\cc\\project-cat-balatro\\Res\\Photo\\{path}");
                bitmap.EndInit();
                photo.Source = bitmap;
                photo.IsEnabled = true;

            }
            catch (Exception ex) { }
        }
        public Enemy() { photo = null; }

        public int get_hp() { return Hp; }
        public void set_hp(int hp) { Hp = hp; }
        public Image get_photo() {  return photo; }
        public void set_photo(Image photo) { this.photo = photo; }
    }
}
