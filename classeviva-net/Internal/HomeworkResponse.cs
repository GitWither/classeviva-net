using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet.Internal
{
    internal struct HomeworkResponse
    {
        [JsonProperty("agenda")]
        public Homework[] Homework { get; set; }
    }
}
