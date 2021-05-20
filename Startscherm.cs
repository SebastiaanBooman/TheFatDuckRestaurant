﻿using System;
using System.Collections.Generic;
using System.IO;
using static TheFatDuckRestaurant.Menu;
//using static TheFatDuckRestaurant.Inloggen;
using static TheFatDuckRestaurant.Reserveren;
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

        public Gebruiker gebruiker = new Gebruiker("", "", "", "");

        public Clickstream clickstream = new Clickstream();

        public void StartFunctie()
        {
            bool passed = false;
            while (!passed)
            {
                char gebruikerInput = gebruiker.startScherm(); // ALL possible input: 1: Fat duck informatie, 2: Login/registratie, 3: Logout, 4: Menu bekijken/aanpassen, 5: Reserveer als klant/reservering koppelen als medewerker, 6: daily revenue bekijken, 7: clickstream van klanten bekijken, 8: Applicatie afsluiten, 9: Eigen reserveringen inkijken als klant, A: Account bekijken van een gebruiker, B: Medewerker toevoegen als eigenaar

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
                                updateGebruikers(this.gebruikers);
                                updateReserveerlijst(this.reserveerLijst);
                                //clickstream.bekijkClicks(1100);
                            }
                        }
                        else if(gebruiker as Medewerker != null)
                        {
                            //medewerker tafels koppelen aan reserveringen.
                        }
                        //reserveerLijst = gebruiker.Reserveer(menu, reserveerLijst); //veranderd de reserveerLijst als er wordt gereserveerd door een gebruiker, ook als een medewerker/eigenaar de reserveringen wilt inzien/koppelen/aanpassen
                        break;
                    case '6':
                        (gebruiker as Medewerker).DailyRevenue(this.reserveerLijst.Reserveringen);
                        break;
                    case '7':
                        //clickstream van klanten
                        break;
                    case '8':
                        passed = true; // Applicatie afsluiten als eigenaar
                        break;
                    case '9':
                        //reserveerLijst.bekijkReserveringen(gebruiker as Klant);
                        gebruiker.bekijkReserveringen(reserveerLijst);
                        updateGebruikers(this.gebruikers);
                        updateReserveerlijst(this.reserveerLijst);
                        break;
                    case 'A':
                        gebruiker.bekijkAccount();
                        break;
                    case 'B':
                        gebruikers.registreerMedewerker();
                        break;
                }
            }
        }
        public void theFatDuckInformatie()
        {
            bool passed = false;
            bool verkeerdeInput = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine(ASCIIART.InformatieArt());
                Console.WriteLine("<Informatie over the Fat Duck>\n\n");
                Console.WriteLine("0: Terug naar startscherm");
                if (verkeerdeInput)
                {
                    Console.WriteLine("VerkeerdeInput, probeer 0");
                    verkeerdeInput = false;
                }
                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                if (userInputChar == '0')
                {
                    passed = true;
                    return;
                }
                verkeerdeInput = true;
            }
        }
    }
}
