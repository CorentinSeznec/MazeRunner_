using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionalWall : InteractableObject
{
    bool up; // False = Down | True = Up
    [SerializeField] float maxTimeUp = 15;
    [SerializeField] float timeRise = 1.5f;
    [SerializeField] float timeLower = 5;
    [SerializeField] float coolDown = 15f;

    BotCollider botCollider1;
    BotCollider botCollider2;
    
    float activeTime = 0;

    float deltaUpDown = 19.5f;
    float pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = 0;
        up = false;
        botCollider1 = transform.parent.GetChild(1).GetComponent<BotCollider>();
        botCollider2 = transform.parent.GetChild(2).GetComponent<BotCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (up)
        {
            activeTime = activeTime + Time.deltaTime;
            if (activeTime < timeRise)
            {
                float deltaMove = Time.deltaTime * deltaUpDown / timeRise;
                moveUp(deltaMove);
            }
            else if (activeTime > maxTimeUp)
            {
                float deltaMove = Time.deltaTime * deltaUpDown / timeLower;
                moveDown(deltaMove);
            }
        }
    }

    public override void OnInteract(GameObject player)
    {
        if (!up && player.GetComponent<WallCooldown>().IsNotOnCoolDown())
        {
            up = true;
            enableBotCollider(player.layer);
            player.GetComponent<WallCooldown>().SetOnCooldown(coolDown);
            gameObject.GetComponent<ColliderBox>().SetTeam(player.layer);
            Invoke("disableBotCollider", timeRise + coolDown + timeLower - 0.2f);
        }
    }

    void moveUp(float dist)
    {
        pos = pos + dist;
        if (pos >= deltaUpDown)
        {
            up = true;
        }
        transform.Translate(new Vector3(0,dist, 0));
    }

    void moveDown(float dist)
    {
        pos = pos - dist;
        if (pos <= 0)
        {
            up = false;
            activeTime = 0;
            gameObject.GetComponent<ColliderBox>().Reset();
        }
        transform.Translate(new Vector3(0,-dist, 0));
    }

    void enableBotCollider(int layer)
    {
        botCollider1.SetActive(layer, Time.fixedTime, timeRise + coolDown + timeLower);
        botCollider2.SetActive(layer, Time.fixedTime, timeRise + coolDown + timeLower);
    }

    void disableBotCollider()
    {
        botCollider1.SetInactive();
        botCollider2.SetInactive();
    }
}
