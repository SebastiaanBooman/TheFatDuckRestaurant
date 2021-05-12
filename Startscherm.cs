﻿using System;
using System.Collections.Generic;
using System.IO;
using static TheFatDuckRestaurant.Menucode;
using static TheFatDuckRestaurant.Inloggen;
using static TheFatDuckRestaurant.Reserveren;

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
        public Gebruikers gebruikers;
        public ReserveerLijst reserveerlijst;
        public Menu menu;
        public Gebruiker gebruiker = new Gebruiker("", "");

        public void StartFunctie()
        {
            bool passed = false;
            while (!passed)
            {
                char gebruikerInput = gebruiker.startScherm();

                switch (gebruikerInput)
                {
                    case '1':
                        this.theFatDuckInformatie();
                        break;
                    case '2':
                        this.login();
                        break;
                    case '3':
                        menu = gebruiker.showMenu(menu); //Veranderd menu als er iets veranderd wordt (bijvoorbeeld door een medewerker)
                        break;
                    case '4':
                        reserveerlijst = gebruiker.Reserveer(menu); //veranderd de reserveerLijst als er wordt gereserveerd door een gebruiker
                        break;
                    case '5':
                        //etc
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
    
    //=> StartschermFunctie(); //Main functie called het startscherm bij het opstarten van de applicatie
    
     /*   public static void StartschermFunctie()
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
                Console.WriteLine("4: Reserveren\x0a");
                Console.WriteLine("0: Applicatie afsluiten\x0a");

                if (verkeerdeInput)
                    Console.WriteLine("Verkeerde input, probeer 1,2,3,4 of 0");

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                switch (userInputChar)
                {
                    case '1':
                        TheFatDuckInformatie();
                        break;
                    case '2':
                        var HuidigeGebruiker = Login();
                        break;
                    case '3':
                        KiesMenu();
                        break;
                    case '4':
                        Reserveer();
                        break;
                    case '0':
                        passed = true;
                        break;
                    default:
                        verkeerdeInput = true;
                        break;
                }
            }
        } */
