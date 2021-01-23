using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet
{
    public class Lesson
    {
        [JsonProperty("evtDate")]
        public DateTime Date { get; set; }

        [JsonProperty("authorName")]
        public string Teacher { get; set; }

        [JsonProperty("subjectDesc")]
        public string Subject { get; set; }

        [JsonProperty("lessonType")]
        public string Type { get; set; }

        [JsonProperty("lessonArg")]
        public string Argument { get; set; }
    }
}
