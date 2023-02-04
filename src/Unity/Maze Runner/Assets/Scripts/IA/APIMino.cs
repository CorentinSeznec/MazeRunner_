using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class APIMino : MonoBehaviour
{

    private List<GameObject> allPlayers = null;
    private OrbManager orbManager;



    [SerializeField] private GameObject goalPrefab;
    
    private MoveToMino move;
    private IntObjectDetector intDetector;

    private GameManager gameManager;
    private Labyrinth labyrinth;
    private ItemGenerator itemGen;

    private Transform goal;

    private NavMeshAgent agent;

    void Awake()
    {
// CHECK !!!!!! SI BESOIN
        move = gameObject.GetComponent<MoveToMino>();
       
        intDetector = gameObject.GetComponent<IntObjectDetector>();

        goal = Instantiate(goalPrefab, transform.position, transform.rotation).transform;

        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        labyrinth = GameObject.Find("environmentHolder").transform.GetChild(0).GetComponent<Labyrinth>();
        itemGen = GameObject.Find("ItemGenerator").GetComponent<ItemGenerator>();
        
        agent = GetComponent<NavMeshAgent>();
        goal.transform.parent = gameManager.transform;
        move.goal = goal;
    }

    // UTILS
    public Vector3 TileToCoord(int x, int y)
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

    public bool IsCenterTile(int x, int y)
    {
        return labyrinth.IsCenterTile(x, y);
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

        return float.MaxValue; // 0 à l'origine
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

    public Vector3 GetMyCoord()
    {
        return GetMe().transform.position;
    }

    public Tuple<int,int> GetMyTile()
    {
        return CoordToTile(GetMe().transform);
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
        ColliderBox box = wall.transform.GetChild(0).GetComponent<ColliderBox>();
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
        // TODO
        return false;
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

    public List<GameObject> GetPlayers()
    {
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        List<GameObject> allies = gameManager.GetAllies();
        
        List<GameObject> enemies = gameManager.GetEnemies();
        GameObject mainPlayer = gameManager.GetPlayer();
        
        allPlayers = new List<GameObject>();
        for (int i=0; i<allies.Count; i++)
        {  
            allPlayers.Add(allies[i]);
        }
        for (int i=0; i<enemies.Count; i++)
        {
            allPlayers.Add(enemies[i]);
        }
        allPlayers.Add(mainPlayer);
        
        return allPlayers;
    }


    public bool OrbTaken()
    {
        allPlayers = GetPlayers();

        for (int i =0; i<allPlayers.Count; i++)
        {
            orbManager = allPlayers[i].GetComponent<OrbManager>();
            if (orbManager.HasOrb())
            {
                return  allPlayers[i];
            }
        }
        return false;
        
    }

// TODO
    public List<GameObject> WhoTakeOrb()
    {
        allPlayers = GetPlayers();

        List<GameObject> OrbPlayerList = new List<GameObject>();

        for (int i =0; i<allPlayers.Count; i++)
        {
            orbManager = allPlayers[i].GetComponent<OrbManager>();
            if (orbManager.HasOrb())
            {   

                OrbPlayerList.Add(allPlayers[i]);
            }
        }
        return OrbPlayerList;
        
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
