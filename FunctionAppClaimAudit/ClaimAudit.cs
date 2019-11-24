using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionAppClaimAudit
{
    public class ClaimAudit 
    {

        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public decimal DamageCost { get; set; }
        public string Type { get; set; }
        public string RequestName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
