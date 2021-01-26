using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet.Objects
{
    public class Content
    {
        [JsonProperty("contentId")]
        public int ContentId { get; set; }
        [JsonProperty("contentName")]
        public string Name { get; set; }
        [JsonProperty("objectId")]
        public int ObjectId { get; set; }
        [JsonProperty("objectType")]
        public string ObjectType { get; set; }
        [JsonProperty("shareDT")]
        public DateTime UploadDate { get; set; }
    }
}
