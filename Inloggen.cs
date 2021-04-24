
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace TheFatDuckRestaurant
{
        public class Gebruikers
        {
            public Gebruiker[] Klanten { get; set; }
            public Gebruiker[] Medewerkers { get; set; }
        }

        public class Gebruiker
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


        public class Inloggen
        {
        public static Dictionary<string, dynamic> Login()
            {
                var jsonString = File.ReadAllText("gebruikers.json");
                Gebruikers gebruikers = JsonSerializer.Deserialize<Gebruikers>(jsonString);
                //Dictionary<string, dynamic> HuidigeGebruiker = null;
                bool Run = true;
                Dictionary<string, dynamic> HuidigeGebruiker = null;
                while (Run)
                {
                    Console.Clear();
                    /*if (HuidigeGebruiker != null)
                    {
                        if (!HuidigeGebruiker["Medewerker"])
                        {
                            Console.WriteLine("1 - Reserveren" + "\x0A" + "2 - Uitloggen" + "\x0A" + "Q Sluit de applicatie");
                            ConsoleKeyInfo Choice = Console.ReadKey();
                            char ChoiceChar = Choice.KeyChar;
                            switch (ChoiceChar)
                            {
                                case '1':
                                    Reserveren(HuidigeGebruiker["Gebruiker"]);
                                    Console.WriteLine("\x0AU heeft gereserveerd." + "\x0A" + "Druk op Enter om verder te gaan.");
                                    Console.ReadLine();
                                    break;
                                case '2':
                                    HuidigeGebruiker = null;
                                    Console.WriteLine("\x0AU bent uitgelogd." + "\x0A" + "Druk op Enter om verder te gaan.");
                                    Console.ReadLine();
                                    break;
                                case 'Q':
                                    Run = false;
                                    break;
                                default:
                                    Console.WriteLine("\x0AOngeldige input." + "\x0A" + "Druk op Enter om het opnieuw te proberen.");
                                    Console.ReadLine();
                                    break;
                            }

                        }
                        else
                        {
                            Console.WriteLine("1 - Uitloggen\x0AQ - Sluit de applicatie");
                            ConsoleKeyInfo Choice = Console.ReadKey();
                            char ChoiceChar = Choice.KeyChar;
                            switch (ChoiceChar)
                            {
                                case '1':
                                    HuidigeGebruiker = null;
                                    Console.WriteLine("\x0AU bent uitgelogd." + "\x0A" + "Druk op Enter om verder te gaan.");
                                    Console.ReadLine();
                                    break;
                                case 'Q':
                                    Run = false;
                                    break;
                                default:
                                    Console.WriteLine("\x0AOngeldige input." + "\x0A" + "Druk op Enter om het opnieuw te proberen.");
                                    Console.ReadLine();
                                    break;
                            }
                        }
                    }
                    else
                    {*/
                        Console.WriteLine("1 - Inloggen als klant" + "\x0A" + "2 - Inloggen als medewerker\x0AQ - Sluit de applicatie");
                        ConsoleKeyInfo Choice = Console.ReadKey();
                        char ChoiceChar = Choice.KeyChar;
                        switch (ChoiceChar)
                        {
                            case '1':
                                HuidigeGebruiker = Inlogscherm(gebruikers.Klanten, gebruikers);
                                return HuidigeGebruiker;
                            case '2':
                                HuidigeGebruiker = Inlogscherm(gebruikers.Medewerkers, gebruikers);
                                return HuidigeGebruiker;
                            case 'Q':
                                Run = false;
                                break;
                            default:
                                Console.WriteLine("\x0AOngeldige input." + "\x0A" + "Druk op Enter om het opnieuw te proberen.");
                                Console.ReadLine();
                                break;
                        }
            }
            return null;
        }
            //}


            public static Dictionary<string, dynamic> Inlogscherm(Gebruiker[] gebruiker, Gebruikers gebruikers)
            {
                Func<int, Gebruiker[], Tuple<bool, string>> CheckWachtwoord = (index, gebruiker) => { string Input = Console.ReadLine(); return Tuple.Create(Input == gebruiker[index].Wachtwoord || Input == "Q", Input); };
                //returnt een tuple die aangeeft of de input het juiste wachtwoord of 'terug' is en de input als een string
                bool NaamBestaat = false;
                Console.Clear();
                Console.WriteLine($"Voer uw gebruikersnaam in of type 'Q' om terug te gaan.");
                string GegevenNaam = Console.ReadLine();
                if (GegevenNaam != "Q")
                {
                    int index = 0;
                    for (int i = 0; i < gebruiker.Length && !NaamBestaat; i++) //checkt of de gebruikersnaam bestaat
                    {
                        if (GegevenNaam == gebruiker[i].Naam) 
                        { 
                            NaamBestaat = true; index = i; 
                        }
                    }
                    if (NaamBestaat)
                    {
                        Console.WriteLine($"\x0AVoer uw wachtwoord in.");
                        Tuple<bool, string> Password = CheckWachtwoord(index, gebruiker);
                        while (!Password.Item1) //blijft om het wachtwoord vragen totdat het juiste wachtwoord voor de gebruikersnaam wordt gegeven of er 'terug' wordt getypt
                        {
                            Console.WriteLine("Verkeerd wachtwoord. Probeer het opnieuw of type 'Q' om terug te gaan.");
                            Password = CheckWachtwoord(index, gebruiker);
                        }
                        if (Password.Item2 != "Q") //sluit het inlogscherm af wanneer 'terug' was getypt
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
                        Console.WriteLine("Verkeerde gebruikersnaam. Druk op Enter om het opnieuw te proberen of type 'Q' om terug te gaan.");
                        if (Console.ReadLine() != "Q")
                        {
                            return Inlogscherm(gebruiker, gebruikers);
                        }
                        return null;
                    }
                }
            return null;
            }
            
        }
}