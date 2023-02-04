using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int layer = 0;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void SetTeam(int given_layer)
    {
        layer = given_layer;
    }

    private void OnTriggerEnter(Collider other)
    {
        int colliderLayer = other.gameObject.layer;
        if((colliderLayer == gm.GetAllyLayer() || colliderLayer == gm.GetEnemyLayer()) && colliderLayer != layer)
        {
            other.GetComponent<OrbManager>().ThrowOrb();
            Destroy(gameObject);
        }
        else if(colliderLayer != layer)
        {
            Destroy(gameObject);
        }
    }
}
