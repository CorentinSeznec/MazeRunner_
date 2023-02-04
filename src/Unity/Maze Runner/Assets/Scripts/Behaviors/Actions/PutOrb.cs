using BehaviorTree;

class PutOrb : BehaviorTree.Node
{
    APIPlayer api;
    public PutOrb(APIPlayer api)
    {
        this.api = api;
    }
    public override Status Evaluate()
    {
        var pedestals = api.GetPedestalsList(5);
        var pedestal = pedestals[0];
        api.GoTo(pedestal);
        api.InteractWith(pedestal);
        return Status.Running;
    }
}