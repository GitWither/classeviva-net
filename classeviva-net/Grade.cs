using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet
{
    public class Grade
    {
        public string Comment { get; }
        public DateTime Date { get; }
        public string Subject { get; }
        public string Type { get; }

        public Grade (string comment, DateTime date, string subject, string type)
        {
            Comment = comment;
            Date = date;
            Subject = subject;
            Type = type;
        }

        public virtual string GetGradeString()
        {
            return string.Empty;
        }
    }
}
