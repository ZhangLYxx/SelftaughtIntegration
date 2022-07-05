using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace Integration.ToolKits
{
    /// <summary>
    /// long 与 字符串的转化
    /// </summary>
    public class LongConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            // 只处理long和ulong两种类型的数据
            if (!IsNullableType(objectType, out var type))
            {
                type = objectType;
            }
            return type.FullName == "System.Int64" || type.FullName == "System.UInt64";
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string v = null;
            if (reader.Value != null)
            {
                v = reader.Value.ToString();
            }
            else
            {
                if (existingValue != null)
                {
                    switch (existingValue)
                    {
                        case long _:
                        case ulong _:
                            return existingValue;
                        default:
                            v = existingValue.ToString();
                            break;
                    }
                }
            }

            if (!IsNullableType(objectType, out var type) && string.IsNullOrEmpty(v))
            {
                type = objectType;
                if (string.IsNullOrEmpty(v))
                {
                    throw new JsonSerializationException($"无法将空字符串或Null转化为{type.FullName}");
                }
                return null;
            }
            if (type == typeof(long) && long.TryParse(v, out var l))
            {
                return l;
            }
            if (type == typeof(ulong) && ulong.TryParse(v, out var ul))
            {
                return ul;
            }
            throw new JsonSerializationException($"无法将{v}转化为{type.FullName}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                if (serializer.NullValueHandling == NullValueHandling.Include)
                {
                    writer.WriteValue("null");
                }
            }
            else
            {
                writer.WriteValue(value.ToString());
            }

        }

        private static bool IsNullableType(Type type, out Type outType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                outType = type.GetGenericArguments()[0];
                return true;
            }
            outType = type;
            return false;
        }
    }
}
