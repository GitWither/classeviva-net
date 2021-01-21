using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;

namespace ClassevivaNet
{
    public class Grade
    {
        /// <summary>
        /// Teacher's comment on the grade
        /// </summary>
        [JsonProperty("notesForFamily")]
        public string Comment { get; set; }

        /// <summary>
        /// Decimal representation of the grade. 0 means the grade has no numeric representation
        /// </summary>
        [JsonProperty(propertyName:"decimalValue", NullValueHandling = NullValueHandling.Ignore)]
        public double DecimalValue { get; set; }

        /// <summary>
        /// String representation of the grade
        /// </summary>
        [JsonProperty("displayValue")]
        public string DisplayValue { get; set; }

        /// <summary>
        /// The date this grade was created
        /// </summary>
        [JsonProperty("evtDate")]
        public DateTime Date { get; set; }
        /// <summary>
        /// The subject this grade was gotten in
        /// </summary>
        [JsonProperty("subjectDesc")]
        public string Subject { get; set; }
        /// <summary>
        /// Type of the grade (oral, test, etc.) WARNING: Returns a string in Italian
        /// </summary>
        [JsonProperty("componentDesc")]
        public string Type { get; set; }
        /// <summary>
        /// Color of the grade. You can use this to determin whether the grade counts towards the average
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }
    }
}
