using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClaimManagement.Domain
{
    public enum ClaimType
    {
        [Description("Collision")]
        Collision = 1,
        [Description("Grounding")]
        Grounding = 2,
        [Description("Bad Weather")]
        BadWeather = 3,
        [Description("Fire")]
        Fire = 4
    }
}
