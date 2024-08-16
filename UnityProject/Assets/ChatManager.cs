using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager Singleton;

    [SerializeField] CanvasGroup chatcontent;
    [SerializeField] TMP_InputField chatinput;
    [SerializeField] TextMeshProUGUI Text;

    public string playername;

    private void Awake()
    {
        ChatManager.Singleton = this;
    }

    // Update is called once per frame 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage(chatinput.text, playername);
            chatinput.text = "";
        }
    }

    public void SendChatMessage(string message, string fromwho = null)
    {
        if(string.IsNullOrEmpty(message)) return;

        string S = fromwho + ">" + message;
        SendChatMessageServerRpc(S);
    }

    void AddMessage(string msg) 
    { 
        Text.text += msg + "\n";
    }

    [Rpc(SendTo.Server)]
    void SendChatMessageServerRpc(string message)
    {
        RecieveChatMessageClientRPC(message);
    }

    [Rpc(SendTo.ClientsAndHost)]
    void RecieveChatMessageClientRPC(string message)
    {
        ChatManager.Singleton.AddMessage(message);
    }
}
