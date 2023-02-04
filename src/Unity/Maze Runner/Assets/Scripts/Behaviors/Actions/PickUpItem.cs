using UnityEngine;
using BehaviorTree;

class PickUpItem : BehaviorTree.Node
{
    APIPlayer api;
    public PickUpItem(APIPlayer api)
    {
        this.api = api;
    }
    public override Status Evaluate()
    {
        var item = (GameObject)GetData("target");
        api.InteractWith(item);
        return Status.Running;
    }
}