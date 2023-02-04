using BehaviorTree;
using UnityEngine;
using System.Collections.Generic;

class SetTargetNearest : BehaviorTree.Node
{
    APIMino api;
    private List<GameObject> AllPlayers = new List<GameObject>();
    private List<float> DistanceList = new List<float>();
    private float dist;
    
    public SetTargetNearest(APIMino api)
    {
        this.api = api;
        SetData("idx", 0);
    }
    public override Status Evaluate()
    {
        // SET la TARGET
        
        AllPlayers = api.GetPlayers();
        GameObject playerToReturn = AllPlayers[0];
        float distanceMin = float.MaxValue;

        if (AllPlayers.Count >0)
        {
            int i =-1;
            int final_i = -1;
            
            foreach (GameObject player in AllPlayers)
            {
                i +=1;
                
                Transform pos_t = player.transform;
                int pos_x = api.CoordToTile(pos_t).Item1;
                int pos_y = api.CoordToTile(pos_t).Item2;
                bool IsCenterTiles = api.IsCenterTile(pos_x, pos_y);
                


                if (!IsCenterTiles)
                {
                    
                    
                    dist = api.ComputeDistance(player, api.GetMe());
                    if (dist < distanceMin)
                    {
                        playerToReturn = player;
                        distanceMin = dist;
                        final_i = i;
                    }
                }
                    
            }

            int old_i = (int)GetData("idx");
            float _dist = api.ComputeDistance(AllPlayers[old_i], api.GetMe());
            if (_dist == float.MaxValue)
            {
                playerToReturn = AllPlayers[old_i];
                final_i = old_i;
            }

            SetData("idx", final_i);

            SetData("target", playerToReturn);


            



            return Status.Success;

        }

        return Status.Failure;
    }
}