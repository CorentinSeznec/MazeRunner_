using UnityEngine;

public class InteractablePickableObject : InteractableObject
{
    [SerializeField] string given_object;

    public override void OnInteract(GameObject player)
    {
        Inventory inv = player.GetComponent<Inventory>();
        if(!inv.IsInventoryFull()){
            inv.GiveItem(given_object);
            int id = GetComponent<InGameItem>().getGenerationID();
            if(id != 0)
            {
                GameObject.Find("ItemGenerator").GetComponent<ItemGenerator>().PickedItem(id);
            }
            Destroy(gameObject);
        }
    }
}