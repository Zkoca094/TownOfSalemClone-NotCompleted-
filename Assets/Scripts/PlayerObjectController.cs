﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using System;

public class PlayerObjectController : NetworkBehaviour
{
    [SyncVar] public int ConnectionID;
    [SyncVar] public int playerIDNumber;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string playerName;
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool Ready;
    [SyncVar(hook = nameof(SendPlayerColor))] public int PlayerCharacther;
    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get {
            if (manager != null)
                return manager;
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }

    }
    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.instance.FindLocalPlayer();
        LobbyController.instance.UpdateLobbyName();
    }
    public override void OnStartClient()
    {
        if (!hasAuthority)
            gameObject.name = playerName;
        Manager.GamePlayers.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }
    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        LobbyController.instance.UpdatePlayerList();
    }
    [Command]
    public void CmdSetPlayerName(string playerName)
    {
        this.PlayerNameUpdate(this.playerName, playerName);
    }
    public void PlayerNameUpdate(string OldValue, string NewValue)
    {
        if (isServer)
        {
            this.playerName = NewValue;
        }
        if (isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }
    public void PlayerReadyUpdate(bool OldValue, bool NewValue)
    {
        if (isServer)
        {
            this.Ready = NewValue;
        }
        if (isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }
    [Command]
    private void CmdSetPlayerReady()
    {
        this.PlayerReadyUpdate(this.Ready, !this.Ready);
    }
    public void ChangeReady()
    {
        if (hasAuthority)
        {
            CmdSetPlayerReady();
        }
    }
    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void CanStartGame(string sceneName)
    {
        if (hasAuthority)
        {                      
            CmdUpdatePlayerColor(PlayerPrefs.GetInt("charactherIndex"));
            CmdCanStartGame(sceneName);
        }
    }
    [Command]
    public void CmdCanStartGame(string sceneName)
    {
        manager.StartGame(sceneName);
    }
    public void CanLeaveGame(ulong CurrentLobbyID)
    {
        if (hasAuthority)
            CmdCanLeaveGame(CurrentLobbyID);
    }
    [Command]
    public void CmdCanLeaveGame(ulong CurrentLobbyID)
    {
        manager.LeaveGame(CurrentLobbyID);
    }
    [Command]
    public void CmdUpdatePlayerColor(int newValue)
    {
        SendPlayerColor(PlayerCharacther, newValue);
    }
    public void SendPlayerColor(int oldValue, int newValue)
    {
        if (isServer)
        {
            PlayerCharacther = newValue;
        }
        if (isClient && (oldValue != newValue))
        {
            UpdateColor(newValue);
        }
    }
    void UpdateColor(int message)
    {
        PlayerCharacther = message;
    }
}
