using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using RestSharp.Deserializers;
using System;

namespace Gi7.Client.Utils
{
    public class UserDeserializer : IDeserializer
    {

        public T Deserialize<T>(IRestResponse response)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new IsoDateTimeConverter());

            var deserialize = serializer.Deserialize<T>(new JsonTextReader(new StringReader(response.Content)));
            return deserialize;
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
    }
}