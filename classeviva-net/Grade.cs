using System;

namespace ClassevivaNet
{
    public class Grade
    {
        /// <summary>
        /// Teacher's comment on the grade
        /// </summary>
        public string Comment { get; }
        /// <summary>
        /// The date this grade was created
        /// </summary>
        public DateTime Date { get; }
        /// <summary>
        /// The subject this grade was gotten in
        /// </summary>
        public string Subject { get; }
        /// <summary>
        /// Type of the grade (oral, test, etc.) WARNING: Returns a string in Italian
        /// </summary>
        public string Type { get; }
        /// <summary>
        /// Whether this grade counts towards the main average grade
        /// </summary>
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
