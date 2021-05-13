﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static TheFatDuckRestaurant.MainClass;
using static TheFatDuckRestaurant.Reserveren;
namespace TheFatDuckRestaurant
{
    public class Gebruikers
    {
        public Gebruiker[] Klanten { get; set; }
        public Gebruiker[] Medewerkers { get; set; }

        public Gebruikers() { } //Empty constructor for json deserialisen.
        public Gebruikers(Gebruiker[] klanten, Gebruiker[] medewerkers)
        {
            this.Klanten = klanten;
            this.Medewerkers = medewerkers;
        }
    }

    public class Gebruiker
    {
        public string Naam { get; set; }
        public string Wachtwoord { get; set; }

        public Gebruiker(string naam, string wachtwoord)
        {
            this.Naam = naam;
            this.Wachtwoord = wachtwoord;
        }
        public Gebruiker() { }

        public virtual char startScherm()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.GeneralArt());
                Console.WriteLine("1: Informatie\x0a");
                Console.WriteLine("2: Login\x0a");
                Console.WriteLine("3: Bezichtig het menu\x0a");

                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer 1,2,3");
                    verkeerdeInput = false;
                }

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                switch (userInputChar)
                {
                    case '1':
                        return '1';
                    case '2':
                        return '2';
                    case '3':
                        return '4';
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
            return '0'; 
        }
    }

    public class Klant : Gebruiker
    {
        public Reservering[] Reserveringen { get; set; }

        public Klant(string naam, string wachtwoord, Reservering[] reserveringen) : base(naam, wachtwoord)
        {
            this.Reserveringen = reserveringen;
        }
        public override char startScherm()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.GeneralArt());
                Console.WriteLine("1: Informatie\x0a");
                Console.WriteLine("2: Logout\n");
                Console.WriteLine("3: Bezichtig het menu\x0a");
                Console.WriteLine("4: Reserveren\x0a");

                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer 1,2,3 of 4");
                    verkeerdeInput = false;
                }

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                switch (userInputChar)
                {
                    case '1':
                        return '1';
                    case '2':
                        return '3';
                    case '3':
                        return '4';
                    case '4':
                        return '5';
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
            return '0';
        }
    }

    public class Medewerker : Gebruiker
    {
        public override char startScherm()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.GeneralArt());
                Console.WriteLine("1: Informatie\x0a");
                Console.WriteLine("2: Logout\n");
                Console.WriteLine("3: Bezichtig het menu\x0a");
                Console.WriteLine("4: Bezichtig reserveringen\x0a");
                Console.WriteLine("5: Bezichtig de Clickstream\x0a");
                Console.WriteLine("6: Bezichtig de dagelijkse opbrengsten\x0a");
                Console.WriteLine("0: Applicatie afsluiten\x0a");

                if (verkeerdeInput)
                {
                    Console.WriteLine("Verkeerde input, probeer 1,2,3,4,5,6 of 0");
                    verkeerdeInput = false;
                }

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                switch (userInputChar)
                {
                    case '1':
                        return '1';
                    case '2':
                        return '3';
                    case '3':
                        return '4';
                    case '4':
                        return '5';
                    case '5':
                        return '7';
                    case '6':
                        return '6';
                    case '0':
                        return '0';
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
            return '0';
        }
    }

    public class Eigenaar : Medewerker
    {
        public override char startScherm()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.GeneralArt());
                Console.WriteLine("1: Informatie\x0a");
                Console.WriteLine("2: Login\x0a");
                Console.WriteLine("3: Logout\n");
                Console.WriteLine("4: Bezichtig het menu\x0a");
                Console.WriteLine("5: Bezichtig reserveringen\x0a");
                Console.WriteLine("6: Bezichtig de Clickstream\x0a");
                Console.WriteLine("0: Applicatie afsluiten\x0a");

                if (verkeerdeInput)
                    Console.WriteLine("Verkeerde input, probeer 1,2,3");

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                switch (userInputChar)
                {
                    case '1':
                        return '1';
                    case '2':
                        return '2';
                    case '3':
                        return '4';
                    case '4':
                        return '3';
                    case '5':
                        return '5';
                    case '6':
                        return '7';
                    case '0':
                        return '0';
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
            return '0';
        }
    }

    public class Inloggen
    {
        public static Dictionary<string, dynamic> Login()
        {
            var jsonString = File.ReadAllText("gebruikers.json");
            Gebruikers gebruikers = JsonSerializer.Deserialize<Gebruikers>(jsonString);
            bool passed = false;
            bool wrongInput = false;
            Dictionary<string, dynamic> HuidigeGebruiker = null;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.LoginArt());
                Console.WriteLine("1: Login als klant\n\n2: Registreer als klant\n\n3: Login als medewerker\n\n0: Ga terug naar het startscherm");
                if (wrongInput)
                {
                    Console.WriteLine("Verkeerde input! Probeer 1, 2, 3 of 0");
                    wrongInput = false;
                }
                ConsoleKeyInfo Choice = Console.ReadKey();
                char ChoiceChar = Choice.KeyChar;
                switch (ChoiceChar)
                {
                    case '1':
                        HuidigeGebruiker = Inlogscherm(gebruikers.Klanten, gebruikers);
                        return HuidigeGebruiker;
                    case '2':
                        var NieuweGebruiker = Registreren.Registreerscherm();
                        return NieuweGebruiker;
                    case '3':
                        HuidigeGebruiker = Inlogscherm(gebruikers.Medewerkers, gebruikers);
                        return HuidigeGebruiker;
                    case '0':
                        return null;
                        break;
                    default:
                        wrongInput = true;
                        break;
                }
            }
            return null;
        }

        public static Dictionary<string, dynamic> Inlogscherm(Gebruiker[] gebruiker, Gebruikers gebruikers)
        {
            Func<int, Gebruiker[], Tuple<bool, string>> CheckWachtwoord = (index, gebruiker) =>
            {
                string Input = Console.ReadLine();
                return Tuple.Create(Input == gebruiker[index].Wachtwoord || Input == "1", Input);
            };

            //returnt een tuple die aangeeft of de input het juiste wachtwoord of 'terug' is en de input als een string
            bool NaamBestaat = false;
            Console.Clear();
            Console.WriteLine(ASCIIART.LoginArt());
            Console.WriteLine("Voer uw gebruikersnaam in\x0A\x0A" + "0: Terug");
            string GegevenNaam = Console.ReadLine();
            if (GegevenNaam != "0")
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
                    Console.WriteLine(ASCIIART.LoginArt());
                    Console.WriteLine($"Gebruikersnaam: {GegevenNaam}\x0A\x0AVoer uw wachtwoord in");
                    Tuple<bool, string> Password = CheckWachtwoord(index, gebruiker);
                    while (!Password.Item1) //blijft om het wachtwoord vragen totdat het juiste wachtwoord voor de gebruikersnaam wordt gegeven of er 'terug' wordt getypt
                    {
                        Console.WriteLine("Verkeerd wachtwoord\x0A\x0A\x0AVoer uw wachtwoord in\x0A\x0A" + "1: Ga terug naar het vorige scherm");
                        Password = CheckWachtwoord(index, gebruiker);
                    }
                    if (Password.Item2 != "1") //sluit het inlogscherm af wanneer 'terug' was getypt
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.LoginArt());
                        Console.WriteLine("U bent ingelogd!\x0A\x0A" + "Enter: Ga terug naar het sartscherm");
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
                    Console.WriteLine("Verkeerde gebruikersnaam.\x0A\x0A" + "Enter: Probeer opnieuw in te loggen\x0A\x0A" + "0: Terug");
                    if (Console.ReadKey().KeyChar != '0')
                    {
                        return Inlogscherm(gebruiker, gebruikers);
                    }
                    return null;
                }
            }
            return null;
        }

    }
    class Registreren
    {
        public static Dictionary<string, dynamic> Registreerscherm()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var jsonString = File.ReadAllText("gebruikers.json");
            Gebruikers gebruikers = JsonSerializer.Deserialize<Gebruikers>(jsonString);

            Console.Clear();
            Console.WriteLine(ASCIIART.RegistrerenArt());
            Console.WriteLine("Voer uw gebruikers naam in\n0: Terug");
            var naamInput = Console.ReadLine();
            Console.Clear();

            if (naamInput != "0")
            {
                foreach (var gebruiker in gebruikers.Klanten)
                {
                    while (gebruiker.Naam == naamInput)
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.RegistrerenArt());
                        Console.WriteLine("Deze naam bestaat al in het systeem! Probeer een andere");
                        naamInput = Console.ReadLine();
                    }
                }
                Console.Clear();
                Console.WriteLine(ASCIIART.RegistrerenArt());
                Console.WriteLine("Voer uw wachtwoord in:");
                string password = Console.ReadLine(); //TODO: Check voor password met requirements

                Gebruiker[] nieuweGebruikerLijst = new Gebruiker[gebruikers.Klanten.Length + 1];

                for(int i = 0; i < gebruikers.Klanten.Length; i++)
                {
                    nieuweGebruikerLijst[i] = gebruikers.Klanten[i]; //Voert alle oude gebruikers als Gebruiker object in de nieuwe lijst
                }
                nieuweGebruikerLijst[nieuweGebruikerLijst.Length - 1] = new Gebruiker(naamInput,  password); //voegt nieuwe gebruiker toe aan lijst
                gebruikers.Klanten = nieuweGebruikerLijst; //Klanten array van gebruikers wordt aangepast naar de nieuwe lijst die is gemaakt.
                var toSerializeKlant = JsonSerializer.Serialize(gebruikers, jsonOptions);
                File.WriteAllText("gebruikers.json", toSerializeKlant);


                Console.Clear();
                Console.WriteLine(ASCIIART.RegistrerenArt());
                Console.WriteLine($"Welkom nieuwe gebruiker: {naamInput}!\n0: Enter");
                Console.ReadKey();
                return null;
            }

            else
            {
                return null;
            }
        }
    }
}