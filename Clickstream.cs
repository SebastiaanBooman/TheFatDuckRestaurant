using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static TheFatDuckRestaurant.ReserveerLijst;

namespace TheFatDuckRestaurant
{
    public class Clickstream
    {
        public CSDag[] Dagen { get; set; }
        public CSTijd[] Tijden { get; set; }
        public Clickstream() { }
        /*public Clickstream()//1 keer doen om het json bestand te schrijven
        {
            Dagen = new CSDag[]
            {
                new CSDag("maandag",0),
                new CSDag("dinsdag",0),
                new CSDag("woensdag",0),
                new CSDag("donderdag",0),
                new CSDag("vrijdag",0),
                new CSDag("zaterdag",0),
                new CSDag("zondag",0)
            };
            Tijden = new CSTijd[]
            {
                new CSTijd(1100,0),
                new CSTijd(1200,0),
                new CSTijd(1300,0),
                new CSTijd(1400,0),
                new CSTijd(1500,0),
                new CSTijd(1600,0),
                new CSTijd(1700,0),
                new CSTijd(1800,0),
                new CSTijd(1900,0),
                new CSTijd(2000,0),
                new CSTijd(2100,0)
            };
        }*/
        public void addClickstream(string Datum, int tijd)
        {
            string DigitCheck = "";
            string dag = "";
            foreach(char sym in Datum)
            {
                if (Char.IsDigit(sym))
                {
                    DigitCheck += sym;
                }
                if(Char.IsLetter(sym) && DigitCheck == "")
                {
                    dag += sym;
                }
            }
            addClicks(dag.ToLower(), tijd);
        }
        public void addClicks(string dag, int tijd)
        {
            foreach(CSDag Dag in Dagen)
            {
                if(Dag.Naam == dag)
                {
                    Dag.Clicks++;
                }
            }
            foreach(CSTijd Tijd in Tijden)
            {
                if(tijd > Tijd.Naam - 70 && tijd <= Tijd.Naam + 30)
                {
                    Tijd.Clicks++;
                }
            }
        }
        public void bekijkClicksD()
        {
            Console.Clear();
            Console.WriteLine(ASCIIART.ReserveringenArt());
            foreach (CSDag Dag in Dagen)
            {
                Console.WriteLine(Dag.Naam + $": {Dag.Clicks} reserveringen\n");
            }
            Console.WriteLine("\nEnter: Ga terug naar het vorige scherm");
            Console.ReadKey();
        }
        public void bekijkClicksT()
        {
            Console.Clear();
            Console.WriteLine(ASCIIART.ReserveringenArt());
            foreach (CSTijd Tijd in Tijden)
            {
                Console.WriteLine(Tijd.Naam + $": {Tijd.Clicks} reserveringen\n");
            }
            Console.WriteLine("\nEnter: Ga terug naar het vorige scherm");
            Console.ReadKey();
        }

    }
    public class CSDag
    {
        public string Naam { get; set; }
        public int Clicks { get; set; }
        public CSDag() { }
        public CSDag(string naam, int clicks)
        {
            Naam = naam;
            Clicks = clicks;
        }
    }
    public class CSTijd
    {
        public int Naam { get; set; }
        public int Clicks { get; set; }
        public CSTijd() { }
        public CSTijd(int naam, int clicks)
        {
            Naam = naam;
            Clicks = clicks;
        }
    }
}
