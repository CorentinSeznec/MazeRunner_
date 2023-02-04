using BehaviorTree;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

class SetOrbTargetNearest : BehaviorTree.Node
{
    APIMino api;
    private List<GameObject> OrbPlayers = new List<GameObject>();
    private List<float> DistanceList = new List<float>();
    private float dist;
    
    public SetOrbTargetNearest(APIMino api)
    {
        this.api = api;
    }
    public override Status Evaluate()
    {
        // SET la TARGET
        OrbPlayers = api.WhoTakeOrb();
        GameObject playerToReturn = OrbPlayers[0];
        int NbOrbHolder = 0;
        float distanceMin = float.MaxValue;

        if (OrbPlayers.Count >0)
        {
            
            foreach (GameObject player in OrbPlayers)
            {
                Transform pos_t = player.transform;
                int pos_x = api.CoordToTile(pos_t).Item1;
                int pos_y = api.CoordToTile(pos_t).Item2;
                bool IsCenterTile = api.IsCenterTile(pos_x, pos_y);

                if (!IsCenterTile)
                {
                    NbOrbHolder +=1;
                    dist = api.ComputeDistance(player, api.GetMe()); 
                    if (dist < distanceMin)
                    {
                        playerToReturn = player;
                        distanceMin = dist;
                    }
                }
                
            }
            
            if (NbOrbHolder == 0)
            {
                return Status.Failure;
            }

            SetData("target", playerToReturn);


            return Status.Success;

        }

        
        return Status.Failure;
    }
}