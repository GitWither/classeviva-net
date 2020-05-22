using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet
{
    public class NumericGrade : Grade
    {
        public double Value { get; }

        public NumericGrade(double value, string comment, DateTime date, string type) : base(comment, date, type)
        {
            Value = value;
        }
    }
}
