using System;
using System.Collections.Generic;
using BehaviorTree;

class DepthFirstExploration : BehaviorTree.Node
{
    APIPlayer api;
    Stack<Tuple<int, int>> stack;
    List<Tuple<int, int>> visited;
    private Tuple<int, int> target;
    private Random rnd;
    public DepthFirstExploration(APIPlayer api)
    {
        this.api = api;
        stack = new Stack<Tuple<int, int>>();
        visited = new List<Tuple<int, int>>();
        target = null;
        rnd = new Random();
    }

    private void AcknowledgeCenter(Tuple<int, int> me)
    {
        var doors = new List<Tuple<int, int>>();
        var queue = new Queue<Tuple<int, int>>();
        queue.Enqueue(me);
        while (queue.Count != 0 && doors.Count < 4)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current)) 
                continue;
            var x = current.Item1;
            var y = current.Item2;
            visited.Add(current);
            var neighbors = new List<Tuple<int, int>>()
            {
                new(x + 1, y),
                new(x - 1, y),
                new(x, y + 1),
                new(x, y - 1),
            };
            foreach (var neighbor in neighbors)
            {
                var type = api.WhatIs(neighbor.Item1, neighbor.Item2);
                if (type == structType.Center && !visited.Contains(neighbor) && !queue.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                }
                else if (type == structType.Ground && !visited.Contains(neighbor))
                {
                    doors.Add(neighbor);
                    if (doors.Count == 4)
                    {
                        break;
                    }
                }
            }
        }
        doors.Sort((a, b) => rnd.Next(-1, 1));
        foreach (var door in doors)
        {
            stack.Push(door);
        }
    }

    public override Status Evaluate()
    {
        var me = api.GetMyTile();
        if (target == null)
        {
            AcknowledgeCenter(me);
            target = me;
        }
        if (!me.Equals(target))
        {
            api.GoTo(target.Item1, target.Item2);
            return Status.Running;
        }
        
        if (!visited.Contains(me))
        {
            visited.Add(me);
            var x = me.Item1;
            var y = me.Item2;
            var neighbors = new List<Tuple<int, int>>()
            {
                new(x + 1, y),
                new(x - 1, y),
                new(x, y + 1),
                new(x, y - 1),
            };
            
            neighbors.Sort((a, b) => rnd.Next(-1, 1));
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