using UnityEngine;

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
    }
}
