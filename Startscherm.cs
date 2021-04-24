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


        public static string ASCIIART()
        {
            string ASCIILogoL1 = "  _______ _            ______    _     _____             _     ";
            string ASCIILogoL2 = " |__   __| |          |  ____|  | |   |  __ \\           | |    ";
            string ASCIILogoL3 = "    | |  | |__   ___  | |__ __ _| |_  | |  | |_   _  ___| | __ ";
            string ASCIILogoL4 = "    | |  | '_ \\ / _ \\ |  __/ _` | __| | |  | | | | |/ __| |/ / ";
            string ASCIILogoL5 = "    | |  | | | |  __/ | | | (_| | |_  | |__| | |_| | (__|   <  ";
            string ASCIILogoL6 = "    |_|  |_| |_|\\___| |_|  \\__,_|\\__| |_____/ \\__,_|\\___|_|\\_\\ ";

            string ASCIIDuckL1 = "           ..\n";
            string ASCIIDuckL2 = "          ( '`<\n";
            string ASCIIDuckL3 = "           )(\n";
            string ASCIIDuckL4 = "    ( ----'  '.\n";
            string ASCIIDuckL5 = "    (         ;\n";
            string ASCIIDuckL6 = "     (_______,'\n";


            string ASCIILogoAll = ASCIILogoL1 + ASCIIDuckL1 + ASCIILogoL2 + ASCIIDuckL2 + ASCIILogoL3 + ASCIIDuckL3 + ASCIILogoL4 + ASCIIDuckL4 + ASCIILogoL5 + ASCIIDuckL5 + ASCIILogoL6 + ASCIIDuckL6;


            Console.WriteLine(ASCIILogoAll);
            return ASCIILogoAll;

        }
        public static void StartschermFunctie()
        {
            bool verkeerdeInput = false;
            bool passed = false;
            while (!passed)
            {
                Console.Clear();
                ASCIIART();
                Console.WriteLine("1: Informatie over The Fat Duck\x0a");
                Console.WriteLine("2: Login bij The Fat Duck\x0a");
                Console.WriteLine("3: Bezichtig het menu\x0a");
                Console.WriteLine("4: Applicatie afsluiten");

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
                        var HuidigeGebruiker = Login();
                        Console.WriteLine(HuidigeGebruiker["Gebruiker"].Naam);
                        Console.ReadLine();
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
                ASCIIART();
                Console.WriteLine("Informatie over Fat Duck restaurant\n\n");
                Console.WriteLine("<Informatie over the Fat Duck>\n\n");
                Console.WriteLine("1: Terug naar startscherm");
                if (verkeerdeInput)
                    Console.WriteLine("VerkeerdeInput, probeer 1");

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
