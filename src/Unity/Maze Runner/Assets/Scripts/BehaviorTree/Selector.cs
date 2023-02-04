using System.Collections.Generic;

namespace BehaviorTree
{
    public class Selector: Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override Status Evaluate()
        {
            foreach (var child in children)
            {
                var childStatus = child.Evaluate();
                if (childStatus == Status.Failure)
                {
                    continue;
                }
                if (childStatus == Status.Success)
                {
                    return Status.Success;
                }
                return Status.Running;
                
            }
            return Status.Failure;
        }
    }
}