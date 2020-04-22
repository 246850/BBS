using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BBS.Framework.Extensions
{
    public static class ObjectMapperExtensions
    {
        public static string AsJson(this object source)
        {
            return JsonSerializer.Serialize(source);
        }

        public static T ToObject<T>(this string source) where T:class
        {
            return JsonSerializer.Deserialize<T>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TSource:class
            where TDestination : class
        {
            if (source == null) return default(TDestination);
            string json = source.AsJson();
            return ToObject<TDestination>(json);
        }
    }
}
