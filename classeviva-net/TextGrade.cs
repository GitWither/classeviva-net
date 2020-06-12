using System;

namespace ClassevivaNet
{
    public class TextGrade : Grade
    {
        public string Value { get; }

        public TextGrade(string value, string comment, DateTime date, string subject, string type, bool countsTowardsAverage) : base(comment, date, subject, type, countsTowardsAverage)
        {
            Value = value;
        }

        public override string GetGradeString()
        {
            return Value;
        }
    }
}
