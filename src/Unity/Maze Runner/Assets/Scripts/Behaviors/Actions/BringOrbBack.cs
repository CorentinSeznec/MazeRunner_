using System;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

class BringOrbBack : BehaviorTree.Node
{
    APIPlayer api;
    Stack<Tuple<int, int>> stack;
    List<Tuple<int, int>> visited;
    private Tuple<int, int> target;
    public BringOrbBack(APIPlayer api)
    {
        this.api = api;
        stack = new Stack<Tuple<int, int>>();
        visited = new List<Tuple<int, int>>();
        target = null;

    }
    public override Status Evaluate()
    {
        var me = api.GetMyTile();
        if (target == null)
        {
            target = me;
        }
        if (!me.Equals(target))
        {
            api.GoTo(target.Item1, target.Item2);
            return Status.Running;
        }
        if (api.WhatIs(me.Item1, me.Item2) == structType.Center)
        {
            if (me.Equals(api.GetCenterTile()))
            {
                return Status.Success;
            }

            var orb = api.GetCenterTile();
            target = orb;
            api.GoTo(orb.Item1, orb.Item2);
            return Status.Running;
        }
        var x = me.Item1;
        var y = me.Item2;
        if (!visited.Contains(me))  
        {
            visited.Add(me);
            var neighbors = new List<Tuple<int, int>>()
            {
                new(x + 1, y),
                new(x - 1, y),
                new(x, y + 1),
                new(x, y - 1),
            };
            var center = api.GetCenterTile();
            neighbors.Sort(delegate(Tuple<int, int> a, Tuple<int, int> b)
            {
                var aDist = Math.Sqrt(Math.Pow(a.Item1 - center.Item1, 2) + Math.Pow(a.Item2 - center.Item2, 2));
                var bDist = Math.Sqrt(Math.Pow(b.Item1 - center.Item1, 2) + Math.Pow(b.Item2 - center.Item2, 2));
                return -aDist.CompareTo(bDist);
            });
            foreach (var neighbor in neighbors)
            {
                var type = api.WhatIs(neighbor.Item1, neighbor.Item2);
                if (type is structType.Ground or structType.OptionalWall or structType.Center && !visited.Contains(neighbor))
                {
                    stack.Push(neighbor);
                }
            }
        }
        target = stack.Pop();
        api.GoTo(target.Item1, target.Item2);
        return Status.Running;
    }
}