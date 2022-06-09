using Mirror;
using UnityEngine.UI;
using UnityEngine;
using System;
using Steamworks;
public class Chat :MonoBehaviour
{
    public static event Action<string> OnMessage;
    public GameObject chatUI;
    public GameObject chatContent,chatItemPrefab;
    public InputField inputField;
    public MyPlayerUI ui;
    public void Start()
    {
        chatUI.SetActive(true);
        OnMessage += HandleNewMessage;
    }
    public void OnDestroy()
    {
        OnMessage -= HandleNewMessage;
    }
    public void HandleNewMessage(string message)
    {
        Text chatText = Instantiate(chatItemPrefab, chatContent.transform).GetComponent<Text>();
        chatText.text = message;
    }
    public void CmdSendMessage(string message)
    {
        RpcHandleMessage($"<b><i>[{ui.myPlayer.playerName}]</i></b>: {message}");
    }
    private void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"{message}");
    }
    public void Send()
    {
        if (!Input.GetKeyDown(KeyCode.Return)) { return; }
        if (string.IsNullOrWhiteSpace(inputField.text)) { return; }
        CmdSendMessage(inputField.text);
        inputField.text = string.Empty;
    }
}
