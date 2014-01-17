using System;
using FlitBit.IoC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LicenseValidator.Core
{
    public class DefaultJsonConverter : JsonConverter
    {
        readonly IContainer _container;

        /// <summary>
        ///   Creates a DTO Converter
        /// </summary>
        public DefaultJsonConverter()
        {
            _container = Container.Current;
        }

        /// <summary>
        ///   We don't need to serialize, therefore, false
        /// </summary>
        public override bool CanWrite { get { return false; } }

        /// <summary>
        ///   Determines if we can convert the type.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return (objectType.IsAbstract || objectType.IsInterface) && _container.CanConstruct(objectType);
        }

        /// <summary>
        ///   Creates a new object via the Factory.  Loads the instance via the serializer.
        /// </summary>
        /// <param name="reader">The JsonReader</param>
        /// <param name="objectType">The Type of DTO</param>
        /// <param name="existingValue">The DTO</param>
        /// <param name="serializer">JsonSerializer</param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JsonReader objectReader = null;

            var instance = _container.CreateInstance(objectType);
            if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.StartArray)
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    var jObject = JObject.Load(reader);
                    objectReader = jObject.CreateReader();
                }

                if (reader.TokenType == JsonToken.StartArray)
                {
                    var jArray = JArray.Load(reader);
                    objectReader = jArray.CreateReader();
                }

                if (objectReader != null)
                    serializer.Populate(objectReader, instance);
            }

            return instance;
        }

        /// <summary>
        ///   Not Implemented
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}