using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace TheFatDuckRestaurant
{

    public class Menu
    {
        public Gerechten[] Voorgerechten { get; set; }
        public Gerechten[] Hoofdgerechten { get; set; }
        public Gerechten[] Nagerechten { get; set; }
    }

    public class Gerechten
    {
        public string naam { get; set; }
        public double prijs { get; set; }
        public string beschrijving { get; set; }
        public string[] ingredienten { get; set; }
    }



    public class Program
    {
        static void Main(string[] args)
        {
            var jsonString = File.ReadAllText("menu.json");
            Menu menu = JsonSerializer.Deserialize<Menu>(jsonString);
            
            foreach(var typeGerecht in menu.Voorgerechten)
            {
                Console.WriteLine(typeGerecht.naam);
            }
        }
    }
}
