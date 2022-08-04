using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace lsy
{
    public class PoolManager : IManager
    {
        private Dictionary<string, Pool> poolDic = new Dictionary<string, Pool>();

        private Transform root;

        private readonly int prefabCount = 10;


        public void Init()
        {
            root = new GameObject("Pool").transform;

            GameObject[] poolObjs = Managers.Instance.ResourceManager.LoadAll<GameObject>(ResourcePath.PoolPath);

            for (int i = 0; i < poolObjs.Length; i++)
            {
                CreatePool(root, poolObjs[i], prefabCount);
            }            
        }


        // 풀을 몇개 생성할지 정하기
        public void CreatePool(Transform parent, GameObject prefab, int count)
        {
            Pool pool = new Pool();
            pool.Init(parent, prefab, count);

            poolDic.Add(prefab.name, pool);
        }


        public string GetOriginPrefabName(string path)
        {
            string name = Managers.Instance.ResourceManager.Load<GameObject>(path).name;

            if (poolDic.ContainsKey(name))
            {
                return name;
            }

            return null;
        }



        public void Push(GameObject obj)
        {
            Poolable poolable = obj.GetComponent<Poolable>();

            if (poolable)
            {
                string name = obj.name;

                if (poolDic.ContainsKey(name))
                    poolDic[name].Push(poolable);
                else
                    Debug.LogError("Not Found NameKey");
            }
        }


        public Poolable Pop(string name)
        {
            if (poolDic.ContainsKey(name))
            {
                return poolDic[name].Pop();
            }

            return null;
        }
    }
}
