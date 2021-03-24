
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
            while (true)
            {
                Inlogscherm(gebruikers.Klanten, false);
            }
        }

        public static bool CheckWachtwoord(int index, Inloggen[] klant)
        {
            string GegevenWachtwoord = Console.ReadLine();
            return GegevenWachtwoord == klant[index].Wachtwoord;
        }
        public static void Inlogscherm(Inloggen[] klant, bool Recursive)
        {

            bool NaamBestaat = Recursive;
            Console.Clear();
            Console.WriteLine($"Voer uw gebruikersnaam in.");
            string GegevenNaam = Console.ReadLine();
            int index = 0;
            for (int i = 0; i < klant.Length && !NaamBestaat; i++) //checkt of de gebruikersnaam al bestaat
            {
                if (GegevenNaam == klant[i].Gebruikersnaam) { NaamBestaat = true; index = i; }
            }
            if (NaamBestaat || Recursive)
            {
                Console.WriteLine($"\x0AVoer uw wachtwoord in.");
                while (!CheckWachtwoord(index, klant))
                {
                    Console.WriteLine("Verkeerd wachtwoord. Probeer het opnieuw.");
                }
                Console.WriteLine($"\x0AU bent ingelogd! Druk op Enter om verder te gaan.");
                Console.ReadLine();
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