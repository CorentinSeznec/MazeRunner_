using UnityEngine;
using BehaviorTree;

class PickUpOrb : BehaviorTree.Node
{
    APIPlayer api;
    public PickUpOrb(APIPlayer api)
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