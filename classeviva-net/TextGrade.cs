using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet
{
    public class TextGrade : Grade
    {
        public string Value { get; }

        public TextGrade(string value, string comment, DateTime date, string type) : base(comment, date, type)
        {
            Value = value;
        }
    }
}
