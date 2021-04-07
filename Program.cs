using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using TheFatDuckRestaurant;
using Newtonsoft.Json;

namespace TheFatDuckRestaurant
{
    public class Werknemers
    {
        public string Naam { get; set; }
        public string Wachtwoord { get; set; }
        public Werknemers(string naam, string wachtwoord)
        {
            Naam = naam;
            Wachtwoord = wachtwoord;
        }

    }

}

class Program
{
    static void Main(string[] args)
    {
        var arrUsers = new Users[]
        {
            new Users("Tom","test"),
            new Users("Sebastian","test"),
        };
        bool successfull = false;


        void login()
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
            Console.WriteLine("Enter your username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();
            Console.Clear();

            var user = File.ReadAllText(@"C:\Users\User\source\repos\SebastiaanBooman\TheFatDuckRestaurant\credentials.json");
            //Werknemers Tempor = JsonConvert.DeserializeObject<Werknemers>(user);
            //string test = JsonConvert.SerializeObject(Tempor, Formatting.Indented);
            //Console.WriteLine(Tempor.Naam);




            var list = JsonConvert.DeserializeObject<List<Werknemers>>(user);
            //var list = JsonSerializer.Deserialize<TheFatDuckRestaurant.Werknemers>(user);
            list.Add(new Werknemers(username, password));
            var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);


            Console.WriteLine(convertedJson);
            File.WriteAllText(@"C:\Users\User\source\repos\SebastiaanBooman\TheFatDuckRestaurant\credentials.json", convertedJson);

            //string tempor = JsonConvert.SerializeObject(newuser, Formatting.Indented);
            //
            //Console.WriteLine(tempor);





            //Werknemers werknemer1 = new Werknemers(username, password);
            //werknemer1 = JsonConvert.DeserializeObject<Werknemers>(user);
            //string jsonString = JsonConvert.SerializeObject(werknemer1);











            //Console.WriteLine(werknemers);


        }





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
}

public class Users
{
    public string username;
    public string password;


    public Users(string username, string password)
    {
        this.username = username;
        this.password = password;

    }
}