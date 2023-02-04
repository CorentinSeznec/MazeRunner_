using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotCollider : MonoBehaviour
{
    private int activeTeam = -1;
    private float time = 0;
    private float activation = 0;

    Vector3 teleporterPoint;

    GameManager gm;

    void Start()
    {
        teleporterPoint = transform.GetChild(0).position;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerStay(Collider collider)
    {
        int layer = collider.gameObject.layer;
        if (activeTeam != -1 && layer != activeTeam && (gm.GetAllyLayer() == layer || gm.GetEnemyLayer() == layer))
        {
            NavMeshAgent agent = collider.gameObject.GetComponent<NavMeshAgent>();
            if (agent)
            {
                IEnumerator coroutine = BlockAgent(agent, activation + time - Time.fixedTime);
                StartCoroutine(coroutine);
            }
        }
    }

    IEnumerator BlockAgent(NavMeshAgent agent, float waittime)
    {
        agent.Warp(teleporterPoint);
        agent.isStopped = true;
        yield return new WaitForSeconds(waittime);
        agent.isStopped = false;
    }

    public void SetActive(int team, float last_activation, float uptime)
    {
        activeTeam = team;
        time = uptime;
        activation = last_activation;
    }

    public void SetInactive()
    {
        activeTeam = -1;
    }
}
