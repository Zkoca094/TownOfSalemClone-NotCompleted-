using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using Mirror;
using System.Linq;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;
    public Text LobbyNameText, playerNameText;
    public GameObject PlayerListViewContent;
    public GameObject PlayerListItemPrefab;
    public GameObject LocalPlayerObject;
    public Button StartGameButton;
    public Text ReadyButtonText;
    public string StartsceneName,leaveSceneName;

    public ulong CurrentLobbyID;
    public bool PlayerItemCreated = false;
    public List<PlayerListItem> PlayerListItems = new List<PlayerListItem>();
    public PlayerObjectController LocalPlayerController;
    public CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
                return manager;
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }

    }
    public void Awake()
    {
        if (instance == null) instance = this;
    }
    public void ReadyPlayer()
    {
        LocalPlayerController.ChangeReady();
    }
    public void UpdateButton()
    {
        if (LocalPlayerController.Ready)
        {
            ReadyButtonText.text = "Unready";
        }
        else 
        {
            ReadyButtonText.text = "Ready";
        }
    }
    public void CheckIfAllReady()
    {
        bool AllReady = false;
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            if (player.Ready)
            {
                AllReady = true;
            }
            else
            {
                AllReady = false;
                break;
            }
        }
        if (AllReady)
        {
            StartGameButton.interactable = true;
            if (LocalPlayerController.playerIDNumber == 1)
            {
                StartGameButton.gameObject.SetActive(true);
            }
            else
            {
                StartGameButton.gameObject.SetActive(false);
            }
        }
        else
        {
            StartGameButton.interactable = false;
        }
    }
    public void UpdateLobbyName()
    {
        CurrentLobbyID = Manager.GetComponent<SteamLobby>().CurrentLobbyID;
        LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name");
    }
    public void UpdatePlayerList()
    {
        if (!PlayerItemCreated) { CreateHostPlayerItem(); }
        if (PlayerListItems.Count < Manager.GamePlayers.Count) { CreateClientPlayerItem(); }
        if (PlayerListItems.Count > Manager.GamePlayers.Count) { RemovePlayerItem(); }
        if (PlayerListItems.Count == Manager.GamePlayers.Count) { UpdatePlayerItem(); }
    }
    public void FindLocalPlayer()
    {
        LocalPlayerObject = GameObject.Find("LocalGamePlayer");
        LocalPlayerController = LocalPlayerObject.GetComponent<PlayerObjectController>();
    }
    public void CreateHostPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            GameObject NewPlayerItem= Instantiate(PlayerListItemPrefab) as GameObject;
            PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();
            NewPlayerItemScript.PlayerName = player.playerName;
            NewPlayerItemScript.ConnectionID = player.ConnectionID;
            NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
            NewPlayerItemScript.Ready = player.Ready;
            NewPlayerItemScript.SetPlayerValues();
            NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
            NewPlayerItem.transform.localScale = Vector3.one;
            PlayerListItems.Add(NewPlayerItemScript);
        }
        PlayerItemCreated = true;
    }
    public void CreateClientPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            if (!PlayerListItems.Any(b => b.ConnectionID == player.ConnectionID))
            {
                GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
                PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();
                NewPlayerItemScript.PlayerName = player.playerName;
                NewPlayerItemScript.ConnectionID = player.ConnectionID;
                NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                NewPlayerItemScript.Ready = player.Ready;
                NewPlayerItemScript.SetPlayerValues();
                NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
                NewPlayerItem.transform.localScale = Vector3.one;
                PlayerListItems.Add(NewPlayerItemScript);
            }
        }
    }
    public void UpdatePlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            foreach (PlayerListItem listItem in PlayerListItems)
            {
                if (listItem.ConnectionID == player.ConnectionID)
                {
                    listItem.PlayerName = player.playerName;
                    listItem.Ready = player.Ready;
                    listItem.SetPlayerValues();
                    if (player == LocalPlayerController)
                    {
                        UpdateButton();
                    }
                }
            }
        }
        CheckIfAllReady();
    }
    public void RemovePlayerItem()
    {
        List<PlayerListItem> playerListItemsToRemove = new List<PlayerListItem>();
        foreach (PlayerListItem playerListItem in PlayerListItems)
        {
            if (!Manager.GamePlayers.Any(b=>b.ConnectionID==playerListItem.ConnectionID))
            {
                playerListItemsToRemove.Add(playerListItem);
            }
        }
        if (playerListItemsToRemove.Count > 0)
        {
            foreach (PlayerListItem playerListItemToRemove in playerListItemsToRemove)
            {
                GameObject objectToRemove = playerListItemToRemove.gameObject;
                PlayerListItems.Remove(playerListItemToRemove);
                Destroy(objectToRemove);
                objectToRemove = null;
            }
        }
    }
    public void StartGame()
    {
        if (GameManager.singleton.CanStartGame())
        {
            LocalPlayerController.CanStartGame(StartsceneName);
        }
        else
        {
            Debug.Log("Please check the number of players and the number of roles.");
        }
    }
    public void LeaveLobby()
    {
        LocalPlayerController.CanLeaveGame(Manager.GetComponent<SteamLobby>().CurrentLobbyID);
    }

    public void Update()
    {
        if (LocalPlayerController != null && playerNameText.text != LocalPlayerController.playerName)
            playerNameText.text = LocalPlayerController.playerName;

        if (LocalPlayerController != null && LocalPlayerController.playerIDNumber != 1)
        {
            Button[] allButtons = GameObject.FindObjectsOfType<Button>();
            for (int i = 0; i < allButtons.Length; i++)
            {
                if (allButtons[i].name.Contains("AddButton") || allButtons[i].name.Contains("LessButton"))
                {
                    allButtons[i].interactable = false;
                }
            }
        }
    }
}
