

using System;
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
            var username = Console.ReadLine();

            Console.WriteLine("Enter your password:");
            var password = Console.ReadLine();
            Console.Clear();




            Array.Resize(ref arrUsers, arrUsers.Length + 1);
            arrUsers[arrUsers.Length - 1] = new Users(username, password);
            successfull = false;

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