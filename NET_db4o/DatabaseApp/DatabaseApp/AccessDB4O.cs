using System;
using System.Collections.Generic;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;

namespace DatabaseApp
{
    class AccessDB4O
    {
        string _path;
        IObjectContainer db;

        public AccessDB4O()
        {
            //Path to dbObject.yap file
            _path = "C:/Users/adamk/Desktop/db4o/dbObject.yap";
            DeleteDatabase();
            dbOpen();
        }

        public void DeleteDatabase()
        {
            if (System.IO.File.Exists(@_path))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {
                    System.IO.File.Delete(@_path);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }

        public bool Insert<T>(T obj)
        {
            try
            {
                db.Store(obj);
            }
            finally { }
            return true;
        }

        public void UpdatePoints<T>(T obj, int points)
        {
            try
            {
                IObjectSet result = db.QueryByExample(obj);
                dbObjects.Zawodnik found = (dbObjects.Zawodnik)result.Next();
                found.setPoints(points);
                db.Store(found);
            }
            finally { }
        }


        public void getAllObjects<T>(T obj)
        {
            IObjectSet result = db.QueryByExample(obj);
            ListResult(result);
        }

        public void dbOpen()
        {
            Console.WriteLine("Opening database");
            db = Db4oEmbedded.OpenFile(_path);
        }

        public void dbClose()
        {
            Console.WriteLine("Closing database");
            db.Close();
        }

        public static void ListResult(IObjectSet result)
        {
            Console.WriteLine(result.Count);
            foreach (object item in result)
            {
                Console.WriteLine(item);
            }
        }

        public void showConstraints()
        {
            IList<dbObjects.Zawodnik> zawodnicy = db.Query<dbObjects.Zawodnik>(delegate (dbObjects.Zawodnik zawodnik) {
                return zawodnik._points > 100 || zawodnik._birthYear > 1984;
            });

            foreach (object item in zawodnicy)
            {
                Console.WriteLine(item);
            }
        }

        public void RetrieveComplexSODA()
        {
            IQuery query = db.Query();
            query.Constrain(typeof(dbObjects.Zawodnik));
            IObjectSet result = query.Execute();
            ListResult(result);
        }

    }
}
