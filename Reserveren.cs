using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TheFatDuckRestaurant
{
    public class Reserveren
    {
        public class ReserveerLijst
        {
            public ReserveerLijst() { }
            public Reservering[] Reserveringen { get; set; }
            public void bekijkReserveringen()
            {
                char Input = '1';
                if(this.Reserveringen.Length == 0)
                {
                    while (this.Reserveringen.Length == 0 && Input != '0')
                    {
                        Console.Clear();
                        Console.WriteLine("U heeft nog geen reserveringen gemaakt.\x0a\x0a");
                        Console.WriteLine("1: Maak een nieuwe reservering aan\x0a" + "0: Ga terug naar het startscherm");
                        Input = Console.ReadKey().KeyChar;
                        Console.Clear();
                        switch (Input)
                        {
                            case '1':
                                createReservering();
                                break;
                            case '0':
                                break;
                            default:
                                Console.WriteLine("Dit is geen geldige input\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                                Console.ReadKey();
                                break;
                        }
                    }
                }
                if(this.Reserveringen.Length > 0)
                {
                    int Index = 1;
                    while (Index != 0)
                    {
                        Console.Clear();
                        int i = 1;
                        foreach (Reservering reservering in this.Reserveringen)
                        {
                            Console.WriteLine($"{i++}: <Info reservering>");
                        }
                        Console.WriteLine($"\x0a{i}: Maak een nieuwe reservering aan\x0a" + "0: Ga terug naar het startscherm");

                        Index = Int32.Parse(Console.ReadKey().KeyChar.ToString());
                        Console.Clear();
                        if (Index == 0)
                        {
                            return;
                        }
                        else if (Index == i)
                        {
                            createReservering();
                        }
                        else if (Index < i && Index > 0)
                        {
                            changeReservering(this.Reserveringen[Index-1]);
                        }
                        else
                        {
                            Console.WriteLine("Dit is geen geldige input\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                            Console.ReadKey();
                        }
                    }
                }
            }
            public void changeReservering(Reservering reservering)
            {
                if(createReservering(reservering.Tijd, reservering.Datum, reservering.Personen))
                {
                    removeReservering(reservering);
                }
            }
            private void removeReservering(Reservering reservering)
            {
                Reservering[] newReserveringen = new Reservering[this.Reserveringen.Length - 1];
                for (int i = 0, j = 0; i < this.Reserveringen.Length; i++)
                {
                    if (this.Reserveringen[i] != reservering)
                    {
                        newReserveringen[j++] = this.Reserveringen[i];
                    }
                }
                this.Reserveringen = newReserveringen;
            }
            public bool createReservering(int tijd = 0, string datum = "", int personen = 0)
            {
                Reservering NieuweReservering = new Reservering(tijd,datum,personen);
                while (true)
                {
                    switch (NieuweReservering.Create())
                    {
                        case '1':
                            string NieuweDatum = NieuweReservering.changeDatum();
                            if(NieuweDatum != null)
                            {
                                if(NieuweReservering.Personen <= VrijePlaatsen(NieuweReservering.Tijd, NieuweDatum))
                                {
                                    NieuweReservering.Datum = NieuweDatum;
                                }
                                else
                                {
                                    Console.WriteLine("Er zijn niet genoeg vrije plaatsen op deze dag op dit tijdstip\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                                    Console.ReadKey();
                                }
                            }
                            break;
                        case '2':
                            int NieuweTijd = NieuweReservering.changeTijd();
                            if (NieuweTijd != 0)
                            {
                                if (NieuweReservering.Personen <= VrijePlaatsen(NieuweTijd, NieuweReservering.Datum))
                                {
                                    NieuweReservering.Tijd = NieuweTijd;
                                }
                                else
                                {
                                    Console.WriteLine("Er zijn niet genoeg vrije plaatsen op deze dag op dit tijdstip\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                                    Console.ReadKey();
                                }
                            }
                            break;
                        case '3':
                            NieuweReservering.changePersonen(VrijePlaatsen(NieuweReservering.Tijd, NieuweReservering.Datum));
                            break;
                        case '4':
                            Console.WriteLine("<Gerechten>");
                            Console.ReadKey();
                            break;
                        case '5':
                            if (AddReservering(NieuweReservering))
                            {
                                return true;
                            }
                            break;
                        case '0':
                            Console.WriteLine("De reservering is geannuleerd\x0a\x0a" + "Enter: Ga terug naar het startscherm");
                            Console.ReadKey();
                            return false;
                        default:
                            Console.WriteLine("Dit is geen geldige input\x0a\x0a" + "Enter: Ga terug naar het vorige scherm");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            private bool AddReservering(Reservering reservering)
            {
                if (reservering.Tijd != 0 && reservering.Datum != "" && reservering.Personen != 0)
                {
                    Reservering[] newReserveringen;
                    if (this.Reserveringen != null)
                    {
                        newReserveringen = new Reservering[this.Reserveringen.Length + 1];
                        for (int i = 0; i < Reserveringen.Length; i++)
                        {
                            newReserveringen[i] = Reserveringen[i];
                        }
                        newReserveringen[Reserveringen.Length] = reservering;
                    }
                    else
                    {
                        newReserveringen = new Reservering[] { reservering };
                    }
                    this.Reserveringen = newReserveringen;
                    updateReserveerlijst();
                    Console.WriteLine("U heeft gereserveerd!\x0a\x0a" + "Enter: Ga terug naar het startscherm");
                    Console.ReadKey();
                    return true;
                }
                string Message = "";
                Message += reservering.Datum == "" ? "U heeft nog geen datum ingevuld\x0a" : "";
                Message += reservering.Tijd == 0 ? "U heeft nog geen tijd ingevuld\x0a" : "";
                Message += reservering.Personen == 0 ? "U heeft nog niet het aantal personen aangegeven\x0a" : "";
                Console.WriteLine(Message + "\x0a" + "Enter: Ga terug naar het vorige scherm");
                Console.ReadKey();
                return false;
            }
            private int VrijePlaatsen(int tijd, string datum)
            {
                int MaxPersonen = 100;
                for (int i = 0; i < this.Reserveringen.Length; i++)
                {
                    if (this.Reserveringen[i].Datum == datum && this.Reserveringen[i].Tijd > tijd - 200 && this.Reserveringen[i].Tijd < tijd + 200)
                    {
                        MaxPersonen -= Reserveringen[i].Personen;
                    }
                }
                return MaxPersonen;
            }
            private void updateReserveerlijst()
            {
                var JSONoptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                File.WriteAllText("reserveringen.json", JsonSerializer.Serialize(this.Reserveringen, JSONoptions));
            }
        }
        public class Reservering
        {
            public int Tijd { get; set; }
            public string Datum { get; set; }
            public int Personen { get; set; }
            public Reservering() { }
            public Reservering(int tijd, string datum, int personen)
            {
                this.Tijd = tijd;
                this.Datum = datum;
                this.Personen = personen;
            }
            public char Create()
            {
                Console.Clear();
                Console.WriteLine("1: Datum\t\t" + (this.Datum == "" ? "<Nog geen datum>" : $"({this.Datum})"));
                Console.WriteLine("2: Tijd\t\t\t" + (this.Tijd == 0 ? "<Nog geen tijd>" : $"({TijdString()})"));
                Console.WriteLine("3: Aantal personen\t" + (this.Personen == 0 ? "<Nog geen aantal personen>" : $"({this.Personen})"));
                Console.WriteLine($"4: Gerechten\t\t(0 gerechten)\x0a" + "5: Bevestig de reservering\x0a" + "0: Annuleer de reservering");
                char Input = Console.ReadKey().KeyChar;
                Console.Clear();
                return Input;
            }
            public void changePersonen(int MaxPersonen)
            {
                Console.WriteLine((this.Personen == 0 ? "Nog geen aantal personen" : $"({this.Personen})") + "\x0a" + $"Er zijn {MaxPersonen} plaatsen vrij\x0aVoor hoeveel personen wilt u reserveren?");
                string Input = Console.ReadLine();
                string PersonenString = "";
                foreach (char sym in Input)
                {
                    if (Char.IsDigit(sym))
                    {
                        PersonenString += sym;
                    }
                }
                int PersonenInt = PersonenString != "" ? Int32.Parse(PersonenString) : 101;
                if(PersonenInt <= MaxPersonen && PersonenInt > 0)
                {
                    this.Personen = PersonenInt;
                }
                else
                {

                    Console.WriteLine("Er zijn niet genoeg plaatsen beschikbaar om deze tijd\x0a\x0a");
                    Console.WriteLine("Enter: Ga terug naar het vorige scherm");
                    Console.ReadKey();
                }
            }
            public int changeTijd()
            {
                Console.WriteLine((Tijd == 0 ? "Nog geen tijd" : $"({TijdString()})") + "\x0aWelke tijd wilt u reserveren? (11:00 - 21:00)");
                string Input = Console.ReadLine();
                Console.Clear();
                string Tijdstring = "";
                foreach(char sym in Input)
                {
                    if (Char.IsDigit(sym))
                    {
                        Tijdstring += sym;
                    }
                }
                int TijdInt = Tijdstring != "" ? Int32.Parse(Tijdstring) : 0;
                TijdInt *= TijdInt < 100 ? 100 : 1;
                if (TijdInt <= 900 && TijdInt >= 0)
                {
                    TijdInt += 1200;
                }
                if (TijdInt <= 2100 && TijdInt >= 1100 && TijdInt % 100 < 60)
                {
                    return TijdInt;
                }
                Console.WriteLine("Deze tijd is ongeldig\x0a\x0a");
                Console.WriteLine("Enter: Ga terug naar het vorige scherm");
                Console.ReadKey();
                return 0;
            }
            public string changeDatum()
            {
                Console.WriteLine((this.Datum == "" ? "Nog geen datum" : $"({this.Datum})") + "\x0aWelke datum wilt u reserveren? (21 juni)");
                string NieuweDatum = Console.ReadLine();
                Console.Clear();
                string Dag = "";
                string Maand = "";
                foreach (char sym in NieuweDatum)
                {
                    if (Char.IsDigit(sym))
                    {
                        Dag += sym;
                    }
                    else if (Char.IsLetter(sym))
                    {
                        Maand += sym;
                    }
                }
                int DagInt = Dag != "" ? Int32.Parse(Dag) : 0;
                if (CheckMaand(Maand.ToLower()) && DagInt > 0 && DagInt < 32)
                {
                    if (CheckDag(DagInt, Maand.ToLower(), DateTime.Now.Year))
                    {
                        return $"{Dag} {Maand}";
                    }
                }
                Console.WriteLine("Deze datum bestaat niet\x0a\x0a");
                Console.WriteLine("Enter: Ga terug naar het vorige scherm");
                Console.ReadKey();
                return null;
            }
            private bool CheckMaand(string maand)
            {
                string[] Maanden = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
                for (int i = 0; i < Maanden.Length; i++)
                {
                    if (maand == Maanden[i])
                    {
                        return true;
                    }
                }
                return false;
            }
            private bool CheckDag(int Dag, string Maand, int Jaar)
            {
                if (Maand == "januari" || Maand == "maart" || Maand == "mei" || Maand == "juli" || Maand == "augustus" || Maand == "oktober" || Maand == "november")
                {
                    return true;
                }
                if (Maand == "februari")
                {
                    if (Jaar % 4 == 0 && (Jaar % 100 != 0 || Jaar % 400 == 0))
                    {
                        return Dag < 30 ? true : false;
                    }
                    return Dag < 29 ? true : false;
                }
                return Dag < 31 ? true : false;
            }
            private string TijdString()
            {
                string tstring = this.Tijd / 100 + ":" + this.Tijd % 100;
                return tstring.Length < 5 ? tstring + "0" : tstring;
            }
        }
        public static void Reserveer()
        {
            ReserveerLijst reserveerlijst = JsonSerializer.Deserialize<ReserveerLijst>(File.ReadAllText("reserveringen.json"));
            reserveerlijst.createReservering();
            reserveerlijst.changeReservering(reserveerlijst.Reserveringen[0]);
        }
    }
}
