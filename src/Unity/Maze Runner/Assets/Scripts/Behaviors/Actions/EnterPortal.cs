using UnityEngine;
using BehaviorTree;

class EnterPortal : BehaviorTree.Node
{
    APIPlayer api;
    public EnterPortal(APIPlayer api)
    {
        this.api = api;
    }
    public override Status Evaluate()
    {
        var orb = (GameObject)GetData("target");
        api.InteractWith(orb);
        return Status.Running;
    }
}