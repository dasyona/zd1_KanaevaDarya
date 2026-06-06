using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP
{
    internal class Catt
    {

        private string name;
        private double weight;

        public string Name
        {

            get
            {
                return name;
            }

            set
            {
                bool OnlyLetters = true;

                foreach (var ch in value)
                {
                    if (!char.IsLetter(ch))
                    {
                        OnlyLetters = false;
                    }
                }

                if (OnlyLetters)
                {
                    name = value;
                }
                else
                {
                    Console.WriteLine($"{value} - неправильное имя!!!");
                    name = "Неизвестный кот";
                }
            }
        }


        //ves

        public double Weight
        {
            get { return weight; }
            set { if (value > 0)
                {
                    weight = value;
                }
                else
                {
                    Console.WriteLine($"{weight} - не может быть =0 или отрицательным. По умолчанию 1.0 кг");
                    weight = 1.0;
                }
            }
        }

        public Catt(string CatName, double CatWeight)
        {
            Name = CatName;
            Weight = CatWeight;
        }

        public void Meow()
        {
            Console.WriteLine($"{name}: МЯууууу~");

        }

        public void ShowInfo()
        {
            Console.WriteLine($"Кот: {name}, Вес: {weight} кг");
        }


        //public void SetCatName(string CatName)
        //{
        //    bool OnlyLetters = true;

        //    foreach (var ch in CatName)
        //    {
        //        if (!char.IsLetter(ch))
        //        {
        //            OnlyLetters = false;
        //        }
        //    }

        //    if (OnlyLetters)
        //    {  name = CatName;}

        //    else
        //    { }
        //        Console.WriteLine($"{CatName} - неправильное имя!");
        //}
    }
}
