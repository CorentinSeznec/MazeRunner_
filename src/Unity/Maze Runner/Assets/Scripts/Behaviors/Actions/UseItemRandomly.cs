using BehaviorTree;
using UnityEngine;
using Random = System.Random;

class UseItemRandomly : BehaviorTree.Node
{
    APIPlayer api;
    private Random rnd;
    
    public UseItemRandomly(APIPlayer api)
    {
        this.api = api;
        rnd = new Random();
    }
    public override Status Evaluate()
    {
        foreach (var item in api.OwnItems())
        {
            if (item != null)
            {
                if (rnd.NextDouble() < Time.deltaTime / 10)
                {
                    api.UseItem(item);
                    return Status.Failure;
                }
            }
        }
        return Status.Failure;
    }
}