﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_cat_balatro
{
    class Events
    {
        public int id { get; set; }
        public string text { get; set; }
        public string bt1 { get; set; }
        public string bt2 { get; set; }
        public string ans1 {  get; set; }
        public string ans2 { get; set; }

        public Events(int id)
        {
            this.id = id;

            if (id == 0)
            {
                text = "Кот Василий отправился автостопом вокруг света. Через месяц вернулся домой грязный, усталый, но счастливый.\nГоворит, теперь понял смысл жизни — путешествовать!\nТы тоже хочешь отправиться в путешествие?";
                bt1 = "Da";
                bt2 = "Net";

                ans1 = "+1 СБРОС\n+1 РУКА";
                ans2 = "-1 СБРОС";
            }
            else if (id == 1) {
                text = "Три кота решили исследовать Арктику. Неделю бродили среди льдин, потом вернулись домой,\n решив, что холодильник удобнее.\n Может, вам подарить карту мира?";
                bt1 = "Da";
                bt2 = "Net";

                ans1 = "+1 РУКА";
                ans2 = "-1 РУКА";
            }
            else if (id == 2)
            {
                text = "Маша взяла своего кота Маруса в кругосветное плавание.\n После первой морской болезни решила больше брать питомца только на пикники.\nЕсли опять попросишься в круиз, останешься дома навсегда!";
                bt1 = "Согласен";
                bt2 = "Не согласен";

                ans1 = "+1 РУКА";
                ans2 = "-1 РУКА";
            }
            else if (id == 3)
            {
                text = "Томас побывал во всех странах Европы, каждый раз возвращаясь с сувенирами — игрушечными мышками разных форматов.\n Теперь мечтает попасть в Африку.\nПоехали вместе в Африку?";
                bt1 = "Da";
                bt2 = "Net";

                ans1 = "+1 СБРОС";
                ans2 = "-1 СБРОС";
            }
            else if (id == 4)
            {
                text = "Барсик объездил всю Россию от Калининграда до Владивостока,\n  делая фото на фоне достопримечательностей.Все восхищались его храбростью, кроме хозяйки, которой пришлось оплачивать билеты.\nХочешь дальше путешествовать бесплатно?";
                bt1 = "Da";
                bt2 = "Net";

                ans1 = "-1 РУКА";
                ans2 = "-1 СБРОС";
            }
        }
    }
}
