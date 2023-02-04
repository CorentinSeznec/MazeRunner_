using UnityEngine;
using System.Collections.Generic;

public class InteractablePortal : InteractableObject
{

    private GameManager gm;

    private List<int> winnerLayer;

    private List<GameObject> wonAlly;
    private List<GameObject> wonEnemy;

    private MessageManager messageManager;

    void Awake()
    {   
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        winnerLayer = new List<int>();
        wonAlly = new List<GameObject>();
        wonEnemy = new List<GameObject>();

        messageManager = GameObject.Find("MessageManager").GetComponent<MessageManager>();
    }

    public void AddWinner(int winner)
    {
        if(!winnerLayer.Contains(winner))
        {
            winnerLayer.Add(winner);
        }
    }

    int VerifWin()
    {
        if(wonAlly.Count == gm.GetNbAlly())
        {
            messageManager.SendMessage("VICTORY", Color.green);
            return gm.GetAllyLayer();
        }
        else if(wonEnemy.Count == gm.GetNbEnemy())
        {
            messageManager.SendMessage("DEFEAT", Color.red);
            return gm.GetEnemyLayer();
        }
        return 0;
    }

    public override void OnInteract(GameObject player)
    {
        if(winnerLayer.Contains(player.layer))
        {
            if(player.layer == gm.GetAllyLayer() && !wonAlly.Contains(player))
            {
                wonAlly.Add(player);
                VerifWin();
            }
            else if(!wonEnemy.Contains(player))
            {
                wonEnemy.Add(player);
                VerifWin();
            }
            
        }
    }
}