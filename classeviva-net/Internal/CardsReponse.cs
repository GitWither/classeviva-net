using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet.Internal
{
    internal struct CardsReponse
    {
        [JsonProperty("cards")]
        public Card[] Cards { get; set; }
    }

    internal struct Card
    {
        [JsonProperty("schDedication")]
        public string SchoolName { get; set; }

        [JsonProperty("schName")]
        public string SchoolType { get; set; }

        [JsonProperty("fiscalCode")]
        public string FiscalCode { get; set; }
    }
}
