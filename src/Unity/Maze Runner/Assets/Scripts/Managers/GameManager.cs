using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool fullAIGame;

    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject aiPlayerObj;
    [SerializeField] GameObject allyAIObj;
    [SerializeField] GameObject enemyAIObj;
    [SerializeField] GameObject minotaureObj;

    private GameObject player = null;
    private List<GameObject> allyAI = null;
    private List<GameObject> enemyAI = null;
    private GameObject minotaure = null;

    private GameObject orbHolder = null;

    private int allyOrb = 0; 
    private int enemyOrb = 0; 

    [SerializeField] int allyLayer = 6;
    [SerializeField] int enemyLayer = 7;

    [SerializeField] float hazardDistance;

    [SerializeField] GameObject portalObject;
    private InteractablePortal portal;

    private MessageManager messageManager;
    private Labyrinth labyrinth;
    
    // Start is called before the first frame update
    void Start()
    {
        labyrinth = labyrinth = GameObject.Find("Labyrinth").GetComponent<Labyrinth>();
        
        allyAI = new List<GameObject>();
        enemyAI = new List<GameObject>();

        GameObject temp = fullAIGame ? aiPlayerObj : playerObj;
        player = Instantiate(temp, new Vector3(1, 1, 0), Quaternion.identity);
        player.GetComponent<Outline>().OutlineColor = Color.blue;
        player.layer = allyLayer;
        player.transform.parent = transform;
        
        minotaure = Instantiate(minotaureObj, labyrinth.TileToCoord(1, 1), Quaternion.identity);
        minotaure.transform.parent = transform;
        
        for(int i = 0 ; i < 2 ; i++)
        {
            allyAI.Add(Instantiate(allyAIObj, new Vector3((i+1)*2 + 1, 1, 0), Quaternion.identity));
            allyAI[i].layer = allyLayer;
            allyAI[i].GetComponent<Outline>().OutlineColor = Color.blue;
            allyAI[i].transform.parent = transform;

        }
        for(int i = 0 ; i < 3 ; i++)
        {
            enemyAI.Add(Instantiate(enemyAIObj, new Vector3(-(i+1) * 2, 1, 0), Quaternion.identity));
            enemyAI[i].layer = enemyLayer;
            enemyAI[i].GetComponent<Outline>().OutlineColor = Color.red; 
            enemyAI[i].transform.parent = transform;
        }
        messageManager = GameObject.Find("MessageManager").GetComponent<MessageManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // GETERS
    public int GetNbAlly()
    {
        return allyAI.Count + 1;
    }

    public int GetNbEnemy()
    {
        return enemyAI.Count;
    }

    public int GetAllyLayer()
    {
        return allyLayer;
    }

    public int GetEnemyLayer()
    {
        return enemyLayer;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public List<GameObject> GetAllies()
    {
        return allyAI;
    }

    public List<GameObject> GetEnemies()
    {
        return enemyAI;
    }

    public GameObject GetMinotaur()
    {
        return minotaure;
    }

    public GameObject GetOrbHolder()
    {
        return orbHolder;
    }

    // INFO ON ORB
    public bool AllyHasOrb(int numOrb)
    {
        return orbHolder != null && orbHolder.layer == allyLayer && allyOrb+1 == numOrb;
    }

    public bool EnemyHasOrb(int numOrb)
    {
        return orbHolder != null && orbHolder.layer == enemyLayer && enemyOrb+1 == numOrb;
    }


    // INFO ON DISTANCE
    float ComputeDistance(GameObject g1, GameObject g2)
    {
        return (g1.transform.position - g2.transform.position).magnitude;
    }

    public bool PlayerCloseToMinotaure()
    {
        float distance = ComputeDistance(player, minotaure);
        return distance <= hazardDistance;
    }

    public bool PlayerCloseToEnemy()
    {
        float distance = hazardDistance + 1;
        foreach(GameObject enemy in enemyAI)
        {
            distance = ComputeDistance(player, enemy);
            if(distance <= hazardDistance)
            {
                return true;
            }
        }
        return false;
    }

    public bool AllyCloseToEnemy()
    {
        float distance = hazardDistance + 1;
        foreach(GameObject ally in allyAI)
        {
            foreach(GameObject enemy in enemyAI)
            {
                distance = ComputeDistance(ally, enemy);
                if(distance <= hazardDistance)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool OrbHolderCloseToEnemy()
    {
        if(orbHolder != null)
        {
            float distance;
            if(orbHolder.layer == allyLayer)
            {
                // orb holder is an ally 
                foreach(GameObject enemy in enemyAI)
                {
                    distance = ComputeDistance(orbHolder, enemy);
                    if(distance <= hazardDistance)
                    {
                        return true;
                    }
                }
            }
            else
            {
                // orb holder is an enemy 
                foreach(GameObject ally in allyAI)
                {
                    distance = ComputeDistance(orbHolder, ally);
                    if(distance <= hazardDistance)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool OrbFree()
    {
        return orbHolder == null;
    }

    void MessageOrbTaken(int layer)
    {
        if(layer == allyLayer)
        {
            if(allyOrb == 0)
            {
                messageManager.SendMessage("The first orb has been taken by an ally", Color.green);
            }
            else if(allyOrb == 1)
            {
                messageManager.SendMessage("The last orb has been taken by an ally", Color.green);
            }
        }
        else
        {
            if(enemyOrb == 0)
            {
                messageManager.SendMessage("The first orb has been taken by an enemy", Color.red);
            }
            else if(enemyOrb == 1)
            {
                messageManager.SendMessage("The last orb has been taken by an enemy", Color.red);
            }
        }
    }

    public void ITookTheOrb(GameObject holder)
    {
        orbHolder = holder;
        Outline outline = orbHolder.GetComponent<Outline>();
        if(orbHolder.layer == allyLayer)
        {
            outline.OutlineColor = Color.blue;
        }
        else
        {
            outline.OutlineColor = Color.red;
        }
        MessageOrbTaken(orbHolder.layer);
        outline.OutlineWidth = 10f;
    }

    void MessageOrbPlaced(int layer)
    {
        if(layer == allyLayer)
        {
            if(allyOrb == 1)
            {
                messageManager.SendMessage("The first orb has been placed by an ally", Color.green);
            }
            else
            {
                messageManager.SendMessage("The last orb has been placed by an ally", Color.green);
            }
        }
        else
        {
            if(enemyOrb == 1)
            {
                messageManager.SendMessage("The first orb has been placed by an enemy", Color.red);
            }
            else
            {
                messageManager.SendMessage("The last orb has been placed by an enemy", Color.red);
            }
        }
    }

    public void IPlacedTheOrb(GameObject holder)
    {
        if(orbHolder.layer == allyLayer)
        {
            allyOrb += 1;
        }
        else
        {
            enemyOrb += 1;
        }
        MessageOrbPlaced(orbHolder.layer);
        orbHolder.GetComponent<Outline>().OutlineWidth = 0f;
        orbHolder = null;
        VerifOrbs();
    }

    public void IThrewTheOrb(GameObject holder)
    {
        if(orbHolder.layer == allyLayer)
        {
            messageManager.SendMessage("The orb has been dropped by an ally", Color.green);
        }
        else
        {
            messageManager.SendMessage("The orb has been dropped by an enemy", Color.red);
        }
        orbHolder.GetComponent<Outline>().OutlineWidth = 0f;
        orbHolder = null;
    }

    public void VerifOrbs()
    {
        if(allyOrb >= 2)
        {
            if(portal == null)
            {
                SpawnPortal();
            }
            portal.AddWinner(allyLayer);
        }
        if(enemyOrb >= 2)
        {
            if(portal == null)
            {
                SpawnPortal();
            }
            portal.AddWinner(enemyLayer);
        }
    }

    void SpawnPortal()
    {
        GameObject temp = Instantiate(portalObject, new Vector3(0, 0.8f, 0), Quaternion.identity);
        portal = temp.GetComponent<InteractablePortal>();
    }

    // USE ITEMS
    private void applySpeedAlly(float speed_mult)
    {
        if(!fullAIGame)
        {
            player.GetComponent<SpeedManager>().SetPlayerSpeed(speed_mult);
        }
        else
        {
            player.GetComponent<SpeedManager>().SetAISpeed(speed_mult);
        }
        foreach (GameObject bot in allyAI)
        {
            bot.GetComponent<SpeedManager>().SetAISpeed(speed_mult);
        }
    }

    private void applySpeedEnemy(float speed_mult)
    {
        foreach (GameObject bot in enemyAI)
        {
            bot.GetComponent<SpeedManager>().SetAISpeed(speed_mult);
        }
    }

    public void SpeedModifierTeam(int team, int speed)
    {
        if (team == allyLayer)
        {
            if (speed > 100)
            {
                float speed_mult = (float)speed / 100;
                applySpeedAlly(speed_mult);
            }
            else
            {
                float speed_mult = (float)speed / 100;
                applySpeedEnemy(speed_mult);
            }
        }
        else
        {
            if (speed > 100)
            {
                float speed_mult = (float)speed / 100;
                applySpeedEnemy(speed_mult);
            }
            else
            {
                float speed_mult = (float)speed / 100;
                applySpeedAlly(speed_mult);
            }
        }
        
    }

    public void SpeedModifier(GameObject player, int speed, float time)
    {
        float speed_mult = (float)speed / 100;
        if (player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>())
        {
            player.GetComponent<SpeedManager>().SetPlayerSpeed(speed_mult);
        }
        else
        {
            player.GetComponent<SpeedManager>().SetAISpeed(speed_mult);
        }
    }

    public void TrapPlacer(int team, Vector3 position, GameObject trap)
    {
        Color color = (team== GetEnemyLayer()) ? Color.red : Color.blue;
        Tuple<int, int> pos_tuple = labyrinth.CoordToTile(position);
        Vector3 pos = labyrinth.TileToCoord(pos_tuple.Item1, pos_tuple.Item2);

        GameObject placed = Instantiate(trap, pos, Quaternion.identity);
        placed.transform.parent = transform;
        var ps = placed.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        ps.startColor = color;
        placed.transform.GetChild(1).GetComponent<TrapTrigger>().SetTeam(team);
    }
}
