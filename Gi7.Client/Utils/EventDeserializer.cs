using Gi7.Client.Model;
using Gi7.Client.Model.Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gi7.Client.Utils
{
    public class EventDeserializer : IDeserializer
    {
        #region IDeserializer Members

        public string DateFormat { get; set; }

        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            var result = Activator.CreateInstance<T>();
            var events = result as List<Event>;

            if (events == null)
                throw new InvalidOperationException("The type must be List<Event>.");

            var json = JArray.Parse(response.Content);
            serializer = new JsonSerializer();

            var deserializeMethod = serializer.GetType().GetMethods().First(m => m.IsGenericMethod && m.Name == "Deserialize");
            var ns = typeof(Event).Namespace;
            var userType = typeof(User);
            var repositoryType = typeof(Repository);
            var userDeserializer = deserializeMethod.MakeGenericMethod(userType);
            var repoDeserializer = deserializeMethod.MakeGenericMethod(repositoryType);

            foreach (var eventData in json)
            {
                try
                {
                    var payload = eventData.SelectToken("payload");

                    var eventDeserializer = deserializeMethod.MakeGenericMethod(Type.GetType(string.Format("{0}.{1}", ns, eventData["type"].Value<String>())));
                    var e = (Event)eventDeserializer.Invoke(serializer, new object[] { new JTokenReader(payload) });

                    e.Actor = (User)userDeserializer.Invoke(serializer, new object[] { new JTokenReader(eventData["actor"]) });
                    e.Public = eventData["public"].Value<bool?>();
                    e.CreatedAt = eventData["created_at"].Value<DateTime>();
                    e.Repo = (Repository)repoDeserializer.Invoke(serializer, new object[] { new JTokenReader(eventData["repo"]) });

                    events.Add(e);
                }
                catch (Exception)
                {
                    // might silencly fail on an event
                }
            }

            return result;
        }

        #endregion

        private JsonSerializer serializer;
    }
}