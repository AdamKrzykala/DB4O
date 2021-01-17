using System;

namespace DatabaseApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Task 1
            AccessDB4O db = new AccessDB4O();
            dbObjects.Draw rys1 = new dbObjects.Draw("rys1");
            rys1.addPoint(0, 1);
            rys1.addPoint(3, 4);
            if(db.InsertDraw<dbObjects.Draw>(rys1)) 
                Console.WriteLine("Rys1 added");
            db.dbClose();

            //Task 2 - QBE
            db.dbOpen();
            dbObjects.Draw rysTemplate = new dbObjects.Draw(null);
            db.getAllObjects<dbObjects.Draw>(rysTemplate);
            dbObjects.Draw rys2 = new dbObjects.Draw("rys2");
            rys2.addPoint(0, 0);
            if (db.InsertDraw<dbObjects.Draw>(rys2))
                Console.WriteLine("Rys2 added");
            db.dbClose();

            //Task 3 - NQ
            db.dbOpen();
            db.getAllDrawsNativeQuery();
            db.shiftingPoints("rys1", 2, 1);
            db.dbClose();

            db.dbOpen();
            Console.WriteLine("Points shifted");
            db.getAllDrawsNativeQuery();
            db.dbClose();
        }
    }
}
