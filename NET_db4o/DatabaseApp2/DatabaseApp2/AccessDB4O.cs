using System;
using System.Collections.Generic;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DatabaseApp2
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

        public bool InsertDraw<T>(T obj)
        {
            try
            {
                db.Store(obj);
            }
            finally { }
            return true;
        }

        public void getAllObjects<T>(T obj)
        {
            IObjectSet result = db.QueryByExample(obj);
            ListResult(result);
        }

        public void getAllDrawsNativeQuery()
        {
            IList<dbObjects.Draw> draws = db.Query<dbObjects.Draw>(delegate (dbObjects.Draw draw) {
                return draw.drawName != null;
            });

            foreach (object item in draws)
            {
                Console.WriteLine(item);
            }
        }

        public void dbOpen()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.ObjectClass(typeof(dbObjects.Draw)).CascadeOnUpdate(true);
            Console.WriteLine("Opening database");
            db = Db4oEmbedded.OpenFile(config, _path);
        }

        public void dbClose()
        {
            Console.WriteLine("Closing database");
            db.Close();
        }

        public static void ListResult(IObjectSet result)
        {
            foreach (object item in result)
            {
                Console.WriteLine(item);
            }
        }

        public void shiftingPoints(string name, float x, float y)
        {
            dbObjects.Draw obj = new dbObjects.Draw(name, new Collection<dbObjects.Point>());
            try
            {
                IObjectSet result = db.QueryByExample(obj);
                dbObjects.Draw found = (dbObjects.Draw)result.Next();
                
                found.MovePoints(x, y);
                db.Store(found);
            }
            finally { }
        }

        public void deletePoint(string name, float x, float y)
        {
            dbObjects.Draw obj = new dbObjects.Draw(name, null);
            try
            {
                IObjectSet result = db.QueryByExample(obj);
                dbObjects.Draw found = (dbObjects.Draw)result.Next();

                foreach (dbObjects.Point point in found.points)
                {
                    if (point.x == x && point.y == y)
                    {
                        found.points.Remove(point);
                        break;
                    }
                }
                this.db.Store(found);
            }
            finally { }
        }
    }
}
