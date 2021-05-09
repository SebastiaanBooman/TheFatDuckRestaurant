using System;
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
            bool Run = true;
            Dictionary<string, dynamic> HuidigeGebruiker = null;
            while (Run)
            {
                Console.Clear();
                Console.WriteLine("Inloggen\n\n1: Login als klant\n\n\n\n2: Registreer als klant\n\n3: Login als medewerker\n\n0: Ga terug naar het startscherm");
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
                        Run = false;
                        break;
                    default:
                        Console.WriteLine("\x0AOngeldige input" + "\x0A\x0A" + "Enter: Probeer het opnieuw\x0A\x0A" + "1: Ga terug naar het startscherm");
                        if (Console.ReadLine() == "1")
                        {
                            Run = false;
                        }
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
                        Console.WriteLine("Verkeerd wachtwoord\x0A\x0A\x0AVoer uw wachtwoord in\x0A\x0A" + "1: Ga terug naar het vorige scherm");
                        Password = CheckWachtwoord(index, gebruiker);
                    }
                    if (Password.Item2 != "1") //sluit het inlogscherm af wanneer 'terug' was getypt
                    {
                        Console.Clear();
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
                    Console.WriteLine("Verkeerde gebruikersnaam.\x0A\x0A" + "Enter: Probeer opnieuw in te loggen\x0A\x0A" + "1: Ga terug naar het startscherm");
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
            Console.WriteLine("Registreer als klant\n\nVoer uw gebruikers naam in\n0: Terug");
            var naamInput = Console.ReadLine();
            Console.Clear();

            if (naamInput != "0")
            {
                foreach (var gebruiker in gebruikers.Klanten)
                {
                    while (gebruiker.Naam == naamInput)
                    {
                        Console.Clear();
                        Console.WriteLine("Deze naam bestaat al! Probeer een andere");
                        string username = Console.ReadLine();
                    }
                }
                Console.Clear();
                Console.WriteLine("Voer uw wachtwoord in:");
                string password = Console.ReadLine();
                Console.Clear();

                Gebruiker[] nieuweGebruikerLijst = new Gebruiker[gebruikers.Klanten.Length + 1];

                for(int i = 0; i < gebruikers.Klanten.Length; i++)
                {
                    nieuweGebruikerLijst[i] = gebruikers.Klanten[i]; //Voert alle oude gebruikers als Gebruiker object in de nieuwe lijst
                }
                nieuweGebruikerLijst[nieuweGebruikerLijst.Length - 1] = new Gebruiker { Naam = naamInput, Wachtwoord = password }; //voegt nieuwe gebruiker toe aan lijst
                gebruikers.Klanten = nieuweGebruikerLijst; //Klanten array van gebruikers wordt aangepast naar de nieuwe lijst die is gemaakt.
                var toSerializeKlant = JsonSerializer.Serialize(gebruikers, jsonOptions);
                File.WriteAllText("gebruikers.json", toSerializeKlant);
                return null;
            }

            else
            {
                return null;
            }
        }
    }
}