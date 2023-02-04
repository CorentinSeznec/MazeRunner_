using System.Collections.Generic;


namespace BehaviorTree
{
    public class Sequence : Node
    {
        public Sequence() : base()
        {
        }

        public Sequence(List<Node> children) : base(children)
        {
        }

        public override Status Evaluate()
        {
            foreach (var child in children)
            {
                var childStatus = child.Evaluate();
                if (childStatus == Status.Failure)
                {
                    return Status.Failure;
                }
                if (childStatus == Status.Success)
                {
                    continue;
                }
                return Status.Running;
            }

            return Status.Success;
        }
    }
}