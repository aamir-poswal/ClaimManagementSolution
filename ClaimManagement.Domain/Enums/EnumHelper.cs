using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ClaimManagement.Domain
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description
                ?? value.ToString();
        }

        public static Dictionary<string, string> GetClaimTypeEnumReadableValues()
        {
            Dictionary<string, string> claimTypes = new Dictionary<string, string>();
            claimTypes.Add("", "Please Select Type");
            int i = 1;
            foreach (ClaimType claimTypeEnum in Enum.GetValues(typeof(ClaimType)))
            {
                claimTypes.Add(i.ToString(), claimTypeEnum.GetDescription());
                i++;
            }
            return claimTypes;
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
            // or return default(T);
        }

    }



}
