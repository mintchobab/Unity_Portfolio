using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy 
{
    public class Pool
    {
        private Stack<Poolable> poolableStack = new Stack<Poolable>();

        private Transform root;
        private GameObject myPrefab;


        public void Init(Transform parent, GameObject prefab, int count)
        {
            root = new GameObject($"{prefab.name}_root").transform;
            root.parent = parent;

            Poolable poolable = prefab.GetOrAddComponent<Poolable>();

            myPrefab = poolable.gameObject;

            for (int i = 0; i < count; i++)
            {
                Create(myPrefab);
            }
        }


        public void Create(GameObject prefab)
        {
            GameObject poolable = Object.Instantiate(prefab, root);
            poolable.name = prefab.name;
            poolable.SetActive(false);

            poolableStack.Push(poolable.GetOrAddComponent<Poolable>());
        }


        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            // 부모 변경
            poolable.gameObject.SetActive(false);

            poolableStack.Push(poolable);
        }


        // 없으면 생성해서 넣기
        public Poolable Pop()
        {
            if (poolableStack.Count == 0)
            {
                Create(myPrefab);
            }

            Poolable poolable = poolableStack.Pop();
            poolable.gameObject.SetActive(true);

            return poolable;
        }        
    }

}
