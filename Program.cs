
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
        public string Naam { get; set; }
        public string Wachtwoord { get; set; }
        public Reservatie[] Reservaties { get; set; }
    }
    public class Reservatie
    {
        public int Tijd { get; set; }
        public int Dag { get; set; }
        public int Maand { get; set; }
        public int Jaar { get; set; }
        public int Personen { get; set; }
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
                    Reserveren(HuidigeGebruiker["Gebruiker"]);
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
                    if (GegevenNaam == gebruiker[i].Naam) { NaamBestaat = true; index = i; }
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
            Tuple<int,int,int>[] test = Data(DateTime.Now);
            for(int i = 0; i < test.Length; i++)
            {
                Console.WriteLine($"{test[i].Item1}/{test[i].Item2}/{test[i].Item3}");
            }
        }

        public static Tuple<int,int,int>[] Data(DateTime Date)
        {
            int Day = Date.Day, Month = Date.Month, Year = Date.Year;
            Tuple<int, int, int>[] tarr = new Tuple<int, int, int>[14];
            tarr[0] = Tuple.Create(Day, Month, Year);
            for(int i = 1; i < 14; i++)
            {
                Day++;
                if(Month == 1 || Month == 3 || Month == 5 || Month == 7 || Month == 8 || Month == 10 || Month == 12)
                {
                    if(Day > 31)
                    {
                        Day = 1;
                        if(Month == 12)
                        {
                            Month = 1;
                            Year++;
                        }
                        else
                        {
                            Month++;
                        }
                    }
                }
                else if(Month == 2)
                {
                    if(Year % 4 == 0 && (Year % 100 != 0 || Year % 400 == 0))
                    {
                        if(Day > 29)
                        {
                            Day = 1;
                            Month++;
                        }
                    }
                    else if(Day > 28)
                    {
                        Day = 1;
                        Month++;
                    }
                }
                else
                {
                    if(Day > 30)
                    {
                        Day = 1;
                        Month++;
                    }
                }
                tarr[i] = Tuple.Create(Day, Month, Year);
            }
            return tarr;
        }
    }
}