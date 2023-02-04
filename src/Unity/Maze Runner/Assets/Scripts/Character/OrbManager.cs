using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    protected bool hasOrb = false;
    protected ItemDatabase itemDatabase;
    protected Item orb;

    protected Labyrinth maze;

    protected GameManager gameManager;

    protected Outline outline;

    void Start()
    {
        itemDatabase = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        orb = itemDatabase.GetItem("Orb");
        maze = GameObject.Find("environmentHolder").transform.Find("Labyrinth").GetComponent<Labyrinth>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        outline = GetComponent<Outline>();
    }

    public bool canTakeOrb()
    {
        return !hasOrb;
    }

    virtual public void takeOrb()
    {   
        if(!hasOrb){
            hasOrb = true;
            outline.OutlineWidth = 10f;
        }
    } 

    virtual public void ThrowOrb()
    {
        if(hasOrb){
            if(maze.SpawnRandomOrb())
            {
                hasOrb = false;
                outline.OutlineWidth = 0f;
                gameManager.IThrewTheOrb(gameObject);
            }
        }
    }

    virtual public void DestroyOrb()
    {
        if(hasOrb){
            hasOrb = false; 
            outline.OutlineWidth = 0f;
        }
    }

    public bool HasOrb()
    {
        return hasOrb;
    }   
}
