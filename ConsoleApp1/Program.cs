using ConsoleApp1.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int unos;

            do
            {
                Console.WriteLine("1. Dodaj studenta");
                Console.WriteLine("2. Prikazi studente");
                Console.WriteLine("3. EXIT");

                //try
                //{
                    unos = int.Parse(Console.ReadLine());

                //}
                //catch (Exception)
                //{
                //    Console.WriteLine("Odaberite ponudjeni broj opcije!")
                //}

                switch (unos)
                {
                    case 1:
                        StudentUI.DodajStudenta();
                        break;
                    case 2:
                        StudentUI.PrikaziStudente();
                        break;
                }

            } while (unos != 3);
        }
    }
}
