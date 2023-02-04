using UnityEngine;

public class InteractableOrb : InteractableObject
{
    private GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public override void OnInteract(GameObject player)
    {
        OrbManager inv = player.GetComponent<OrbManager>();
        if(inv.canTakeOrb() && gm.OrbFree()){
            inv.takeOrb();
            gm.ITookTheOrb(player);
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}