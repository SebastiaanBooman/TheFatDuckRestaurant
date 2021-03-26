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
            var jsonString = File.ReadAllText("menu.json");
            Menu menu = JsonSerializer.Deserialize<Menu>(jsonString);
            KiesMenu(menu);
        }

        //Deze functie zorgt ervoor dat het juiste menu wordt geladen. 
        //Er moet nog wel functionaliteit komen voor als de input niet A, B of C is.
        public static void KiesMenu(Menu menu)
        {
            Console.WriteLine("Voorgerechten\x0A Klik op A om de voorgerechten in te zien\x0A\x0A\x0A");
            Console.WriteLine("Hoofdgerechten\x0A Klik op B om de hoofdgerechten in te zien\x0A\x0A\x0A");
            Console.WriteLine("Nagerechten\x0A Klik op C om de nagerechten in te zien\x0A\x0A");
            var toetsUser = Console.ReadLine();
            if (toetsUser == "A")
                MenuGerechten(menu.Voorgerechten, "Voorgerechten");

            if (toetsUser == "B")
                MenuGerechten(menu.Hoofdgerechten, "Hoofdgerechten");

            if (toetsUser == "C")
                MenuGerechten(menu.Nagerechten, "Nagerechten");

        }
        public static void MenuGerechten(Gerechten[] typeGerecht, string typeGerechtNaam)
        {

            Console.Clear();
            Console.WriteLine($"Dit zijn de {typeGerechtNaam} van The Fat Duck.\x0A\x0A");
            for (int i = 1; i < typeGerecht.Length + 1; i++)
             {

                 Console.WriteLine(typeGerecht[i - 1].naam);
                 Console.WriteLine($"Toets {i} voor meer informatie over dit gerecht \x0A\x0A");
             }
            Console.WriteLine($"Toets Q om terug te gaan \x0A\x0A");
            var a = Console.ReadLine();
            int b = Int32.Parse(a);
            bool passed = false;
            while (!passed) // checkt of de user input wel op het menu staat of Q is, anders vraagt het om een nieuwe input.
                {
                    if (b > typeGerecht.Length || b <= 0 || a != "Q") //TODO: System crashes als de input een string als "Q" is omdat hij een cijver verwacht bij line 48.
                        {
                        Console.WriteLine("Dit gerecht bestaat niet! Probeer een ander gerecht");
                        a = Console.ReadLine();
                        b = Int32.Parse(a);
                }
                    else
                        {
                    passed = true;
                        }
            }
            showItem(b, typeGerecht);
        }
        public static void showItem(int x, Gerechten[] typeGerecht)
        {
            Console.Clear();
            Console.WriteLine($"Gerecht: " + typeGerecht[x - 1].naam + "\x0A\x0A");
            Console.WriteLine($"Prijs: " + typeGerecht[x-1].prijs + "\x0a");
            Console.WriteLine($"Beschrijving: " + typeGerecht[x - 1].beschrijving + "\x0a");
            Console.WriteLine($"Ingredienten: ");
            for (int i = 0; i < typeGerecht[x-1].ingredienten.Length; i++)
            {
                Console.WriteLine(typeGerecht[x - 1].ingredienten[i]);
            }

            Console.WriteLine($"\x0a\x0aToets Q om terug te gaan");
        }
    }
}
