using BehaviorTree;
using UnityEngine;

class SleepUntil : BehaviorTree.Node
{
    APIPlayer api;
    public SleepUntil(APIPlayer api)
    {
        this.api = api;
    }
    public override Status Evaluate()
    {
        var sleep = GetData("sleep_until");
        if (sleep == null)
            return Status.Failure;
        var sleepUntil = (float)sleep;
        if (Time.time < sleepUntil)
        {
            api.GoTo(api.GetMe());
            return Status.Running;
        }
        ClearData("sleep_until");
        return Status.Failure;
    }
}