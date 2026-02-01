
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoMessageHandler : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> messasges = new List<TextMeshProUGUI>();
    [SerializeField] private int maxMessages;
    [SerializeField] private int index = 0;

    void Start()
    {
        maxMessages = messasges.Count;
        ClearMessages();
    }

    public void AddMessage(string text)
    {
        messasges[index].text = text;
        index++;

        if (index > maxMessages) index = 0;

    }

    public void ClearMessages()
    {
        foreach (var message in messasges)
        {
            message.text = "";
        }
    }
}
