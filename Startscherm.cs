using System;
using System.Collections.Generic;
using System.IO;
using static TheFatDuckRestaurant.Menucode;
using static TheFatDuckRestaurant.Inloggen;

namespace TheFatDuckRestaurant
{
    class Startscherm
    {
        static void Main(string[] args)
        {
            //Menu menu = instantiateMenu();
            //addItemMenu(menu.Voorgerechten);
            //KiesMenu(menu);
            StartschermFunctie();
        }
        public static void StartschermFunctie()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                Console.WriteLine("The Fat Duck Restaurant\x0aToets op 1 om meer informatie te zien over The Fat Duck\x0a");
                Console.WriteLine("Inloggen\x0aToets op 2 om in te loggen bij The Fat Duck\x0a");
                Console.WriteLine("Menu Bekijken\x0aKlik op 3 om het menu te bezichtigen\x0a");
                Console.WriteLine("Afsluiten\x0aKlik op 4 om de applicatie te sluiten");

                if (verkeerdeInput)
                    Console.WriteLine("Verkeerde input, probeer 1,2,3 of 4");

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                switch (userInputChar)
                {
                    case '1':
                        TheFatDuckInformatie();
                        Console.WriteLine("Test");
                        break;
                    case '2':
                        Login();
                        break;
                    case '3':
                        KiesMenu();
                        break;
                    case '4':
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
                Console.WriteLine("Informatie over Fat Duck restaurant\n\n");
                Console.WriteLine("<Informatie over the Fat Duck>\n\n");
                Console.WriteLine("\x0aKlik op 1 om terug naar het startscherm te gaan");
                if (verkeerdeInput)
                    Console.WriteLine("VerkeerdeInput, probeer q");

                ConsoleKeyInfo userInput = Console.ReadKey();
                char userInputChar = userInput.KeyChar;
                if(userInputChar == '1')
                {
                    passed = true;
                    return;
                }
                verkeerdeInput = true;
            }
        }
    }
}
