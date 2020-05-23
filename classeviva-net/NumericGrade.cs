using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet
{
    public class NumericGrade : Grade
    {
        public double Value { get; }

        public NumericGrade(double value, string comment, DateTime date, string subject, string type) : base(comment, date, subject, type)
        {
            Value = value;
        }
    }
}
