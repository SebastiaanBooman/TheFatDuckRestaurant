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



    public class Menucode
    {
        static void Main(string[] args)
        {
            var jsonString = File.ReadAllText("menu.json");
            Menu menu = JsonSerializer.Deserialize<Menu>(jsonString);
            KiesMenu(menu);
        }
        public static void KiesMenu(Menu menu)
        {
            bool verkeerdeInput = false;
            bool passed = false;

            while (!passed)
            {
                Console.Clear();
                Console.WriteLine("Voorgerechten\x0A Klik op A om de voorgerechten in te zien\x0A\x0A\x0A");
                Console.WriteLine("Hoofdgerechten\x0A Klik op B om de hoofdgerechten in te zien\x0A\x0A\x0A");
                Console.WriteLine("Nagerechten\x0A Klik op C om de nagerechten in te zien\x0A\x0A");
                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer A, B, C of Q");
                }
                var toetsUser = Console.ReadLine();
                if (toetsUser == "A" || toetsUser == "a")
                {
                    passed = true;
                    MenuGerechten(menu.Voorgerechten, "Voorgerechten", menu);
                }
                else if (toetsUser == "B" || toetsUser == "b")
                {
                    passed = true;
                    MenuGerechten(menu.Hoofdgerechten, "Hoofdgerechten", menu);
                }
                else if (toetsUser == "C" || toetsUser == "c")
                {
                    passed = true;
                    MenuGerechten(menu.Nagerechten, "Nagerechten", menu);

                }
                else
                {
                    verkeerdeInput = true;
                }
            }
        }
        public static void MenuGerechten(Gerechten[] typeGerecht, string typeGerechtNaam, Menu menu)
        {
            string userInput = null;
            int userInputConverted = 0;
            bool passed = false;
            bool verkeerdeInput = false;
            while (!passed) // checkt of de user input wel op het menu staat of Q is, anders vraagt het om een nieuwe input.
            {
                Console.Clear();
                Console.WriteLine($"Dit zijn de {typeGerechtNaam} van The Fat Duck.\x0A\x0A");
                for (int i = 1; i < typeGerecht.Length + 1; i++)
                {

                    Console.WriteLine(typeGerecht[i - 1].naam);
                    Console.WriteLine($"Toets {i} voor meer informatie over dit gerecht \x0A\x0A");
                }
                Console.WriteLine($"Toets Q om terug te gaan \x0A\x0A");
                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer Q");
                }
                try
                {
                    userInput = Console.ReadLine();
                    userInputConverted = Int32.Parse(userInput);
                }
                catch (System.FormatException)
                {
                    if (userInput != "Q")
                    {
                        verkeerdeInput = true;
                    }
                    else
                    {
                        passed = true;
                        KiesMenu(menu);
                    }
                }

                if (userInputConverted > typeGerecht.Length || userInputConverted <= 0)
                {
                    Console.WriteLine("Dit gerecht bestaat niet! Probeer een ander gerecht");
                }
                else
                {
                    passed = true;
                    showItem(userInputConverted, typeGerecht, typeGerechtNaam, menu);
                }
            }
        }
        public static void showItem(int x, Gerechten[] typeGerecht, string typeGerechtNaam, Menu menu)
        {
            bool passed = false;
            bool verkeerdeInput = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine($"Gerecht: " + typeGerecht[x - 1].naam + "\x0A\x0A");
                Console.WriteLine($"Prijs: " + typeGerecht[x - 1].prijs + "\x0a");
                Console.WriteLine($"Beschrijving: " + typeGerecht[x - 1].beschrijving + "\x0a");
                Console.WriteLine($"Ingredienten: ");
                for (int i = 0; i < typeGerecht[x - 1].ingredienten.Length; i++)
                {
                    Console.WriteLine(typeGerecht[x - 1].ingredienten[i]);
                }
                Console.WriteLine($"\x0a\x0aToets Q om terug te gaan");
                if (verkeerdeInput)
                {
                    Console.WriteLine("Vekeerde input, probeer Q");
                    verkeerdeInput = false;
                }
                string userInput = Console.ReadLine();
                if (userInput == "Q")
                {
                    passed = true;
                    MenuGerechten(typeGerecht, typeGerechtNaam, menu);
                }
                else
                {
                    verkeerdeInput = true;
                }
            }
        }

        public static void addItemMenu(Menu menu)
        {
            var newGerecht = new Gerechten { 
                naam = "Kaas",
                prijs = 10,
                beschrijving = "Cheese",
                ingredienten = new[] {"zuivel", "Kase" }
            };
         //   menu.Voorgerechten = null;

            Menu newMenu = new Menu
            {
                Voorgerechten = new[] {menu.Voorgerechten[0] }, //Hier moet de code weten hoeveel items moeten worden toegevoegd aan de juiste array. +1 voor de array waar het nieuwe item in komt.
                Hoofdgerechten = new[] {menu.Hoofdgerechten[0},
                Nagerechten = new [] {menu.Nagerechten[0}
            };



        }
    }
}
