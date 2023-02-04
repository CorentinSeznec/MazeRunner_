using System;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;
using System.Collections.Generic;

class ChasePlayer : BehaviorTree.Node
{
    APIMino api;
    public ChasePlayer(APIMino api)
    {
        this.api = api;
    }
    
    public override Status Evaluate()
    {
        // define target

        var target = (GameObject)GetData("target");
        
        if (api.ComputeDistance(target, api.GetMe()) < 8f)
        { 
            return Status.Success;
        }


        api.GoTo(target);
        return Status.Running;
    }
}