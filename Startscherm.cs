using System;
using System.IO;
using System.Text.Json;

namespace TheFatDuckRestaurant
{
    public static class MainClass //MainClass bestaat om het restaurant object aan te maken in de Main, en de startfunctie van het restaurant te roepen
    {
        static void Main(string[] args)
        {
            Restaurant TheFatDuck = new Restaurant();
            TheFatDuck.StartFunctie();
        }
    }

    public class Restaurant
    {
        public Gebruikers gebruikers = JsonSerializer.Deserialize<Gebruikers>(File.ReadAllText("gebruikers.json"));    //Alle json bestanden worden aan het begin van de class restaurant geladen vanuit de files. zodat het systeem bij de startup weer up to date is.
        public ReserveerLijst reserveerLijst = JsonSerializer.Deserialize<ReserveerLijst>(File.ReadAllText("reserveringen.json"));
        public Menu menu = JsonSerializer.Deserialize<Menu>(File.ReadAllText("menu.json"));
        public TafelArray tafels = JsonSerializer.Deserialize<TafelArray>(File.ReadAllText("Tafels.json"));
        public Clickstream clickstream = JsonSerializer.Deserialize<Clickstream>(File.ReadAllText("ClickStream.json"));
        public DailyRevenues dailyRevenues = JsonSerializer.Deserialize<DailyRevenues>(File.ReadAllText("DailyRevenues.json"));

        public Gebruiker gebruiker = new Gebruiker("", "", "", ""); //Aan het begin wordt een nieuwe gebruiker aangemaakt zonder attributen. Dit is een uitgelogde gebruiker

        public void StartFunctie()
        {
            bool shutOff = false;
            UpdateReserveringen();
            while (!shutOff)
            {
                char gebruikerInput = gebruiker.startScherm(); //Liggend aan het type dat gebruiker is, zal een ander startScherm worden gecalled. (Gebruiker, Klant, Medewerker of Eigenaar). Ieder van deze classes heeft zijn eigen functionaliteit en zal ook andere of soms dezelfde characters returnen voor een bepaalde functie

                switch (gebruikerInput)
                {
                    case '1': //Informatie over Fat Duck (kan door alle gebruiker types gecalled worden)
                        this.theFatDuckInformatie();
                        break;
                    case '2': //Inloggen als Klant, Medewerker of Eigenaar, of een nieuw account registreren als Klant. Al deze veranderingen zal de gebruiker aanpassen (het wordt implicit gecast)
                        gebruiker = gebruikers.accountManager(gebruiker);
                        break;
                    case '3': //Uitloggen als Klant, Medewerker of Eigenaar. Dit zal de gebruiker variable weer implicit casten als een Gebruiker type. Waardoor het basis startScherm() zal worden getoont 
                        gebruiker = gebruikers.logOut();
                        break;
                    case '4': //Menu bekijken als Klant, Medewerker of Eigenaar. Veranderd het menu als er iets wordt aangepast.
                        menu = gebruiker.bekijkMenu(menu);
                        break;
                    case '5': //Reserveren als klant. Veranderd de reserveerlijst, clickstream en gebruikers informatie als er wordt gereserveerd.
                        if (reserveerLijst.createReservering(gebruiker.Naam, menu))
                        {
                            clickstream.addClickstream(reserveerLijst.Reserveringen[reserveerLijst.Reserveringen.Length - 1].Datum, reserveerLijst.Reserveringen[reserveerLijst.Reserveringen.Length - 1].Tijd);
                            SaveGebruikers(this.gebruikers);
                            SaveReserveerlijst(this.reserveerLijst);
                            SaveClickstream(this.clickstream);
                        }
                        break;
                    case '6': //Daily revenues bekijken als Medewerker of Eigenaar.
                        dailyRevenues.bekijkRevenue(reserveerLijst.Reserveringen);
                        break;
                    case '7': //Clickstream bekijken als Medewerker of Eigenaar.
                        clickstream.bekijkClicks();
                        break;
                    case '8': //Applicatie afsluiten als Eigenaar of Medewerker.
                        shutOff = true;
                        break;
                    case '9': //Eigen reserveringen bekijken als Klant.
                        reserveerLijst.BekijkReserveringenKlant(gebruiker.Naam, tafels);
                        SaveReserveerlijst(this.reserveerLijst);
                        break;
                    case 'A': //Eigen account bekijken als Klant.
                        gebruiker.bekijkAccount();
                        break;
                    case 'B': //Medewerker account toevoegen als Eigenaar.
                        gebruikers.registreerMedewerker();
                        break;
                    case 'C': // tafels koppelen als Medewerker
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
                Console.WriteLine("The Fat Duck Restaurant\t\t\t\t\t\tAdres: Holland Amerikakade 104, 3072 MC Rotterdam");
                Console.WriteLine("\t\t\t\t\t\t\t\tEmail: TheFatDuck@gmail.com");
                Console.WriteLine("The Fat Duck Restaurant is opgericht in 2020 door\t\tTelefoon: 010 123 4567");
                Console.WriteLine("eigenaar Jake Darcy. Met een totaal van 100 zitplaatsen\t\tOpeningstijden: 11:00 tot 23:00");
                Console.WriteLine("heeft het restaurant twee verdiepingen, 50 per verdieping.\t");
                Console.WriteLine("The Fat Duck Restaurant specialiseert zich in Zuid-Oost\t\t");
                Console.WriteLine("Aziatische gerechten en adverteert zich op het gebruik\t\t");
                Console.WriteLine("van alleen de verste producten.\n");
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
                if (CheckDatum.DatumGeweest(reservering.Datum))
                {
                    double totalRevenue = 0;
                    for(int i = 0; i < reservering.Bestelling.Count; i++)
                    {
                        totalRevenue += reservering.Bestelling[i].Prijs;
                    }
                    dailyRevenues.Add(reservering.Datum, totalRevenue);
                    reserveerLijst.removeReservering(reservering, tafels);
                }
            }
            SaveReserveerlijst(reserveerLijst);
            SaveDailyRevenues();
        }

    }
}