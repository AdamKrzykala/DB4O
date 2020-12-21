using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseApp.dbObjects
{
    class Zawodnik
    {
        public string _lastName;
        public int _points;
        public int _birthYear;

        public Zawodnik(string lastname, int points, int birthyear)
        {
            _lastName = lastname;
            _points = points;
            _birthYear = birthyear;
        }

        override public string ToString()
        {
            return string.Format("Nazwisko: {0} Ilość punktów: {1} Rok urodzenia: {2}", _lastName, _points, _birthYear);
        }

        public void setPoints(int points)
        {
            _points = points;
        }

    }
}
