using System;
namespace _GameAssets._Scripts
{
    public interface IStorageService
    {
        void Save(string key, object data, Action<bool> callback = null);
        void Load<T>(string key, Action<T> callback);
    }

}
