using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace TheFatDuckRestaurant
{

    public class Menu
    {
        public Gerechten[] Voorgerechten { get; set; }
        public Gerechten[] Hoofdgerechten { get; set; }
        public Gerechten[] Nagerechten { get; set; }
    }

    public class Gerechten
    {
        public string naam { get; set; }
        public double prijs { get; set; }
        public string beschrijving { get; set; }
        public string[] ingredienten { get; set; }
    }



    public class Program
    {
        static void Main(string[] args)
        {
            MenuGerechten("Voorgerechten");
        }
        public static void MenuGerechten(string typeGerechten)
        {

            var jsonString = File.ReadAllText("menu.json");
            Menu menu = JsonSerializer.Deserialize<Menu>(jsonString);

            Console.Clear();
            Console.WriteLine($"Dit zijn de {typeGerechten} van The Fat Duck.\x0A\x0A");
            if(typeGerechten == "Voorgerechten")
            {
                for (int i = 1; i < menu.Voorgerechten.Length + 1; i++)
                {

                    Console.WriteLine(menu.Voorgerechten[i - 1].naam);
                    Console.WriteLine($"Toets {i} voor meer informatie over dit gerecht \x0A\x0A");

                }
                Console.WriteLine($"Toets Q om terug te gaan \x0A\x0A");
                string a = Console.ReadLine();
                int result = Int32.Parse(a);
                showItem(result, typeGerechten);
            }
            else if(typeGerechten == "Hoofdgerechten")
            {
                for (int i = 1; i < menu.Hoofdgerechten.Length + 1; i++)
                {

                    Console.WriteLine(menu.Hoofdgerechten[i - 1].naam);
                    Console.WriteLine($"Toets {i} voor meer informatie over dit gerecht \x0A\x0A");

                }
                Console.WriteLine($"Toets Q om terug te gaan \x0A\x0A");
                string a = Console.ReadLine();
                int result = Int32.Parse(a);
                showItem(result, typeGerechten);
            }
            else
            {
                for (int i = 1; i < menu.Nagerechten.Length + 1; i++)
                {

                    Console.WriteLine(menu.Nagerechten[i - 1].naam);
                    Console.WriteLine($"Toets {i} voor meer informatie over dit gerecht \x0A\x0A");

                }
                Console.WriteLine($"Toets Q om terug te gaan \x0A\x0A");
                string a = Console.ReadLine();
                int result = Int32.Parse(a);
                showItem(result, typeGerechten);
            }
        }
        public static void showItem(int x, string y)
        {
            var jsonString = File.ReadAllText("menu.json");
            Menu menu = JsonSerializer.Deserialize<Menu>(jsonString);
        
            Console.WriteLine(menu.Voorgerechten[x-1].prijs);
            Console.ReadLine();
        }
    }
}
