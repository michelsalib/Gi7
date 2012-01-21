﻿using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace Gi7.Service
{
    /// <summary>
    /// Provide simple access to store/retrieve typed data from the isolated file storage
    /// These data as considered as cache
    /// </summary>
    public class CacheProvider
    {
        private readonly string _path;

        public CacheProvider(String key)
        {
            _path = String.Format("cache/{0}/", key).TrimEnd('/');

            if (!IsolatedStorageFile.GetUserStoreForApplication().DirectoryExists(_path))
                IsolatedStorageFile.GetUserStoreForApplication().CreateDirectory(_path);
        }

        public void Save(String key, object item)
        {
            string file = CleanFilePath(key);
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                using (IsolatedStorageFileStream stream = store.OpenFile(_path + file, FileMode.Create))
                {
                    var serializer = new XmlSerializer(item.GetType());
                    serializer.Serialize(stream, item);
                }
        }

        public T Get<T>(String key)
        {
            string file = String.Format("{0}/{1}", _path, CleanFilePath(key));
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store.FileExists(file))
                    {
                        using (IsolatedStorageFileStream stream = store.OpenFile(file, FileMode.Open))
                        {
                            var serializer = new XmlSerializer(typeof (T));
                            return (T) serializer.Deserialize(stream);
                        }
                    } else
                    {
                        return default(T);
                    }
                }
            } catch (Exception)
            {
                return default(T);
            }
        }

        public void Clear()
        {
            foreach (string file in IsolatedStorageFile.GetUserStoreForApplication().GetFileNames(_path + "*"))
            {
                IsolatedStorageFile.GetUserStoreForApplication().DeleteFile(_path + file);
            }

            try
            {
                if (IsolatedStorageFile.GetUserStoreForApplication().DirectoryExists(_path))
                    IsolatedStorageFile.GetUserStoreForApplication().DeleteDirectory(_path);
            } catch (Exception)
            {
            }
        }

        private String CleanFilePath(String key)
        {
            return key.Replace('/', '_').Replace('?', '_').Replace('=', '_').Replace('&', '_');
        }
    }
}