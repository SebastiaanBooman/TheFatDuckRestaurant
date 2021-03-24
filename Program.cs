
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace TheFatDuckRestaurant
{

    public class Gebruikers
    {
        public Inloggen[] Klanten { get; set; }
        //public Inloggen[] Medewerkers { get; set; }
    }

    public class Inloggen
    {
        public string Gebruikersnaam { get; set; }
        public string Wachtwoord { get; set; }
    }



    public class Program
    {
        static void Main(string[] args)
        {
            var jsonString = File.ReadAllText("gebruikers.json");
            Gebruikers gebruikers = JsonSerializer.Deserialize<Gebruikers>(jsonString);
            Inlogscherm(gebruikers.Klanten, false);
        }
        public static void Inlogscherm(Inloggen[] klant, bool Recursive)
        {

            if (!Recursive)
            {
                Console.Clear();
                Console.WriteLine($"Voer uw gebruikersnaam in.");
            }
            bool NaamBestaat = Recursive;
            string GegevenNaam = Console.ReadLine();
            for (int i = 0; i < klant.Length && !NaamBestaat; i++)
            {
                if (GegevenNaam == klant[i].Gebruikersnaam) { NaamBestaat = true; }
            }
            if (NaamBestaat)
            {
                Console.WriteLine($"\x0AVoer uw wachtwoord in.");
                bool WachtwoordBestaat = false;
                string GegevenWachtwoord = Console.ReadLine();
                for (int i = 0; i < klant.Length; i++)
                {
                    if (GegevenNaam == klant[i].Wachtwoord) { WachtwoordBestaat = true; }
                }
                if (WachtwoordBestaat)
                {
                    Console.WriteLine($"\x0AU bent ingelogd!");
                }
                else 
                { 
                    Console.WriteLine("Verkeerd wachtwoord. Druk op Enter om het opnieuw te proberen.");
                    Console.ReadLine();
                    Inlogscherm(klant, true);
                }
            }
            else 
            { 
                Console.WriteLine("Verkeerde gebruikersnaam. Druk op Enter om het opnieuw te proberen.");
                Console.ReadLine();
                Inlogscherm(klant, false);
            }
        }
    }
}

            /*Console.WriteLine($"Toets Q om terug te gaan \x0A\x0A");
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
*/