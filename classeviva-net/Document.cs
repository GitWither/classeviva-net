using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet
{
    public class Document
    {
        [JsonProperty("pubId")]
        public int Id { get; set; }
        [JsonProperty("evtCode")]
        public string Code { get; set; }
        [JsonProperty("pubDT")]
        public DateTime PublishDate { get; set; }
        [JsonProperty("cntTitle")]
        public string Title { get; set; }
        [JsonProperty("needSign")]
        public bool RequiresSignature { get; set; }
        [JsonProperty("needReply")]
        public bool RequiresReply { get; set; }
        [JsonProperty("needJoin")]
        public bool RequiresAdhesion { get; set; }
    }
}
