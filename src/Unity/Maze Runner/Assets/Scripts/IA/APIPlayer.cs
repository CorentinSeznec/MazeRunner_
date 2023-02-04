using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class APIPlayer : MonoBehaviour
{
    [SerializeField] private GameObject goalPrefab;
    
    private Inventory inventory;
    private OrbManager orbManager;
    private MoveTo move;
    private IntObjectDetector intDetector;

    private GameManager gameManager;
    private Labyrinth labyrinth;
    private ItemGenerator itemGen;
    private WallCooldown wallCooldown;

    private Transform goal;

    private NavMeshAgent agent;

    void Awake()
    {
        inventory = gameObject.GetComponent<Inventory>();
        orbManager = gameObject.GetComponent<OrbManager>();
        move = gameObject.GetComponent<MoveTo>();
        intDetector = gameObject.GetComponent<IntObjectDetector>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        labyrinth = GameObject.Find("environmentHolder").transform.GetChild(0).GetComponent<Labyrinth>();
        itemGen = GameObject.Find("ItemGenerator").GetComponent<ItemGenerator>();
        wallCooldown = gameObject.GetComponent<WallCooldown>();

        goal = Instantiate(goalPrefab, transform.position, transform.rotation).transform;
        goal.transform.parent = gameManager.transform;
        
        move.goals[0] = goal;

        agent = GetComponent<NavMeshAgent>();
    }

    // UTILS
    Vector3 TileToCoord(int x, int y)
    {
        return labyrinth.TileToCoord(x, y);
    }

    bool IsValidTile(int x, int y)
    {
        return labyrinth.IsValidTile(x, y);
    }

    public Tuple<int,int> CoordToTile(Transform t)
    {
        return labyrinth.CoordToTile(t);
    }
    
    public Tuple<int,int> CoordToTile(Vector3 v)
    {
        return labyrinth.CoordToTile(v);
    }

    public GameObject GetObject(int x, int y)
    {
        return labyrinth.GetObjectIn(x ,y);
    }

    public float GetInteractionRadius()
    {
        return intDetector.GetInteractionRadius();
    }

    public float ComputeDistance(Vector3 initialPos, Vector3 finalPos)
    {
        NavMeshPath Path = new NavMeshPath();
        if (NavMesh.CalculatePath(initialPos, finalPos, agent.areaMask, Path))
        {
            float distance = Vector3.Distance(initialPos, Path.corners[0]);

            for (int j =1; j< Path.corners.Length; j++)
            {
                distance += Vector3.Distance(Path.corners[j-1], Path.corners[j]);
            }
            return distance;
        }
        return 0;
    }

    public float ComputeDistance(GameObject initialObject, GameObject finalObject)
    {
        Vector3 initialPos = initialObject.transform.position;
        Vector3 finalPos = finalObject.transform.position;
        return ComputeDistance(initialPos, finalPos);
    }

    // MOVEMENT
    public bool GoTo(int x, int y)
    {
        if(IsValidTile(x, y))
        {
            Vector3 destination = TileToCoord(x, y);
            destination = new Vector3(destination.x, 1, destination.z);
            goal.position =  destination;
            return true;
        }
        return false;
    }

    public bool GoTo(GameObject given_goal)
    {
        Vector3 destination = given_goal.transform.position;
        destination = new Vector3(destination.x, 1, destination.z);
        goal.position = destination;
        return true;
    }

    public GameObject GetMe()
    {
        return gameObject;
    }

    public Vector3 GetGoalCoord()
    {
        return goal.position;
    }

    public Vector3 GetMyCoord()
    {
        return GetMe().transform.position;
    }

    public Tuple<int,int> GetMyTile()
    {
        return CoordToTile(GetMe().transform);
    }

    // USE ITEMS
    public bool UseItem(GameObject itemObject)
    {
        return inventory.UseItem(itemObject);
    }

    public bool UseItem(Item item)
    {
        return inventory.UseItem(item);
    }

    public bool UseItem(int index)
    {
        return inventory.UseItem(index);
    }

    // INFORMATION ON INVENTORY 

    public List<Item> OwnItems()
    {
        return inventory.OwnItems();
    }

    public bool HaveOrb()
    {
        return orbManager.HasOrb();
    }

    // KNOW ITS ENVIRONNMENT

    public Tuple<int, int> GetCenterTile()
    {
        return labyrinth.GetCenterTile();
    }

    public structType WhatIs(int x, int y)
    {
        return labyrinth.WhatIs(x, y);
    }

    public OptionalWallType GetOptionalWallType(GameObject wall)
    {
        ColliderBox box = wall.GetComponent<ColliderBox>();
        if(box == null)
        {
            return OptionalWallType.None;
        }
        return box.GetWallType();

    }

    public OptionalWallType GetOptionalWallType(int x, int y)
    {
        GameObject wall = labyrinth.GetObjectIn(x ,y);
        return GetOptionalWallType(wall);
    }

    public bool OnCooldownWall()
    {
        return wallCooldown.IsOnCoolDown();
    }

    public List<GameObject> GetSpawnedObjects()
    {
        return itemGen.GetSpawnedItems();
    } 

    public Vector3 GetMinotaurCoord()
    {
        return gameManager.GetMinotaur().transform.position;
    }

    public Tuple<int, int> GetMinotaurTile()
    {
        return CoordToTile(gameManager.GetMinotaur().transform);
    }

    public List<GameObject> GetAllies()
    {
        List<GameObject> ret = gameManager.GetAllies();
        int index = ret.FindIndex(obj => obj == gameObject);
        if(index != -1)
        {
            ret.RemoveAt(index);
        }
        return ret;
    }

    public List<GameObject> GetEnemies()
    {   
        List<GameObject> ret = gameManager.GetEnemies();
        int index = ret.FindIndex(obj => obj == gameObject);
        if(index != -1)
        {
            ret.RemoveAt(index);
        }
        return ret;
    }




    public List<GameObject> GetItemList(float radius)
    {
        return intDetector.GetItemList(radius);
    }

    public List<GameObject> GetOptionalWallList(float radius)
    {
        return intDetector.GetOptionalWallList(radius);
    }

    public List<GameObject> GetOrbList(float radius)
    {
        return intDetector.GetOrbList(radius);
    }

    public List<GameObject> GetPedestalsList(float radius)
    {
        return intDetector.GetPedestalsList(radius);
    }

    public GameObject GetPortal(float radius)
    {
        return intDetector.GetPortal(radius);
    }

    public GameObject GetOrbHolder()
    {   
        return gameManager.GetOrbHolder();
    }

    // INTERACT WITH OBJECTS
    public bool InteractWith(GameObject obj)
    {
        return intDetector.InteractWith(obj);
    }
}
