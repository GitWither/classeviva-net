using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet.Internal
{
    internal struct GradesResponse
    {
        [JsonProperty("grades")]
        public Grade[] Grades { get; set; }
    }
}
