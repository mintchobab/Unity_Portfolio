using UnityEngine;

public class ResourceManager : IManager
{
    public void Init() { }


    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
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
