using UnityEngine;

public class UsableSpeedModificator : UsableObject
{
    [SerializeField] int speedChange;

    public override void Use(GameObject player)
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(player.layer == gm.GetAllyLayer() && speedChange > 100)
        {
            GameObject.Find("MessageManager").GetComponent<MessageManager>().SendMessage("Your team has received a temporary speed boost", Color.green);
        }
        else if(player.layer == gm.GetEnemyLayer() && speedChange < 100)
        {
            GameObject.Find("MessageManager").GetComponent<MessageManager>().SendMessage("Your team has received a temporary speed malus", Color.red);
        }
        gm.SpeedModifierTeam(player.layer, speedChange);
    }
}