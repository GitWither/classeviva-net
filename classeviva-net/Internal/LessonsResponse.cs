using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet.Internal
{
    internal struct LessonsResponse
    {
        [JsonProperty("lessons")]
        public Lesson[] Lessons { get; set; }
    }
}
