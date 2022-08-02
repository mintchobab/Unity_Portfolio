using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : IManager
{
    public void Init() { }

    private Dictionary<string, Object> resourceCache = new Dictionary<string, Object>();


    public T Load<T>(string path) where T : Object
    {
        if (!resourceCache.ContainsKey(path))
            resourceCache[path] = Resources.Load<T>(path);

        return (T)resourceCache[path];
    }


    public T Instantiate<T>(string path, Transform parent = null) where T : Object
    {
        T prefab = Load<T>(path);

        if (prefab == null)
        {
            Debug.Log($"Failed to load : {path}");
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }

}
