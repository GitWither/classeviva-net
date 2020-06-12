using System;

namespace ClassevivaNet
{
    public class NumericGrade : Grade
    {
        public double Value { get; }

        public NumericGrade(double value, string comment, DateTime date, string subject, string type, bool countsTowardsAverage) : base(comment, date, subject, type, countsTowardsAverage)
        {
            Value = value;
        }

        public override string GetGradeString()
        {
            return Value.ToString();
        }
    }
}
