using System;

namespace ClassevivaNet
{
    public class Homework
    {
        /// <summary>
        /// ID of the assignment
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Title of the assignment
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// Date on which the assignment started
        /// </summary>
        public DateTime StartDate { get; }
        /// <summary>
        /// Date on which the assignment ended
        /// </summary>
        public DateTime EndDate { get; }
        /// <summary>
        /// Whether the assignment is for the whole day
        /// </summary>
        public bool IsAllDay { get; }
        /// <summary>
        /// The date on which the assignment was created
        /// </summary>
        public DateTime CreationDate { get; }
        /// <summary>
        /// Author of the assignment
        /// </summary>
        public string Author { get; }
        /// <summary>
        /// Content of the assignment
        /// </summary>
        public string Content { get; }

        public Homework(string id, string title, DateTime startDate, DateTime endDate, bool isAllDay, DateTime creationDate, string author, string content)
        {
            Id = id;
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            IsAllDay = isAllDay;
            CreationDate = creationDate;
            Author = author;
            Content = content;
        }
    }
}
