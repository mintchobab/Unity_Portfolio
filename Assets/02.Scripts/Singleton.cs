using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
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
                    Debug.Log("싱글톤 없음");
                }
                else
                {
                    instance = t;
                }
            }

            return instance;
        }
    }
}
