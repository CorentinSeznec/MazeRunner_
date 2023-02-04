using System.Collections.Generic;
using BehaviorTree;
using System;

public class MinoBehaviour : Tree
{
    private APIMino _api; 
    protected override BehaviorTree.Node CreateTree()
    {
        _api = GetComponent<APIMino>();
        BehaviorTree.Node root = new Selector(new List<BehaviorTree.Node>
        {
            // if player has an orb
            new Sequence(new List<BehaviorTree.Node>
            {
                new CheckOrbTaken(_api),
                new SetOrbTargetNearest(_api),
                new ChasePlayer(_api),
                //new HitPlayer(_api),
            }),

            new Sequence(new List<BehaviorTree.Node>
            {
                new SetTargetNearest(_api),
                new ChasePlayer(_api),
                //new HitPlayer(_api),
                // faire ca, le prblm vient du fait que personne ne ramasse l'orbe
                // chase nearest player

            }),

        });
        return root;
    }
}