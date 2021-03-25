
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
                Inlogscherm(gebruikers.Klanten);
            }
        }

        public static bool CheckWachtwoord(int index, Inloggen[] klant) //Checkt of het wachtwoord en de gebruikersnaam bij elkaar horen
        {
            string GegevenWachtwoord = Console.ReadLine();
            return GegevenWachtwoord == klant[index].Wachtwoord;
        }
        public static void Inlogscherm(Inloggen[] klant)
        {

            bool NaamBestaat = false;
            Console.Clear();
            Console.WriteLine($"Voer uw gebruikersnaam in.");
            string GegevenNaam = Console.ReadLine();
            int index = 0;
            for (int i = 0; i < klant.Length && !NaamBestaat; i++) //checkt of de gebruikersnaam bestaat
            {
                if (GegevenNaam == klant[i].Gebruikersnaam) { NaamBestaat = true; index = i; }
            }
            if (NaamBestaat)
            {
                Console.WriteLine($"\x0AVoer uw wachtwoord in.");
                while (!CheckWachtwoord(index, klant)) //blijft om het wachtwoord vragen totdat het juiste wachtwoord voor de gebruikersnaam wordt gegeven
                {
                    Console.WriteLine("Verkeerd wachtwoord. Probeer het opnieuw.");
                }
                Console.WriteLine($"\x0AU bent ingelogd! Druk op Enter om verder te gaan.");
                Console.ReadLine();
            }
            else //reset het inlogscherm wanneer een nog niet geregistreerde gebruikersnaam wordt gegeven
            { 
                Console.WriteLine("Verkeerde gebruikersnaam. Druk op Enter om het opnieuw te proberen.");
                Console.ReadLine();
                Inlogscherm(klant);
            }
        }
    }
}