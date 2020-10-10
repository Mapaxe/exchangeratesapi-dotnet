using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ExchangeRates.Utils
{
    internal class CustomDeserializationContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.Writable) return property;
            var prop = member as PropertyInfo;
            if (prop == null) return property;
            var hasPrivateSetter = prop.GetSetMethod(true) != null;
            property.Writable = hasPrivateSetter;

            return property;
        }
    }
}