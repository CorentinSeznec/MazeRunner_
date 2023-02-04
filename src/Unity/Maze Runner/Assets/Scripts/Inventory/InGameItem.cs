using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameItem : MonoBehaviour
{
    private int generationID;
    private ItemDatabase database;
    private Item item;


    public int getGenerationID()
    {
        return generationID;
    }

    public GameObject CreateItem(Transform t, int id)
    {
        database = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        item = database.GetItem(name);
        generationID = id;
        return Instantiate(item.GetInHandModel(), t.position, t.rotation);
    }

    public void DestroyItem()
    {
        GameObject.Find("ItemGenerator").GetComponent<ItemGenerator>().PickedItem(generationID);
        Destroy(gameObject);
    }
}
