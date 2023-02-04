using BehaviorTree;
using UnityEngine;

class CheckCanPlaceWall : BehaviorTree.Node
{
    APIPlayer api;
    public CheckCanPlaceWall(APIPlayer api)
    {
        this.api = api;
    }
    public override Status Evaluate()
    {
        if (api.OnCooldownWall())
        {
            return Status.Failure;
        }
        return Status.Success;
    }
}