using BehaviorTree;

class CheckPortalNear : BehaviorTree.Node
{
    APIPlayer api;
    float distance;
    public CheckPortalNear(APIPlayer api, float distance = 10)
    {
        this.api = api;
        this.distance = distance;
    }
    public override Status Evaluate()
    {
        var portal = api.GetPortal(distance);
        if (portal != null)
        {
            SetData("target", portal);
            return Status.Success;
        }
        return Status.Failure;
    }
}