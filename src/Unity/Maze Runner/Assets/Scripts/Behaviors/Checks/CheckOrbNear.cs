using BehaviorTree;

class CheckOrbNear : BehaviorTree.Node
{
    APIPlayer api;
    float distance;
    public CheckOrbNear(APIPlayer api, float distance = 25)
    {
        this.api = api;
        this.distance = distance;
    }
    public override Status Evaluate()
    {
        var orbs = api.GetOrbList(distance);
        foreach (var orb in orbs)
        {
            if (api.ComputeDistance(orb, api.GetMe()) < distance)
            {
                SetData("target", orb);
                return Status.Success;
            }
        }
        return Status.Failure;
    }
}