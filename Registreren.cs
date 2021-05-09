using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using TheFatDuckRestaurant;

namespace TheFatDuckRestaurant
{
}
 /*   public class Klanten
    {
        public string Naam { get; set; }
        public string Wachtwoord { get; set; }
    }

    public class Medewerker
    {
        public string Naam { get; set; }
        public string Wachtwoord { get; set; }
    }

    public class Gebruikers
    {
        public List<Klanten> Klanten { get; set; }
        public List<Medewerker> Medewerkers { get; set; }
    } 

    class Registreren
    {
         /*   void login()
            {
                Console.WriteLine("Enter your username:");
                var username = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Enter your password:");
                var password = Console.ReadLine();
                Console.Clear();


                foreach (Users user in arrUsers)
                {
                    if (username == user.username && password == user.password)
                    {
                        Console.WriteLine("You have successfully logged in!");
                        Console.ReadLine();
                        successfull = true;
                        break;
                    }
                }
                if (!successfull)
                {
                    Console.WriteLine("Your username or password is incorect, try again");
                } 
            } 
            void register()
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                    var user = File.ReadAllText("credentials.json");
                    Gebruikers jsonDes = JsonSerializer.Deserialize<Gebruikers>(user);


                    Console.WriteLine("press 1 to register klant or press 2 to register medewerker");
                    var input = Console.ReadLine();
                    Console.Clear();

                 /*   if (input == "1")
                    {
                        Console.WriteLine("Enter your username:");
                        string username = Console.ReadLine();
                        foreach (var item in jsonDes.Klanten)
                        {
                            while (item.Naam == username)
                            {
                                Console.Clear();
                                Console.WriteLine("Username exists, enter different username");
                                username = Console.ReadLine();
                            }

                        }
                        Console.WriteLine("Enter your password:");
                        string password = Console.ReadLine();
                        Console.Clear();
                        jsonDes.Klanten.Add(new Klanten()
                            {
                                Naam = username,
                                Wachtwoord = password
                            });
                            var toSerializeKlant = JsonSerializer.Serialize(jsonDes, jsonOptions);
                            File.WriteAllText(@"C:\Users\User\source\repos\SebastiaanBooman\TheFatDuckRestaurant\credentials.json", toSerializeKlant);


                    }

                    else if (input == "2")
                    {
                        Console.WriteLine("Enter your username:");
                        string username = Console.ReadLine();
                        foreach (var item in jsonDes.Medewerkers)
                        {
                            while (item.Naam == username)
                            {
                                Console.Clear();
                                Console.WriteLine("Username exists, enter different username");
                                username = Console.ReadLine();
                            }

                        }

                            Console.WriteLine("Enter your password:");
                            string password = Console.ReadLine();
                            Console.Clear();
                            jsonDes.Medewerkers.Add(new Medewerker()
                            {
                                Naam = username,
                                Wachtwoord = password
                            });
                            var toSerializeMedewerker = JsonSerializer.Serialize(jsonDes, jsonOptions);
                            File.WriteAllText(@"C:\Users\User\source\repos\SebastiaanBooman\TheFatDuckRestaurant\credentials.json", toSerializeMedewerker); 
                }
            }


                /*
                while (!successfull)
                {
                    Console.WriteLine("Welcome to The Fat Duck! \x0APress 1 to login with your credentials \x0APress 2 to register \x0APress 3 for more informations \x0APress 4 for menu");
                    var input = Console.ReadLine();
                    Console.Clear();

                    if (input == "1")
                    {
                        login();

                    }

                    else if (input == "2")
                    {

                        register();

                    }

                    else if (input == "3")
                    {
                        Console.WriteLine("Here you can find more informations");

                    }
                    else if (input == "4")
                    {
                        Console.WriteLine("Menu should be here");
                    }
                    else
                    {
                        Console.WriteLine("Try again!");
                    }

                } 

        }

   /*     public class Users
        {
            public string username;
            public string password;


            public Users(string username, string password)
            {
                this.username = username;
                this.password = password;

            } 
        } */