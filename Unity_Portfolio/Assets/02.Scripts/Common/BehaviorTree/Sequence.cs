using System.Collections.Generic;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }


        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        state = NodeState.Failure;
                        return state;

                    case NodeState.Success:
                        continue;

                    case NodeState.Running:
                        state = NodeState.Running;
                        return state;

                    default:
                        state = NodeState.Success;
                        return state;
                }
            }

            state = NodeState.Success;
            return state;
        }

    }
}
