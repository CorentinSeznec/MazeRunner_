using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIItem : MonoBehaviour
{
    private Item item;
    private Image spriteImage;
    private TextMeshProUGUI description;

    private void Awake()
    {
        description = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        spriteImage = GetComponent<Image>();
        UpdateItem(null);
        UnSelectedItem();

    }

    public Item GetSlotItem()
    {
        return item;
    }

    public void UpdateItem(Item given_item)
    {
        item = given_item;
        if(item != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = item.GetIcon();
            description.text = item.GetName();
        }
        else
        {
            spriteImage.color = Color.clear;
            description.text = "";
            
        }
    }

    public void SelectedItem()
    {
        description.enabled = true;
    }

    public void UnSelectedItem()
    {
        description.enabled = false;
    }

    public bool IsEmptySlot()
    {
        return item == null;
    }
}
