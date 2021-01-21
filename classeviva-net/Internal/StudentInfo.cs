﻿using Newtonsoft.Json;

using System;

namespace ClassevivaNet.Internal
{
    internal struct StudentInfo
    {
        [JsonProperty("ident")]
        public string Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("release")]
        public DateTime LoggedIn { get; set; }
        [JsonProperty("expire")]
        public DateTime ExpireTime { get; set; }
    }
}
