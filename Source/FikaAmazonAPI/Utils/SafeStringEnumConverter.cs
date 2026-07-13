using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace FikaAmazonAPI.Utils
{
    /// <summary>
    /// A StringEnumConverter that does not fail the whole response when Amazon returns an
    /// enum value this SDK version does not know yet (e.g. Finances v2024-06-19 shipped
    /// SAFET_CLAIM_ID before it was documented). Unknown values map to null for nullable
    /// enum members instead of throwing JsonSerializationException.
    /// </summary>
    public class SafeStringEnumConverter : StringEnumConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch (JsonSerializationException)
            {
                if (Nullable.GetUnderlyingType(objectType) != null)
                    return null;
                return Activator.CreateInstance(objectType);
            }
        }
    }
}
