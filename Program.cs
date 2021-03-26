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

            MenuGerechten(menu.Voorgerechten, "Voorgerechten");
        }
        public static void MenuGerechten(Gerechten[] typeGerecht, string typeGerechtNaam)
        {
            string userInput = null;
            int userInputConverted = 0;
            Console.Clear();
            Console.WriteLine($"Dit zijn de {typeGerechtNaam} van The Fat Duck.\x0A\x0A");
            for (int i = 1; i < typeGerecht.Length + 1; i++)
            {

                Console.WriteLine(typeGerecht[i - 1].naam);
                Console.WriteLine($"Toets {i} voor meer informatie over dit gerecht \x0A\x0A");
            }
            Console.WriteLine($"Toets Q om terug te gaan \x0A\x0A");

            bool passed = false;
            while (!passed) // checkt of de user input wel op het menu staat of Q is, anders vraagt het om een nieuwe input.
            {
                try
                {
                    userInput = Console.ReadLine();
                    userInputConverted = Int32.Parse(userInput);
                }
                catch (System.FormatException)
                {
                    if (userInput == "Q")
                    {
                        passed = true;
                        // call het vorige scherm functie /
                    }
                }

                if (userInputConverted > typeGerecht.Length || userInputConverted <= 0)
                {
                    Console.WriteLine("Dit gerecht bestaat niet! Probeer een ander gerecht");
                }
                else
                {
                    passed = true;
                    showItem(userInputConverted, typeGerecht);
                }
            }
        }
        public static void showItem(int x, Gerechten[] typeGerecht)
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
        }
        /*  public static bool menuGerechtenError()
          {
              bool passed = false;
              while (!passed)
              {

              }
              return true;
          } */
    }
}
