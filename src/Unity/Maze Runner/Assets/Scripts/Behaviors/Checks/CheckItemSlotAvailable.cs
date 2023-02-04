using BehaviorTree;
using UnityEngine;

class CheckItemSlotAvailable : BehaviorTree.Node
{
    APIPlayer api;
    public CheckItemSlotAvailable(APIPlayer api)
    {
        this.api = api;
    }
    public override Status Evaluate()
    {
        foreach (var item in api.OwnItems())
        {
            if (item == null)
            {
                return Status.Success;
            }
        }
        return Status.Failure;
    }
}