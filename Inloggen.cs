
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static TheFatDuckRestaurant.Startscherm;
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
                            Console.WriteLine("1: Reserveren" + "\x0A\x0A" + "2: Uitloggen" + "\x0A\x0A" + "Q: Sluit de applicatie");
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
                        Console.WriteLine("Inloggen\x0A\x0A"+"1: Login als klant" + "\x0A\x0A" + "2: Login als medewerker\x0A\x0A"+"3: Ga terug naar het startscherm");
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
                            case '3':
                                Run = false;
                                break;
                            default:
                                Console.WriteLine("\x0AOngeldige input" + "\x0A\x0A" + "Enter: Probeer het opnieuw\x0A\x0A" + "1: Ga terug naar het startscherm");
                                if(Console.ReadLine() == "1")
                        {
                            Run = false;
                        }
                                break;
                        }
            }
            return null;
        }
            //}


            public static Dictionary<string, dynamic> Inlogscherm(Gebruiker[] gebruiker, Gebruikers gebruikers)
            {
                Func<int, Gebruiker[], Tuple<bool, string>> CheckWachtwoord = (index, gebruiker) => { string Input = Console.ReadLine(); return Tuple.Create(Input == gebruiker[index].Wachtwoord || Input == "1", Input); };
                //returnt een tuple die aangeeft of de input het juiste wachtwoord of 'terug' is en de input als een string
                bool NaamBestaat = false;
                Console.Clear();
                Console.WriteLine("Voer uw gebruikersnaam in\x0A\x0A" + "1: Ga terug naar het vorige scherm");
                string GegevenNaam = Console.ReadLine();
                if (GegevenNaam != "1")
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
                        Console.Clear();
                        Console.WriteLine($"Gebruikersnaam: {GegevenNaam}\x0A\x0AVoer uw wachtwoord in");
                        Tuple<bool, string> Password = CheckWachtwoord(index, gebruiker);
                        while (!Password.Item1) //blijft om het wachtwoord vragen totdat het juiste wachtwoord voor de gebruikersnaam wordt gegeven of er 'terug' wordt getypt
                        {
                            Console.WriteLine("Verkeerd wachtwoord\x0A\x0A\x0AVoer uw wachtwoord in\x0A\x0A"+"1: Ga terug naar het vorige scherm");
                            Password = CheckWachtwoord(index, gebruiker);
                        }
                        if (Password.Item2 != "1") //sluit het inlogscherm af wanneer 'terug' was getypt
                        {
                            Console.Clear();
                            Console.WriteLine("U bent ingelogd!\x0A\x0A" +"Enter: Ga terug naar het sartscherm");
                            Dictionary<string, dynamic> dic = new Dictionary<string, dynamic>();
                            dic.Add("Gebruiker", gebruiker[index]);
                            dic.Add("Medewerker", gebruiker == gebruikers.Medewerkers);
                        Console.ReadKey();
                            return dic;
                            //return Tuple.Create(gebruiker, index);
                        }
                    }
                    else //reset het inlogscherm wanneer een nog niet geregistreerde gebruikersnaam wordt gegeven of sluit het inlogscherm af wanneer 'terug' is getypt
                    {
                        Console.WriteLine("Verkeerde gebruikersnaam.\x0A\x0A" + "Enter: Probeer opnieuw in te loggen\x0A\x0A"+"1: Ga terug naar het startscherm");
                        if (Console.ReadKey().KeyChar != '1')
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