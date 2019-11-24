using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ClaimManagement.Domain.Entities.CosmosDocument
{
    public class AuditableDocument
    {
        [JsonProperty(PropertyName = "createdAt")]
        public DateTime CreatedAt { get; set; }
        //We can extend this class for additional properties like
        //Assuming that in future we authentication and authorization is in plcace
        //CreatedBy
        //ModifiedBy
    }
}
