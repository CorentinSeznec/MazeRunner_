using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Item
{
    private int id;
    private string name;
    private Sprite icon;
    private GameObject inHandModel;
    private int rarity;

    public Item(int given_id, string given_name, int given_rarity)
    {
        id = given_id;
        name = Regex.Replace(given_name, @"\s+", "");
        inHandModel = (Resources.Load("Models/Items/" + name) as GameObject);
        icon = Resources.Load<Sprite>("Sprites/Items/" + name);
        rarity =  given_rarity;
    }

    public Item(Item given_item)
    {
        id = given_item.GetId();
        name = given_item.GetName();
        inHandModel = (Resources.Load("Models/Items/" + name) as GameObject);
        icon = Resources.Load<Sprite>("Sprites/Items/" + name);
        rarity =  given_item.GetRarity();
    }


    public int GetId()
    {
        return id;
    }

    public string GetName()
    {
        return name;
    }

    public int GetRarity()
    {
        return rarity;
    }
    public Sprite GetIcon()
    {
        return icon;
    }

    public GameObject GetInHandModel()
    {
        return inHandModel;
    }

    public void UseIt(GameObject player)
    {
        UsableObject usable = inHandModel.GetComponent<UsableObject>();
        usable.Use(player);
    }
}
