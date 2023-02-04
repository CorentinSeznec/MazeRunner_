using UnityEngine;
using BehaviorTree;

class PlaceOptionalWall : BehaviorTree.Node
{
    APIPlayer api;
    public PlaceOptionalWall(APIPlayer api)
    {
        this.api = api;
    }
    public override Status Evaluate()
    {
        var wall = (GameObject)GetData("target");
        api.InteractWith(wall);
        return Status.Running;
    }
}