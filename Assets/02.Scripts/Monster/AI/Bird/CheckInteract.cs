using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

namespace lsy
{
    public class CheckInteract : Node
    {
        private BirdBT bird;

        public CheckInteract(BirdBT bird)
        {
            this.bird = bird;
        }

        public override NodeState Evaluate()
        {
            // 플레이어에게 인터렉트 되는지 확인
            state = NodeState.Failure;
            return state;
        }
    }
}
