using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet
{
    public class Grade
    {
        public DateTime Date { get; }
        public string Comment { get; }
        public string Type { get; }

        public Grade (string comment, DateTime date, string type)
        {
            Comment = comment;
            Date = date;
            Type = type;
        }
    }
}
