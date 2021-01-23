using Newtonsoft.Json;

using System;

namespace ClassevivaNet
{
    public class Homework
    {
        /// <summary>
        /// ID of the assignment
        /// </summary>
        [JsonProperty("evtId")]
        public string Id { get; set; }
        /// <summary>
        /// Title of the assignment
        /// </summary>
        [JsonProperty("subjectDesc")]
        public string Title { get; set; }
        /// <summary>
        /// Date on which the assignment started
        /// </summary>
        [JsonProperty("evtDatetimeBegin")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Date on which the assignment ended
        /// </summary>
        [JsonProperty("evtDatetimeEnd")]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Whether the assignment is for the whole day
        /// </summary>
        [JsonProperty("isFullDay")]
        public bool IsAllDay { get; set; }
        /// <summary>
        /// Author of the assignment
        /// </summary>
        [JsonProperty("authorName")]
        public string Author { get; set; }
        /// <summary>
        /// Content of the assignment
        /// </summary>
        [JsonProperty("notes")]
        public string Content { get; set; }
    }
}
