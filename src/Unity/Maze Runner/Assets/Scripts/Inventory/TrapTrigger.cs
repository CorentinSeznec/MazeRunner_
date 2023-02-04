using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    private int team = -1;
    [SerializeField] int speedChange = 80;
    [SerializeField] float time = 10F;
    private GameManager gameManager;

    public void SetTeam(int layer)
    {
        team = layer;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerStay(Collider collider)
    {
        if (team != -1)
        {
            int layer = collider.gameObject.layer;
            if (layer != team && (layer == gameManager.GetAllyLayer() || layer == gameManager.GetEnemyLayer()))
            {
                gameManager.SpeedModifier(collider.gameObject, speedChange, time);
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
