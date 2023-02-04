using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public Item inHand = null;
    public GameObject instance = null;

    void Update(){
        if(instance != null){
            instance.transform.position = transform.position;
        }
    }

    public void GetInHand(Item item){
        if(item != null && inHand != null && item.GetName() == inHand.GetName()) return;
        SuppressInHand();
        inHand = item;
        if(item != null){
            instance = Instantiate(inHand.GetInHandModel(), transform);
            instance.layer = 0;
            instance.tag = "Untagged";
        }
    }

    private void SuppressInHand(){
        if(instance != null){
            Destroy(instance);
        }
    }
}
