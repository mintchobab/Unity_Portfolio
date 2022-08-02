using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace lsy
{
    public class PoolManager : IManager
    {
        private Dictionary<string, Pool> poolDic = new Dictionary<string, Pool>();

        private readonly string poolPath = "Assets/Resources/Load/Prefab/Pool";
        private readonly int prefabCount = 10;

        private Transform root;



        public void Init()
        {
            root = new GameObject("Pool").transform;

            // ������Ʈ�信�� Ǯ�� ������Ʈ �� �����ϱ�    
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { poolPath });

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                CreatePool(root, prefab, prefabCount);
            }
        }


        // Ǯ�� � �������� ���ϱ�
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
