using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace APIGate.Models.Poton
{
    public class PhotonAuthRequest
    {
        [JsonProperty(Required = Required.Always)]
        public int ResultCode { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string UserId { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string Message { get; set; }

    }

    public class PhotonAuthResponse
    {
        [JsonProperty(Required = Required.Always)]
        public int ResultCode { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string UserId { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string Message { get; set; }

    }
}
