
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
        public Reservatie Reservatie { get; set; }
    }
    public class Reservatie
    {
        public int Tijd { get; set; }
        public int Dag { get; set; }
        public string Maand { get; set; }
        public int Personenen { get; set; }
    }


    public class Program
    {
        static void Main(string[] args)
        {
            var jsonString = File.ReadAllText("gebruikers.json");
            Gebruikers gebruikers = JsonSerializer.Deserialize<Gebruikers>(jsonString);
            Dictionary<string,dynamic> HuidigeGebruiker = null;
            while (true)
            {
                Console.Clear();
                if (HuidigeGebruiker != null) 
                { 
                    Console.WriteLine(HuidigeGebruiker["Medewerker"]);
                    Console.WriteLine(HuidigeGebruiker["Gebruiker"].Reservatie.Maand);
                    Reserveren(HuidigeGebruiker["Gebruiker"]);
                    Console.WriteLine(HuidigeGebruiker["Gebruiker"].Reservatie.Maand);
                }
                Console.WriteLine("Wilt u als klant ('a') of als medewerker ('b') inloggen?");
                string Choice = Console.ReadLine();
                if(Choice == "a")
                {
                    HuidigeGebruiker = Inlogscherm(gebruikers.Klanten,gebruikers);
                }
                else if(Choice == "b")
                {
                    HuidigeGebruiker = Inlogscherm(gebruikers.Medewerkers,gebruikers);
                }
            }
        }
        

        public static Dictionary<string,dynamic> Inlogscherm(Inloggen[] gebruiker, Gebruikers gebruikers)
        {
            Func<int, Inloggen[], Tuple<bool, string>> CheckWachtwoord = (index, gebruiker) => { string Input = Console.ReadLine(); return Tuple.Create(Input == gebruiker[index].Wachtwoord || Input == "terug", Input); };
            //returnt een tuple die aangeeft of de input het juiste wachtwoord of 'terug' is en de input als een string
            bool NaamBestaat = false;
            Console.Clear();
            Console.WriteLine($"Voer uw gebruikersnaam in of type 'terug' om terug te gaan.");
            string GegevenNaam = Console.ReadLine();
            if (GegevenNaam != "terug")
            {
                int index = 0;
                for (int i = 0; i < gebruiker.Length && !NaamBestaat; i++) //checkt of de gebruikersnaam bestaat
                {
                    if (GegevenNaam == gebruiker[i].Gebruikersnaam) { NaamBestaat = true; index = i; }
                }
                if (NaamBestaat)
                {
                    Console.WriteLine($"\x0AVoer uw wachtwoord in.");
                    Tuple<bool, string> Password = CheckWachtwoord(index, gebruiker);
                    while (!Password.Item1) //blijft om het wachtwoord vragen totdat het juiste wachtwoord voor de gebruikersnaam wordt gegeven of er 'terug' wordt getypt
                    {
                        Console.WriteLine("Verkeerd wachtwoord. Probeer het opnieuw of type 'terug' om terug te gaan.");
                        Password = CheckWachtwoord(index, gebruiker);
                    }
                    if (Password.Item2 != "terug") //sluit het inlogscherm af wanneer 'terug' was getypt
                    {
                        Console.WriteLine($"\x0AU bent ingelogd! Druk op Enter om verder te gaan.");
                        Console.ReadLine();
                        Dictionary<string, dynamic> dic = new Dictionary<string, dynamic>();
                        dic.Add("Gebruiker", gebruiker[index]);
                        dic.Add("Medewerker", gebruiker == gebruikers.Medewerkers);
                        return dic;
                        //return Tuple.Create(gebruiker, index);
                    }
                }
                else //reset het inlogscherm wanneer een nog niet geregistreerde gebruikersnaam wordt gegeven of sluit het inlogscherm af wanneer 'terug' is getypt
                {
                    Console.WriteLine("Verkeerde gebruikersnaam. Druk op Enter om het opnieuw te proberen of type 'terug' om terug te gaan.");
                    if (Console.ReadLine() != "terug")
                    {
                        return Inlogscherm(gebruiker, gebruikers);
                    }
                }
            }
            return null;
        }
        public static void Reserveren(Inloggen klant)
        {
            klant.Reservatie.Maand = "Februari";
        }
    }
}