using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace Github7.Service
{
    public class CacheProvider
    {
        private string _path;

        public CacheProvider(String key)
        {
            _path = String.Format("cache/{0}/", key);

            if (!IsolatedStorageFile.GetUserStoreForApplication().DirectoryExists(_path.TrimEnd('/')))
                IsolatedStorageFile.GetUserStoreForApplication().CreateDirectory(_path.TrimEnd('/'));
        }

        public void Save(String key, object item)
        {
            var file = key.Replace('/', '_');
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = store.OpenFile(_path + file, FileMode.Create))
            {
                var serializer = new XmlSerializer(item.GetType());
                serializer.Serialize(stream, item);
            }
        }

        public T Get<T>(String key)
        {
            var file = key.Replace('/', '_');
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store.FileExists(_path + file))
                    {
                        using (var stream = store.OpenFile(_path + file, FileMode.Open))
                        {
                            var serializer = new XmlSerializer(typeof(T));
                            return (T)serializer.Deserialize(stream);
                        }
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        internal void Clear()
        {
            foreach (var file in IsolatedStorageFile.GetUserStoreForApplication().GetFileNames(_path + "*"))
            {
                IsolatedStorageFile.GetUserStoreForApplication().DeleteFile(_path + file);
            }
            IsolatedStorageFile.GetUserStoreForApplication().DeleteDirectory(_path.TrimEnd('/'));
        }
    }
}
