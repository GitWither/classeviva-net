using System;

namespace ClassevivaNet
{
    public class Grade
    {
        public string Comment { get; }
        public DateTime Date { get; }
        public string Subject { get; }
        public string Type { get; }
        public bool CountsTowardsAverage { get; }

        public Grade (string comment, DateTime date, string subject, string type, bool countsTowardsAverage)
        {
            Comment = comment;
            Date = date;
            Subject = subject;
            Type = type;
            CountsTowardsAverage = countsTowardsAverage;
        }

        /// <summary>
        /// Gets the string version of a grade
        /// </summary>
        /// <returns>A stringified version of the grade</returns>
        public virtual string GetGradeString()
        {
            return string.Empty;
        }
    }
}
