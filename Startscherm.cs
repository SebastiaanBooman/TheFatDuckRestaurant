using System;
using System.Collections.Generic;
using System.IO;
using static TheFatDuckRestaurant.Menucode;
using static TheFatDuckRestaurant.Inloggen;
using static TheFatDuckRestaurant.Reserveren;

namespace TheFatDuckRestaurant
{
    class Startscherm
    {
        static void Main(string[] args) => StartschermFunctie(); //Main functie called het startscherm bij het opstarten van de applicatie
    
        public static void StartschermFunctie()
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
        }
        public static void TheFatDuckInformatie()
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
                    Console.WriteLine("VerkeerdeInput, probeer 0");

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                if(userInputChar == '0')
                {
                    passed = true;
                    return;
                }
                verkeerdeInput = true;
            }
        }
    }
}
