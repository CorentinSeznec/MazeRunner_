using System;
using BehaviorTree;
using UnityEngine;

class CheckMinotaurNear : BehaviorTree.Node
{
    APIPlayer api;
    float distance;
    private float cooldown;
    public CheckMinotaurNear(APIPlayer api, float distance = 25)
    {
        this.api = api;
        this.distance = distance;
        cooldown = 0;

    }
    public override Status Evaluate()
    {
        if (Time.time < cooldown)
        {
            return Status.Success;
        }
        var delta = api.GetMinotaurCoord() - api.GetMyCoord();
        if (delta.magnitude > distance)
        {
            return Status.Failure;
        }

        if (api.ComputeDistance(api.GetMinotaurCoord(), api.GetMyCoord()) > distance)
        {
            return Status.Failure;
        }

        cooldown = Time.time + 10;
        return Status.Success;
    }
}