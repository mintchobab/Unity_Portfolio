using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node root = null;

        private bool canEvaluate = true;

        protected virtual void Start()
        {
            root = SetUpTree();
        }

        private void Update()
        {
            if (!canEvaluate)
                return;

            if (root != null)
                root.Evaluate();
        }


        public void StartEvaluate()
        {
            canEvaluate = true;
        }

        public void StopEvaluate()
        {
            canEvaluate = false;
        }


        protected abstract Node SetUpTree();
    }
}
