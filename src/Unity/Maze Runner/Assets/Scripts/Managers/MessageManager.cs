using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] Animation messageAnimation;
    [SerializeField] float messageTime = 5f;

    List<Tuple<string, Color>> messages;

    bool free;

    void Start()
    {
        free = true;
        messages = new List<Tuple<string, Color>>();
    }

    void Update()
    {
        if(messages.Count != 0 && free == true)
        {
            ShowMessage(messages[0]);
            messages.RemoveAt(0);
        }
    }

    public void SendMessage(string message, Color color)
    {
        color = new Color(color.r, color.g, color.b, 0);
        messages.Add(new Tuple<string, Color>(message, color));
        
    }

    void ShowMessage(Tuple<string, Color> message)
    {
        free = false;
        messageText.text = message.Item1;
        messageText.color = message.Item2;
        messageAnimation.Play("Appear");
        Invoke("HideMessage", messageTime);
    }

    public void HideMessage()
    {
        messageAnimation.Play("Disappear");
        free = true;
    }
}