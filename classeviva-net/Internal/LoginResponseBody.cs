using Newtonsoft.Json;

namespace ClassevivaNet.Internal
{
    internal struct ResponseBody
    {
        public Data data;
        public string[] error;
    }

    internal struct Data
    {
        public Auth auth;
    }

    internal struct Auth
    {
        public bool loggedIn;
        public bool verified;
        public string[] errors;
        public AccountInfo accountInfo;
    }

    internal struct AccountInfo
    {
        [JsonProperty("nome")]
        public string name;
        [JsonProperty("cognome")]
        public string surname;
    }
}
