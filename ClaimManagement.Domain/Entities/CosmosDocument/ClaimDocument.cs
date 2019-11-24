using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClaimManagement.Domain.Entities.CosmosDocument
{
    public class ClaimDocument : AuditableDocument
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "damageCost")]
        public string DamageCost { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
