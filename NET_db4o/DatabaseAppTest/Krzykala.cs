// Autor pliku: Adam Krzykala 235411 Termin: PN 13:15 Grupa D

using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace robd_zadanie
{
    public class Prostokat
    {
        private float x, y;
        private float dlugosc, wysokosc;

        public Prostokat(float x, float y, float dlugosc, float wysokosc)
        {
            this.x = x;
            this.y = y;
            this.dlugosc = dlugosc;
            this.wysokosc = wysokosc;
        }

        public override string ToString()
        {
            return string.Format("Prostokat - wierzchołek [x: {0}, y: {1}], dlugosc: {2}, wysokosc: {3}", this.x, this.y, this.dlugosc, this.wysokosc);
        }

        public void skaluj(float skala)
        {
            this.dlugosc = this.dlugosc * skala;
            this.wysokosc = this.wysokosc * skala;
        }
    }

    public class Okreg
    {
        float srodek_x, srodek_y;
        float promien;

        public Okreg(float srodek_x, float srodek_y, float promien)
        {
            this.srodek_x = srodek_x;
            this.srodek_y = srodek_y;
            this.promien = promien;
        }

        public override string ToString()
        {
            return string.Format("Okreg - srodek [x: {0}, y: {1}], promien: {2}", this.srodek_x, this.srodek_y, this.promien);
        }

        public void skaluj(float skala)
        {
            this.promien = this.promien * skala;
        }
    }

    class Rysunek
    {
        public string nazwaRysunku;
        public List<Prostokat> prostokaty;
        public List<Okreg> okregi;

        public Rysunek(string nazwaRysunku)
        {
            this.nazwaRysunku = nazwaRysunku;
            this.prostokaty = new List<Prostokat>();
            this.okregi = new List<Okreg>();
        }

        public override string ToString()
        {
            string opis = string.Format("{0}:  ", this.nazwaRysunku);
            foreach (object item in prostokaty)
            {
                opis += string.Format(" [{0}] ", item.ToString());
            }
            foreach (object item in okregi)
            {
                opis += string.Format(" [{0}] ", item.ToString());
            }

            return opis;
        }


        public void dodajProstokat(Prostokat prostokat)
        {
            this.prostokaty.Add(prostokat);
        }

        public void dodajOkreg(Okreg okreg)
        {
            this.okregi.Add(okreg);
        }

        public void skalujFigury(float skala)
        {
            foreach (Prostokat prostokat in this.prostokaty)
            {
                prostokat.skaluj(skala);
            }
            foreach (Okreg okreg in this.okregi)
            {
                okreg.skaluj(skala);
            }
        }
    }

    class DataBaseInstance
    {
        public string path;
        public IObjectContainer db;

        public void dbOpen()
        {
            Console.WriteLine("Opening database");
            IEmbeddedConfiguration dbConfig = Db4oEmbedded.NewConfiguration();
            dbConfig.Common.ObjectClass(typeof(Rysunek)).CascadeOnUpdate(true);
            db = Db4oEmbedded.OpenFile(dbConfig, this.path);
        }

        public void dbClose()
        {
            Console.WriteLine("Closing database");
            db.Close();
        }

        public void dbDelete()
        {
            if (System.IO.File.Exists(@path))
            {
                try
                {
                    Console.WriteLine("Deleting database");
                    System.IO.File.Delete(@path);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }

        public void dbAccess(string dbPath)
        {
            this.path = dbPath;
            dbDelete();
        }

        public bool Insert<T>(T obj)
        {
            try
            {
                this.db.Store(obj);
            }
            finally
            {
                Console.WriteLine("Dodano {0}", obj);
            }
            return true;
        }

        public static void ListResult(IObjectSet result)
        {
            Console.WriteLine(result.Count);
            foreach (object item in result)
            {
                Console.WriteLine(item);
            }
        }

        public void getAllObjects<T>(T obj)
        {
            IObjectSet result = db.QueryByExample(obj);
            ListResult(result);
        }

        public void skalujFiguryWRysunku(string nazwaRysunku, float skala)
        {
            Rysunek obj = new Rysunek(nazwaRysunku);
            try
            {
                IObjectSet result = db.QueryByExample(obj);
                Rysunek found = (Rysunek)result.Next();

                found.skalujFigury(skala);
                this.db.Store(found);
            }
            finally { }
        }

        public void dodajProstokatDoRysunku(string name, Prostokat prostokat)
        {
            Rysunek obj = new Rysunek(name);
            try
            {
                IObjectSet result = db.QueryByExample(obj);
                Rysunek found = (Rysunek)result.Next();

                found.dodajProstokat(prostokat);
                this.db.Store(found);
            }
            finally { }
        }

        public void dodajOkregDoRysunku(string name, Okreg okreg)
        {
            Rysunek obj = new Rysunek(name);
            try
            {
                IObjectSet result = db.QueryByExample(obj);
                Rysunek found = (Rysunek)result.Next();

                found.dodajOkreg(okreg);
                this.db.Store(found);
            }
            finally { }
        }
    }

    class MainClass
    {
        static public char DisplayMenu()
        {
            Console.WriteLine("\n\n--------Baza rysunkow technicznych----------------\n");
            Console.WriteLine("1. Wyswietl nazwy wszystkich rysunkow i ich parametry");
            Console.WriteLine("2. Dodaj nowy rysunek techniczny");
            Console.WriteLine("3. Pobierz rysunek techniczny z bazy");
            Console.WriteLine("4. Edytuj rysunek techniczny");
            Console.WriteLine("5. Zapisz 2 predefiniowane rysunki");
            Console.WriteLine("0. Wyjscie\n");
            Console.Write(" Wybierz opcje:");
            var result = Console.ReadKey();
            return result.KeyChar;
        }

        static public char EditMenu()
        {
            Console.WriteLine("\n\n----Edycja--\n");
            Console.WriteLine("1. Wyswietl parametry wszytskich figur");
            Console.WriteLine("2. Dodaj nowa figure ");
            Console.WriteLine("3. Przeskaluj rysunek");
            Console.WriteLine("0. Wyjscie do menu glownego \n");
            Console.Write(" Wybierz opcje:");
            var result = Console.ReadKey();

            return result.KeyChar;
        }

        public static void Main(string[] args)
        {
            DataBaseInstance db1 = new DataBaseInstance();
            db1.dbAccess("./db4oInstance.yap");

            char option;
            do
            {
                option = DisplayMenu();
                switch (option)
                {
                    case '1': //wyswietl nazwy rysunkow
                        Console.WriteLine("\n");
                        db1.dbOpen();
                        Rysunek rys1 = new Rysunek(null);
                        db1.getAllObjects<Rysunek>(rys1);
                        db1.dbClose();
                        Console.WriteLine("\n");
                        break;
                    case '2': //dodaj nowy rusnek techniczny
                        Console.WriteLine("\n");
                        db1.dbOpen();
                        Console.Write("Podaj nazwe rysunku: ");
                        string nazwa = Console.ReadLine().ToString();
                        Rysunek rys2 = new Rysunek(nazwa);
                        db1.Insert<Rysunek>(rys2);
                        db1.dbClose();
                        Console.WriteLine("\n");
                        break;
                    case '3': //pobierz rysunek techniczny z bazy
                        Console.WriteLine("\n");
                        db1.dbOpen();
                        Console.Write("Podaj nazwe rysunku: ");
                        string nazwa1 = Console.ReadLine().ToString();
                        Rysunek rys3 = new Rysunek(nazwa1);
                        db1.getAllObjects<Rysunek>(rys3);
                        db1.dbClose();
                        Console.WriteLine("\n");
                        break;
                    case '4': //edytuj rysunek
                        var optionTemp = EditMenu();
                        switch (optionTemp)
                        {
                            case '0':
                                break;
                            case '1':
                                Console.WriteLine("\n");
                                db1.dbOpen();
                                Console.Write("Podaj nazwe rysunku: ");
                                string nazwa5 = Console.ReadLine().ToString();
                                Rysunek rys5 = new Rysunek(nazwa5);
                                db1.getAllObjects<Rysunek>(rys5);
                                db1.dbClose();
                                Console.WriteLine("\n");
                                break;
                            case '2':
                                Console.WriteLine("\n");
                                Console.Write("1. Dodaj prostokat \n");
                                Console.Write("2. Dodaj okreg \n");
                                Console.Write(" Wybierz opcje:");
                                var optionTemp2 = Console.ReadKey();
                                switch (optionTemp2.KeyChar)
                                {
                                    case '1':
                                        Console.WriteLine("\n");
                                        db1.dbOpen();
                                        Console.Write("Podaj nazwe rysunku: ");
                                        string nazwa6 = Console.ReadLine().ToString();
                                        Console.Write("Podaj x lewego gornego punktu: ");
                                        float punkt_x = float.Parse(Console.ReadLine());
                                        Console.Write("Podaj y lewego gornego punktu: ");
                                        float punkt_y = float.Parse(Console.ReadLine());
                                        Console.Write("Podaj dlugosc: ");
                                        float dlugosc = float.Parse(Console.ReadLine());
                                        Console.Write("Podaj wysokosc: ");
                                        float wysokosc = float.Parse(Console.ReadLine());
                                        db1.dodajProstokatDoRysunku(nazwa6, new Prostokat(punkt_x, punkt_y, dlugosc, wysokosc));
                                        db1.dbClose();
                                        Console.WriteLine("\n");
                                        break;
                                    case '2':
                                        Console.WriteLine("\n");
                                        db1.dbOpen();
                                        Console.Write("Podaj nazwe rysunku: ");
                                        string nazwa7 = Console.ReadLine().ToString();
                                        Console.Write("Podaj x srodka okregu: ");
                                        float x_sr = float.Parse(Console.ReadLine());
                                        Console.Write("Podaj y srodka okregu: ");
                                        float y_sr = float.Parse(Console.ReadLine());
                                        Console.Write("Podaj promien: ");
                                        float R = float.Parse(Console.ReadLine());
             
                                        db1.dodajOkregDoRysunku(nazwa7, new Okreg(x_sr, y_sr, R));
                                        db1.dbClose();
                                        Console.WriteLine("\n");
                                        break;
                                }
                                break;
                            case '3':
                                Console.WriteLine("\n");
                                db1.dbOpen();
                                Console.Write("Podaj nazwe rysunku: ");
                                string nazwa4 = Console.ReadLine().ToString();
                                Console.Write("Podaj skale: ");
                                float skala = float.Parse(Console.ReadLine());
                                db1.skalujFiguryWRysunku(nazwa4, skala);
                                db1.dbClose();
                                Console.WriteLine("\n");
                                break;
                        }
                        break;
                    case '5': //zapisz predefinowane rysunki
                        Console.WriteLine("\n");
                        db1.dbOpen();
                        Rysunek rysPred1 = new Rysunek("Rysunek predefiniowany 1");
                        rysPred1.dodajProstokat(new Prostokat(0, 0, 10, 20));
                        rysPred1.dodajOkreg(new Okreg(5, 5, 10));
                        rysPred1.dodajOkreg(new Okreg(-5, -5, 10));

                        Rysunek rysPred2 = new Rysunek("Rysunek predefiniowany 2");
                        rysPred2.dodajProstokat(new Prostokat(4, 10, 3, 15));
                        rysPred2.dodajProstokat(new Prostokat(0, 12, 10, 1));
                        
                        //Dodawanie rysunku predefiniowanego nr 1
                        db1.Insert<Rysunek>(rysPred1);

                        //Dodawanie rysuunku predefiniowanego nr 2
                        db1.Insert<Rysunek>(rysPred2);
                        
                        db1.dbClose();
                        Console.WriteLine("\n");
                        break;

                }
            } while (option != '0');
        }
    }
}
