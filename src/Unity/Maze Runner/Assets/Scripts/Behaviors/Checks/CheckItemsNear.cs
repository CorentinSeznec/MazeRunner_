using BehaviorTree;

class CheckItemsNear : BehaviorTree.Node
{
    APIPlayer api;
    float distance;
    public CheckItemsNear(APIPlayer api, float distance = 12)
    {
        this.api = api;
        this.distance = distance;
    }
    public override Status Evaluate()
    {
        var items = api.GetItemList(distance);
        foreach (var item in items)
        {
            if (api.ComputeDistance(item, api.GetMe()) < distance)
            {
                SetData("target", item);
                return Status.Success;
            }
        }
        return Status.Failure;
    }
}