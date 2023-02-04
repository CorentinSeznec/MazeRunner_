using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryMainPlayer : Inventory
{
    private List<UIItem> uiItems = new List<UIItem>();

    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform inventoryPanel;
    
    private float base_alpha;
    [SerializeField] GameObject hand;
    private HandManager hm;

    private void Awake()
    {
        
        hm = hand.GetComponent<HandManager>();
        for(int i = 0 ; i < slotNb ; i++){
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(inventoryPanel);
            uiItems.Add(instance.GetComponentInChildren<UIItem>());
        }
        base_alpha = inventoryPanel.transform.GetChild(selectedSlot).GetComponent<Image>().color.a;
    }

    void Update(){
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if(scrollValue > 0){
            changeSelected(-1);
        }
        else if (scrollValue < 0){
            changeSelected(1);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            UseSelectedItem();
        }
        
        for(int i = 1 ; i < slotNb + 1; i++){
            if(Input.GetKeyDown(i.ToString())){
                SetSelected(i - 1);
            }
        }
    }

    public override void GiveItem(int id){
        Item itemToAdd = itemDatabase.GetItem(id);
        int emptySlot = characterItems.FindIndex(i => i == null);
        characterItems.RemoveAt(emptySlot);
        characterItems.Insert(emptySlot, itemToAdd);
        UpdateSlot(emptySlot, itemToAdd);
        SetSelected(selectedSlot);
    }

    public override void GiveItem(string name){
        Item itemToAdd = itemDatabase.GetItem(name);
        int emptySlot = characterItems.FindIndex(i => i == null);
        characterItems.RemoveAt(emptySlot);
        characterItems.Insert(emptySlot, itemToAdd);
        UpdateSlot(emptySlot, itemToAdd);
        SetSelected(selectedSlot);
    }

    public override void RemoveItem(int index)
    {
        characterItems.RemoveAt(index);
        characterItems.Insert(index, null);
        UpdateSlot(index, null);
        SetSelected(selectedSlot);
    }

    protected override void changeSelected(int jump){
        int newSlot = selectedSlot + jump;
        if(newSlot < 0){
            newSlot = slotNb - 1;
        }
        else if (newSlot > slotNb - 1){
            newSlot = 0;
        }
        SetSelected(newSlot);
    }

    override protected void SetSelected(int slotNum){
        int save = selectedSlot;
        selectedSlot = slotNum;
        UpdateUI(save);
    }

    private void UpdateUI(int prevSelected)
    {
        Color c1 = inventoryPanel.transform.GetChild(selectedSlot).gameObject.GetComponent<Image>().color;
        Color c2 = c1;
        c1.a = base_alpha;
        c2.a = 255;
        inventoryPanel.transform.GetChild(prevSelected).gameObject.GetComponent<Image>().color = c1;
        inventoryPanel.transform.GetChild(selectedSlot).gameObject.GetComponent<Image>().color = c2;
        uiItems[prevSelected].UnSelectedItem();
        uiItems[selectedSlot].SelectedItem();
        hm.GetInHand(ObjectSelected());
    }

    public override void UseSelectedItem()
    {
        Item selected = ObjectSelected();
        if(selected != null)
        {
            selected.UseIt(gameObject);
            RemoveItem(selectedSlot);
        }
    }

    public List<UIItem> GetUIItems()
    {
        return uiItems;
    }

    // Change the value of a slot in the inventory UI
    private void UpdateSlot(int slot, Item given_item)
    {
        uiItems[slot].UpdateItem(given_item);
    }
}
