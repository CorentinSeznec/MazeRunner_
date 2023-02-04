using BehaviorTree;
using UnityEngine;

class CheckOptionalWallNear : BehaviorTree.Node
{
    APIPlayer api;
    float distance;
    public CheckOptionalWallNear(APIPlayer api, float distance = 10)
    {
        this.api = api;
        this.distance = distance;
    }
    public override Status Evaluate()
    {
        var walls = api.GetOptionalWallList(distance);
        foreach (var wall in walls)
        {
            var type = api.GetOptionalWallType(wall);
            if (type == OptionalWallType.Deactivate && api.ComputeDistance(wall, api.GetMe()) < distance)
            {
                SetData("target", wall);
                return Status.Success;
            }
        }
        return Status.Failure;
    }
}