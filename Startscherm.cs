using System;
using System.IO;
using System.Text.Json;

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
                        if (reserveerLijst.createReservering(gebruiker.Naam, menu))
                        {
                            clickstream.addClickstream(reserveerLijst.Reserveringen[reserveerLijst.Reserveringen.Length - 1].Datum, reserveerLijst.Reserveringen[reserveerLijst.Reserveringen.Length - 1].Tijd);
                            SaveGebruikers(this.gebruikers);
                            SaveReserveerlijst(this.reserveerLijst);
                            SaveClickstream(this.clickstream);
                            //clickstream.bekijkClicks(1100);
                        }
                        break;
                    case '6':
                        dailyRevenues.bekijkRevenue(reserveerLijst.Reserveringen);
                        break;
                    case '7':
                        clickstream.bekijkClicks();
                        //clickstream van klanten
                        break;
                    case '8':
                        shutOff= true; // Applicatie afsluiten als eigenaar
                        break;
                    case '9': //Reserveringen bekijken als klant
                        reserveerLijst.BekijkReserveringenKlant(gebruiker.Naam, tafels);
                        SaveReserveerlijst(this.reserveerLijst);
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