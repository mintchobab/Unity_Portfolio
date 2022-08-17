using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T t = FindObjectOfType<T>();
                if (t == null)
                {
                    GameObject obj = new GameObject();
                    T newT = obj.AddComponent<T>();

                    instance = newT;
                }
                else
                {
                    instance = t;
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        Init();
    }

    public abstract void Init();
}
