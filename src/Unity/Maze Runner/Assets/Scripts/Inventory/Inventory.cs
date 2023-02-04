using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    protected List<Item> characterItems = new List<Item>();
    protected ItemDatabase itemDatabase;
    protected int selectedSlot = 0;
    public int slotNb = 3;

    void Start()
    {
        itemDatabase = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        for(int i = 0 ; i < slotNb ; i++){
            characterItems.Add(null);
        }
        changeSelected(0);
    }

    public virtual void GiveItem(int id)
    {
        Item itemToAdd = itemDatabase.GetItem(id);
        int emptySlot = characterItems.FindIndex(i => i == null);
        characterItems.RemoveAt(emptySlot);
        characterItems.Insert(emptySlot, itemToAdd);
    }

    public virtual void GiveItem(string name)
    {
        Item itemToAdd = itemDatabase.GetItem(name);
        int emptySlot = characterItems.FindIndex(i => i == null);
        characterItems.RemoveAt(emptySlot);
        characterItems.Insert(emptySlot, itemToAdd);
    }

    public virtual int SearchForItem(int id)
    {
        return characterItems.FindIndex(item => item != null && item.GetId() == id);
    }

    public virtual int SearchForItem(string name)
    {
        return characterItems.FindIndex(item => item != null && item.GetName() == name);
    }

    public virtual int SearchForItem(GameObject inHandModel)
    {
        return characterItems.FindIndex(item => item != null && item.GetInHandModel() == inHandModel);
    }

    public virtual void RemoveItem(int index)
    {
        characterItems.RemoveAt(index);
        characterItems.Insert(index, null);
    }

    protected virtual void changeSelected(int jump)
    {
        selectedSlot += jump;
        if(selectedSlot < 0){
            selectedSlot = slotNb;
        }
        else if (selectedSlot > slotNb - 1){
            selectedSlot = 0;
        }
    }

    protected virtual void SetSelected(int slotNum)
    {
        selectedSlot = slotNum;
    }

    public int GetSelectedSlot()
    {
        return selectedSlot;
    }

    public Item ObjectSelected()
    {
        return characterItems[selectedSlot];
    }

    public virtual void UseSelectedItem()
    {
        Item selected = ObjectSelected();
        if(selected != null)
        {
            selected.UseIt(gameObject);
            RemoveItem(selectedSlot);
        }
    }

    public List<Item> OwnItems()
    {
        return characterItems;
    }

    public bool UseItem(int index)
    {
        if(index < characterItems.Count)
        {
            characterItems[index].UseIt(gameObject);
            RemoveItem(index);
            return true;
        }
        return false;
    }

    public bool UseItem(GameObject obj)
    {
        int index = SearchForItem(obj);
        if(index  != -1)
        {
            characterItems[index].UseIt(gameObject);
            RemoveItem(index);
            return true;
        }
        return false;
    }

    public bool UseItem(Item item)
    {
        int index = SearchForItem(item.GetId());
        if(index  != -1)
        {
            characterItems[index].UseIt(gameObject);
            RemoveItem(index);
            return true;
        }
        return false;
    }



    public virtual bool IsInventoryFull()
    {
        return !characterItems.Contains(null);
    }
}
