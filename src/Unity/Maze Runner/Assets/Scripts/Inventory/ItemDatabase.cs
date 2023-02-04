using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    private List<Item> items = new List<Item>();
    [SerializeField] List<int> rarityRatio = new List<int>(){50, 30, 10, 1};

    public void Awake()
    {
        BuildDatabase();
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.GetId() == id);
    }

    public Item GetItem(string name)
    {
        return items.Find(item => item.GetName() == name);
    }

    public Item GetItem(GameObject inHandModel)
    {
        return items.Find(item => item.GetInHandModel() == inHandModel);
    }

    public List<Item> GetItemsByRarity(int rarity)
    {
        return items.FindAll(item => item.GetRarity() == rarity);
    }

    public List<int> GetRarityRatio()
    {
        return rarityRatio;
    }

    public List<Item> GetItemList()
    {
        return items;
    }

    void BuildDatabase()
    {
        items = new List<Item>(){
            new Item(0, "Speed_Boost", 0),
            new Item(1, "Speed_Malus", 1),
            new Item(2, "Orb", -1),
            new Item(3, "Trap_Speed_Malus", 1),
            new Item(4, "Blow_Gun", 1)
        };
    }
}
