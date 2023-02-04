using UnityEngine;

public class UsableTrap : UsableObject
{
    [SerializeField] GameObject trap;

    public override void Use(GameObject player)
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().TrapPlacer(player.layer, player.transform.position, trap);
    }
}
