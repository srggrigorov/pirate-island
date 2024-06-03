using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace _GameAssets._Scripts
{
    class BinaryStorageService : IStorageService
    {
        public void Save(string key, object data, Action<bool> callback = null)
        {
            using var dataStream = new FileStream(BuildPath(key), FileMode.Create);
            var converter = new BinaryFormatter();
            converter.Serialize(dataStream, data);
            dataStream.Close();
            callback?.Invoke(true);
        }
        public void Load<T>(string key, Action<T> callback)
        {
            using var dataStream = new FileStream(BuildPath(key), FileMode.Open);
            var converter = new BinaryFormatter();
            var data = (T)converter.Deserialize(dataStream);
            dataStream.Close();
            callback?.Invoke(data);
        }

        private string BuildPath(string key)
        {
            return Path.Combine(Application.persistentDataPath, key);
        }
    }
}
