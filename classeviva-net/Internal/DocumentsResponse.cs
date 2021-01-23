using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet.Internal
{
    internal struct DocumentsResponse
    {
        [JsonProperty("items")]
        public Document[] Documents { get; set; }
    }
}
