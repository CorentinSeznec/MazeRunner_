using BehaviorTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class CheckOrbTaken : BehaviorTree.Node
{
    APIMino api;
    public CheckOrbTaken(APIMino api)
    {
        this.api = api;
        
    }
    public override Status Evaluate()
    {
        if (api.OrbTaken())
        {

            return Status.Success;
        }

        return Status.Failure;
    }
}