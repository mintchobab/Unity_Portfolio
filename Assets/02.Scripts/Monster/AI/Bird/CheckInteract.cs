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
            // �÷��̾�� ���ͷ�Ʈ �Ǵ��� Ȯ��
            state = NodeState.Failure;
            return state;
        }
    }
}
