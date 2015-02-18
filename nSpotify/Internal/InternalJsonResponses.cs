using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nSpotify
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Cfid
    {
        [JsonProperty("error")]
        public Error Error { get; protected set; }

        [JsonProperty("token")]
        public string Token { get; protected set; }

        [JsonProperty("version")]
        public string Version { get; protected set; }

        [JsonProperty("client_version")]
        public string ClientVersion { get; protected set; }

        [JsonProperty("running")]
        public bool Running { get; protected set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Error
    {
        [JsonProperty("type")]
        public string Type { get; protected set; }

        [JsonProperty("message")]
        public string Message { get; protected set; }
    }
}
