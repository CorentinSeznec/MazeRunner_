using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManagerMainPlayer : OrbManager
{
    [SerializeField] GameObject hand;
    private HandManager hm;

    void Start()
    {
        hm = hand.GetComponent<HandManager>();
        itemDatabase = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        orb = itemDatabase.GetItem("Orb");
        maze = GameObject.Find("environmentHolder").transform.Find("Labyrinth").GetComponent<Labyrinth>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        outline = GetComponent<Outline>();
    }

    public override void takeOrb()
    {   
        if(!hasOrb){
            hm.GetInHand(orb);
        }
        base.takeOrb();
    } 

    public override void ThrowOrb()
    {
        if(hasOrb){
            if(maze.SpawnRandomOrb())
            {
                hasOrb = false;
                hm.GetInHand(null);
                gameManager.IThrewTheOrb(gameObject);
            }
        }
    }

    public override void DestroyOrb()
    {
        if(hasOrb){
            hm.GetInHand(null);
        }
        base.DestroyOrb();
    }
}
