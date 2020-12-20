using Microsoft.Xrm.Sdk;
using System;
using System.Globalization;

namespace Veri.System.Xrm.Sdk
{
    public static class EntityExtensions
    {

        public static DateTime? GetDate(this Entity entity, string attribute)
        {
            DateTime? retVal = null;
            if (entity.Attributes.ContainsKey(attribute))
            {
                object typeValue = entity[attribute];
                string typeName = entity[attribute].GetType().Name;
                if (typeName == nameof(AliasedValue))
                {
                    typeValue = ((AliasedValue)typeValue).Value;
                    typeName = typeValue.GetType().Name;
                }
                switch (typeName)
                {
                    case nameof(DateTime):
                        retVal = (DateTime)typeValue;
                        break;
                }
            }
            return retVal;
        }
        public static string GetConvertedValue(this Entity entity, string attribute)
        {
            string retVal = null;
            if (entity.Attributes.ContainsKey(attribute))
            {
                object typeValue = entity[attribute];
                string typeName = entity[attribute].GetType().Name;
                if (typeName == nameof(AliasedValue))
                {
                    typeValue = ((AliasedValue)typeValue).Value;
                    typeName = typeValue.GetType().Name;
                }
                switch (typeName)
                {
                    case nameof(Guid):
                        retVal = ((Guid)typeValue).ToString();
                        break;

                    case nameof(EntityReference):
                        retVal = ((EntityReference)typeValue).Name;
                        break;

                    case "string":
                        retVal = (string)typeValue;
                        break;

                    case "int":
                        retVal = (string)typeValue;
                        break;

                    case nameof(Money):
                        decimal val = ((Money)typeValue).Value;
                        retVal = val.ToString("C2", new CultureInfo("en-GB"));
                        break;

                    case nameof(OptionSetValue):
                        retVal = entity.FormattedValues[attribute];
                        break;

                    case nameof(AliasedValue):
                        retVal = ((AliasedValue)typeValue).Value.ToString();
                        break;

                    case nameof(DateTime):
                        retVal = ((DateTime)typeValue).ToString("dd/MM/yyyy");
                        break;

                    case nameof(Boolean):
                        retVal = (typeValue).ToString();
                        break;

                    default:
                        retVal = (string)typeValue;
                        break;
                }

            }
            if (retVal != null)
            {
                retVal = retVal.Trim();
            }
            return retVal;
        }
        public static decimal GetMoneyValue(this Entity entity, string attribute)
        {
            object typeValue = entity[attribute];
            string typeName = entity[attribute].GetType().Name;
            if (typeName == nameof(AliasedValue))
            {
                typeValue = ((AliasedValue)typeValue).Value;
                typeName = typeValue.GetType().Name;
            }
            if (typeName == nameof(Money))
            {
                return ((Money)typeValue).Value;
            }
            return 0;
        }
    }
}
