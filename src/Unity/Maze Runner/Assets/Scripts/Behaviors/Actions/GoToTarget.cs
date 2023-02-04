using UnityEngine;
using BehaviorTree;

class GoToTarget : BehaviorTree.Node
{
    APIPlayer api;
    public GoToTarget(APIPlayer api)
    {
        this.api = api;
    }
    public override Status Evaluate()
    {
        var target = (GameObject)GetData("target");
        if (api.GetMyTile().Equals(api.CoordToTile(target.transform)))
        {
            return Status.Success;
        }
        api.GoTo(target);
        return Status.Running;
    }
}