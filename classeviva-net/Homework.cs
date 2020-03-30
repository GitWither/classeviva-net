using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ClassevivaNet
{
    public class Homework
    {
        public string Id { get; }
        public string Title { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public bool IsAllDay { get; }
        public DateTime CreationDate { get; }
        public string Author { get; }
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
