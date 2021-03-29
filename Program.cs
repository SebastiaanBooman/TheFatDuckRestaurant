
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
        public Inloggen[] Medewerkers { get; set; }
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
                Console.Clear();
                Console.WriteLine("Wilt u als klant ('a') of als medewerker ('b') inloggen?");
                string Choice = Console.ReadLine();
                if(Choice == "a")
                {
                    Inlogscherm(gebruikers.Klanten);
                }
                else if(Choice == "b")
                {
                    Inlogscherm(gebruikers.Medewerkers);
                }
            }
        }
        

        public static void Inlogscherm(Inloggen[] gebruiker)
        {
            Func<int, Inloggen[], Tuple<bool, string>> CheckWachtwoord = (index, gebruiker) => { string Input = Console.ReadLine(); return Tuple.Create(Input == gebruiker[index].Wachtwoord || Input == "terug", Input); };
            bool NaamBestaat = false;
            Console.Clear();
            Console.WriteLine($"Voer uw gebruikersnaam in.");
            string GegevenNaam = Console.ReadLine();
            int index = 0;
            for (int i = 0; i < gebruiker.Length && !NaamBestaat; i++) //checkt of de gebruikersnaam bestaat
            {
                if (GegevenNaam == gebruiker[i].Gebruikersnaam) { NaamBestaat = true; index = i; }
            }
            if (NaamBestaat)
            {
                Console.WriteLine($"\x0AVoer uw wachtwoord in.");
                Tuple<bool, string> Password = CheckWachtwoord(index, gebruiker);
                while (!Password.Item1) //blijft om het wachtwoord vragen totdat het juiste wachtwoord voor de gebruikersnaam wordt gegeven
                {
                    Console.WriteLine("Verkeerd wachtwoord. Probeer het opnieuw of type 'terug' om terug te gaan.");
                    Password = CheckWachtwoord(index, gebruiker);
                }
                if (Password.Item2 != "terug")
                {
                    Console.WriteLine($"\x0AU bent ingelogd! Druk op Enter om verder te gaan.");
                    Console.ReadLine();
                }
            }
            else //reset het inlogscherm wanneer een nog niet geregistreerde gebruikersnaam wordt gegeven
            { 
                Console.WriteLine("Verkeerde gebruikersnaam. Druk op Enter om het opnieuw te proberen of type 'terug' om terug te gaan.");
                if (Console.ReadLine() != "terug")
                {
                    Inlogscherm(gebruiker);
                }
            }
        }
    }
}