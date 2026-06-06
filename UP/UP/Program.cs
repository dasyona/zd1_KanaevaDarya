using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP
{
    internal class Program
    {
        static void Main(string[] args)
        {
          
            Console.Write("Введите количество котов: ");
            int catCount;

            while (!int.TryParse(Console.ReadLine(), out catCount) || catCount <= 0)
            {
                Console.Write("Пожалуйста, введите положительное целое число: ");
            }

         
            List<Catt> cats = new List<Catt>();

       
            for (int i = 1; i <= catCount; i++)
            {
                Console.WriteLine($"\n-Ввод данных для кота {i} -");

           
                Console.Write("Введите имя кота (только буквы): ");
                string catName = Console.ReadLine();

              
                Console.Write("Введите вес кота (положительное число): ");
                double catWeight;
                while (!double.TryParse(Console.ReadLine(), out catWeight))
                {
                    Console.Write("Пожалуйста, введите корректное число для веса: ");
                }

              
                Catt newCat = new Catt(catName, catWeight);
                cats.Add(newCat);
            }

        
            Console.WriteLine("\n= Информация о всех котах =");
            foreach (var cat in cats)
            {
                cat.ShowInfo();
            }

        
            Console.WriteLine("\n=Коты мяукают =");
            foreach (var cat in cats)
            {
                cat.Meow();
            }

    
            if (cats.Count > 0)
            {
                
                Console.WriteLine("\n= Пример изменения данных кота");
                Console.WriteLine("\n= Введите номер кота которому хотите изменить значение");
                int num = Convert.ToInt32(Console.ReadLine());
           


                Console.Write("Введите новое имя для первого кота: ");
                string newName = Console.ReadLine();
                cats[num-1].Name = newName;

                Console.Write("Введите новый вес для первого кота: ");
                double newWeight;
                while (!double.TryParse(Console.ReadLine(), out newWeight))
                {
                    Console.Write("Пожалуйста, введите корректное число для веса: ");
                }
                cats[num-1].Weight = newWeight;

                Console.WriteLine("\nОбновленная информация о первом коте:");
                cats[num - 1].ShowInfo();
                cats[num - 1].Meow();
            }

            Console.ReadKey();
        }
    }

}

    
   


    