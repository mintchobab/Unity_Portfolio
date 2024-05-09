using UnityEngine;
using UnityEngine.Events;

namespace lsy
{
    public static class Extensions
    {
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            T component = obj.GetComponent<T>();

            if (component == null)
            {
                obj.AddComponent<T>();
            }

            return component;
        }

        public static void SetListener(this UnityEvent unityEvent, UnityAction unityAction)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(unityAction);
        }

        public static void SetListener<T0>(this UnityEvent<T0> unityEvent, UnityAction<T0> unityAction)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(unityAction);
        }

        public static void SetListener<T0, T1>(this UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> unityAction)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(unityAction);
        }
    }
}
