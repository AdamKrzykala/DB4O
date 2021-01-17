using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //New database, deleting old one
            AccessDB4O db = new AccessDB4O();

            dbObjects.Zawodnik zawodnik1 = new dbObjects.Zawodnik("Kowalski", 100, 1984);
            dbObjects.Zawodnik zawodnik2 = new dbObjects.Zawodnik("Nowak", 97, 1988);
            dbObjects.Zawodnik zawodnik3 = new dbObjects.Zawodnik("Grabowski", 100, 1982);

            db.Insert(zawodnik1);
            db.Insert(zawodnik2);
            db.Insert(zawodnik3);

            db.dbClose();

            db.dbOpen();
            Console.WriteLine("Wszyscy zawodnicy");
            dbObjects.Zawodnik proto = new dbObjects.Zawodnik(null, 0, 0);
            db.getAllObjects(proto);
            Console.WriteLine("");

            Console.WriteLine("Zawodnicy z liczbą punktów 100");
            dbObjects.Zawodnik proto2 = new dbObjects.Zawodnik(null, 100, 0);
            db.getAllObjects(proto2);
            Console.WriteLine("");

            Console.WriteLine("Aktualizacja pkt dla Kowalski");
            dbObjects.Zawodnik proto3 = new dbObjects.Zawodnik("Kowalski", 0, 0);
            db.UpdatePoints(proto3, 110);

            Console.WriteLine("Dodawanie Andrzejewski");
            dbObjects.Zawodnik zawodnik4 = new dbObjects.Zawodnik("Andrzejewski", 98, 1989);
            db.Insert(zawodnik4);

            Console.WriteLine("Wszyscy zawodnicy");
            db.getAllObjects(proto);
            Console.WriteLine("");

            db.dbClose();
            db.dbOpen();
            Console.WriteLine("Native Query - wszystkie rekordy gdzie punkty > 100 lub data urodzenia > 1984");
            db.showConstraints();
            Console.WriteLine("");

            Console.WriteLine("SODA wszyscy");
            db.RetrieveComplexSODA();
            Console.WriteLine("");
            db.dbClose();

            db.dbOpen();
            Console.WriteLine("SODA Constraints");
            db.showConstraintsSODA();
            Console.WriteLine("");
            db.dbClose();
        }
    }
}
