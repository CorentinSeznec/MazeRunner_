using BehaviorTree;
using UnityEngine;

class CheckHaveOrb : BehaviorTree.Node
{
    APIPlayer api;
    public CheckHaveOrb(APIPlayer api)
    {
        this.api = api;
    }
    public override Status Evaluate()
    {
        if (api.HaveOrb())
        {
            return Status.Success;
        }
        return Status.Failure;
    }
}