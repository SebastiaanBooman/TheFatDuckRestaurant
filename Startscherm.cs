﻿using System;
using System.Collections.Generic;
using System.IO;
using static TheFatDuckRestaurant.Menu;
//using static TheFatDuckRestaurant.Inloggen;
using static TheFatDuckRestaurant.ReserveerLijst;
using static TheFatDuckRestaurant.Clickstream;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TheFatDuckRestaurant
{
    public static class MainClass
    {
        static void Main(string[] args)
        {
            Restaurant TheFatDuck = new Restaurant();
            TheFatDuck.StartFunctie();
        }
    }

    public class Restaurant
    {
        public static string jsonString = File.ReadAllText("gebruikers.json");
        public static string MenujsonString = File.ReadAllText("menu.json");
        public static string TafelsjsonString = File.ReadAllText("Tafels.json");

        public Gebruikers gebruikers = JsonSerializer.Deserialize<Gebruikers>(jsonString);
        public ReserveerLijst reserveerLijst = JsonSerializer.Deserialize<ReserveerLijst>(File.ReadAllText("reserveringen.json"));
        public Menu menu = JsonSerializer.Deserialize<Menu>(MenujsonString);
        public TafelArray tafels = JsonSerializer.Deserialize<TafelArray>(TafelsjsonString);
        public Clickstream clickstream = JsonSerializer.Deserialize<Clickstream>(File.ReadAllText("ClickStream.json"));

        public DailyRevenues dailyRevenues = JsonSerializer.Deserialize<DailyRevenues>(File.ReadAllText("DailyRevenues.json"));

        public Gebruiker gebruiker = new Gebruiker("", "", "", "");

        public void StartFunctie()
        {
            bool shutOff = false;
            UpdateReserveringen();
            while (!shutOff)
            {
                char gebruikerInput = gebruiker.startScherm(); // ALL possible input: 1: Fat duck informatie, 2: Login/registratie, 3: Logout, 4: Menu bekijken/aanpassen, 5: Reserveer als klant, 6: daily revenue bekijken, 7: clickstream van klanten bekijken, 8: Applicatie afsluiten, 9: Eigen reserveringen inkijken als klant, A: Account bekijken van een gebruiker, B: Medewerker toevoegen als eigenaar

                switch (gebruikerInput)
                {
                    case '1':
                        this.theFatDuckInformatie();
                        break;
                    case '2':
                        gebruiker = gebruikers.accountManager(gebruiker); //Veranderd de gebruiker als er wordt ingelogd of een nieuw account wordt aangemaakt
                        break;
                    case '3':
                        gebruiker = gebruikers.logOut();
                        break;
                    case '4':
                        menu = gebruiker.bekijkMenu(menu); //Veranderd menu als er iets veranderd wordt (bijvoorbeeld door een medewerker)
                        break;
                    case '5':
                        if(gebruiker as Klant != null)
                        {
                            if (reserveerLijst.createReservering(gebruiker.Naam, menu))
                            {
                                clickstream.addClickstream(reserveerLijst.Reserveringen[reserveerLijst.Reserveringen.Length-1].Datum, reserveerLijst.Reserveringen[reserveerLijst.Reserveringen.Length-1].Tijd);
                                SaveGebruikers(this.gebruikers);
                                SaveReserveerlijst(this.reserveerLijst);
                                SaveClickstream(this.clickstream);
                                //clickstream.bekijkClicks(1100);
                            }
                        }
                        else if(gebruiker as Medewerker != null)
                        {
                            //medewerker tafels koppelen aan reserveringen.
                        }
                        break;
                    case '6': //TODO: Deze input moet in DailyRevenues.cs komen en niet hier
                        string Datum = null;
                        Reservering X = new Reservering();
                        while (Datum == null)
                        {
                            Console.Clear();
                            Console.WriteLine(ASCIIART.OpbrengstenArt());
                            Console.WriteLine("Voor welke dag wilt u de opbrengst bekijken? (21 juni 2021)");
                            Datum = X.checkDatum(Console.ReadLine(), false);
                        }
                        Console.Clear();
                        if (!dailyRevenues.bekijkRevenue(Datum))
                        {
                            (gebruiker as Medewerker).DailyRevenue(this.reserveerLijst.Reserveringen, Datum);
                        }
                        break;
                    case '7':
                        clickstream.bekijkClicks();
                        //clickstream van klanten
                        break;
                    case '8':
                        shutOff= true; // Applicatie afsluiten als eigenaar
                        break;
                    case '9': //Reserveringen bekijken als klant
                        reserveerLijst.BekijkReserveringenKlant(gebruiker.Naam);
                        break;
                    case 'A':
                        gebruiker.bekijkAccount();
                        break;
                    case 'B':
                        gebruikers.registreerMedewerker();
                        break;
                    case 'C': // tafels koppelen als medewerker
                        reserveerLijst.BekijkReserveringenMedewerker(tafels);
                        SaveReserveerlijst(this.reserveerLijst);
                        break;

                }
            }
        }
        private void theFatDuckInformatie()
        {
            bool verkeerdeInput = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.InformatieArt());
                Console.WriteLine("<Informatie over the Fat Duck>\n\n");
                Console.WriteLine("0: Terug naar startscherm");
                if (verkeerdeInput)
                    Console.WriteLine("VerkeerdeInput, probeer 0");
                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                if (userInputChar == '0')
                    return;
                verkeerdeInput = true;
            }
        }
        public void SaveReserveerlijst(ReserveerLijst reserveerlijst)
        {
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            File.WriteAllText("reserveringen.json", JsonSerializer.Serialize(reserveerlijst, JSONoptions));
        }
        private void SaveGebruikers(Gebruikers gebruikers)
        {
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            File.WriteAllText("gebruikers.json", JsonSerializer.Serialize(gebruikers, JSONoptions));
        }
        private void SaveClickstream(Clickstream clickstream)
        {
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            File.WriteAllText("ClickStream.json", JsonSerializer.Serialize(clickstream, JSONoptions));
        }
        private void SaveDailyRevenues()
        {
            var JSONoptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            File.WriteAllText("DailyRevenues.json", JsonSerializer.Serialize(dailyRevenues, JSONoptions));
        }
        private void UpdateReserveringen()
        {
            foreach(Reservering reservering in reserveerLijst.Reserveringen)
            {
                if (DatumGeweest(reservering.Datum))
                {
                    double totalRevenue = 0;
                    for(int i = 0; i < reservering.Bestelling.Count; i++)
                    {
                        totalRevenue += reservering.Bestelling[i].Prijs;
                    }
                    dailyRevenues.Add(reservering.Datum, totalRevenue);
                    reserveerLijst.removeReservering(reservering);
                }
            }
            SaveReserveerlijst(reserveerLijst);
            SaveDailyRevenues();
        }
        public static bool DatumGeweest(string Datum)
        {
            string Dag = "";
            string Maand = "";
            string Jaar = "";
            foreach(char sym in Datum)
            {
                if(Char.IsDigit(sym) && Maand == "")
                {
                    Dag += sym;
                }
                else if (Char.IsDigit(sym))
                {
                    Jaar += sym;
                }
                else if (Char.IsLetter(sym) && Dag != "")
                {
                    Maand += sym;
                }
            }
            if(Int32.Parse(Jaar) < DateTime.Now.Year)
            {
                return true;
            }
            string[] Maanden = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
            int Maandint = 0;
            for(int i = 0; i < Maanden.Length; i++)
            {
                if(Maanden[i] == Maand)
                {
                    Maandint = i + 1;
                }
            }
            if(Int32.Parse(Jaar) == DateTime.Now.Year)
            {
                if(Maandint < DateTime.Now.Month || (Maandint == DateTime.Now.Month && Int32.Parse(Dag) < DateTime.Now.Day))
                {
                    return true;
                }
            }
            return false;
        }

    }
}