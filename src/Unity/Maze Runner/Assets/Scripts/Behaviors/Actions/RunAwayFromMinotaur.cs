using System;
using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

class RunAwayFromMinotaur : BehaviorTree.Node
{
    APIPlayer api;
    public RunAwayFromMinotaur(APIPlayer api)
    {
        this.api = api;
    }
    public override Status Evaluate()
    {
        var me = api.GetMyTile();
        var x = me.Item1;
        var y = me.Item2;
        var neighbors = new List<Tuple<int, int>>()
        {
            new(x + 1, y),
            new(x - 1, y),
            new(x, y + 1),
            new(x, y - 1),
        };
        var minotaur = api.GetMinotaurTile();
        neighbors.Sort(delegate(Tuple<int, int> a, Tuple<int, int> b)
        {
            var aDist = Math.Sqrt(Math.Pow(a.Item1 - minotaur.Item1, 2) + Math.Pow(a.Item2 - minotaur.Item2, 2));
            var bDist = Math.Sqrt(Math.Pow(b.Item1 - minotaur.Item1, 2) + Math.Pow(b.Item2 - minotaur.Item2, 2));
            return -aDist.CompareTo(bDist);
        });
        foreach (var neighbor in neighbors)
        {
            var type = api.WhatIs(neighbor.Item1, neighbor.Item2);
            if (type is structType.Ground or structType.OptionalWall or structType.Center)
            {
                api.GoTo(neighbor.Item1, neighbor.Item2);
                return Status.Running;
            }
        }
        return Status.Failure;
    }
}