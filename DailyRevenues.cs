﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TheFatDuckRestaurant
{
    public class DailyRevenues
    {
        public DailyRevenue[] Revenues { get; set; }
        public DailyRevenues() { }
        public bool bekijkRevenue(string Datum)
        {
            if (Revenues != null)
            {
                foreach (DailyRevenue dailyrevenue in Revenues)
                {
                    if (dailyrevenue.Datum == Datum)
                    {
                        Console.Clear();
                        Console.WriteLine(ASCIIART.OpbrengstenArt());
                        Console.WriteLine($"De opbrengst van {Datum} is {dailyrevenue.Revenue} euro\n\nEnter: Ga terug naar het vorige scherm");
                        Console.ReadKey();
                        return true;
                    }
                }
            }
            return false;
        }
        public void Add(string Datum, double Revenue)
        {
            bool Exists = false;
            if(Revenues == null)
            {
                Revenues = new DailyRevenue[0];
            }
            foreach(DailyRevenue dailyrevenue in Revenues)
            {
                if(dailyrevenue.Datum == Datum && !Exists)
                {
                    Exists = true;
                    dailyrevenue.Revenue += Revenue;
                }
            }
            if (!Exists)
            {
                DailyRevenue[] newRevenues = new DailyRevenue[Revenues.Length + 1];
                for(int i = 0; i < Revenues.Length; i++)
                {
                    newRevenues[i] = Revenues[i];
                }
                newRevenues[Revenues.Length] = new DailyRevenue(Datum, Revenue);
                Revenues = newRevenues;
            }
        }
    }
    public class DailyRevenue
    {
        public string Datum { get; set; }
        public double Revenue { get; set; }
        public DailyRevenue() { }
        public DailyRevenue(string datum, double revenue)
        {
            Datum = datum;
            Revenue = revenue;
        }
    }
}