using System;
using System.Collections.Generic;
using System.Text;

namespace ClaimManagement.Domain.Common
{
    public class AuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        //We can extend this class for additional properties like
        //Assuming that in future we authentication and authorization is in plcace
        //CreatedBy
        //ModifiedBy
    }
}
