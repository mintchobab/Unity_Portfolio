using System;
using UnityEngine;

namespace lsy
{
    public class Utility : MonoBehaviour
    {
        public static T StringToEnum<T>(string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }
    }
}
