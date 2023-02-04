using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OptionalWallType
{
    EnemyTeam, // Enemy team can pass through
    AllyTeam,   // Ally team can pass through
    Deactivate, // Both teams can pass through
    None
}

public class ColliderBox : MonoBehaviour
{
    [SerializeField] Material DefaultMaterial;
    [SerializeField] Material BlueMaterial;
    [SerializeField] Material RedMaterial;

    GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        Reset();
    }

    public void SetTeam(int newTeam)
    {
        if (gameObject.layer == 3)
        {
            gameObject.layer = newTeam == gm.GetAllyLayer() ? gm.GetEnemyLayer() : gm.GetAllyLayer();
            GetComponent<Renderer>().material = newTeam == gm.GetAllyLayer() ? BlueMaterial : RedMaterial;
        }
    }

    public OptionalWallType GetWallType()
    {
        if(gameObject.layer == gm.GetEnemyLayer())
        {
            return OptionalWallType.AllyTeam;
        }
        else if(gameObject.layer == gm.GetAllyLayer())
        {
            return OptionalWallType.EnemyTeam;
        }
        else if(gameObject.layer == 3)
        {
            return OptionalWallType.Deactivate;
        }
        else
        {
            return OptionalWallType.None;
        }
    }

    public void Reset()
    {
        gameObject.layer = 3;
        GetComponent<Renderer>().material = DefaultMaterial;
    }
}
