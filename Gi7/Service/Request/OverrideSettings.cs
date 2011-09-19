using System;
using RestSharp.Deserializers;

namespace Gi7.Service.Request
{
    public class OverrideSettings
    {
        public String BaseUri { get; set; }

        public IDeserializer Deserializer { get; set; }

        public String ContentType { get; set; }
    }
}
