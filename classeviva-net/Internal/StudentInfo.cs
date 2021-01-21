using Newtonsoft.Json;

using System;
using System.Text.RegularExpressions;

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

        public string GetFormattedToken()
        {
            return Regex.Replace(this.Id, "\\D", string.Empty);
        }
    }
}
