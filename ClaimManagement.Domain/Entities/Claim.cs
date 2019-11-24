using ClaimManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClaimManagement.Domain.Entities
{
    public class Claim : AuditableEntity
    {

        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public decimal DamageCost { get; set; }
        public ClaimType Type { get; set; }
    }
}
