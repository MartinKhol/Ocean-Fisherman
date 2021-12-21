using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public static Message Instance;

    private Text text;

    private bool messageUp = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        text = GetComponent<Text>();
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        text.text = message;
  
        if (messageUp == false)
        {
            messageUp = true;
            Invoke("HideMessage", duration); 
        }
    }

    private void HideMessage()
    {
        text.text = string.Empty;
        messageUp = false;
    }

}
