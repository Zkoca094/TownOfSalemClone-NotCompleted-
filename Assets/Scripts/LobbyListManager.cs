using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
public class LobbyListManager : MonoBehaviour
{
    public static LobbyListManager instance;
    public GameObject lobbiesMenu, createLobbyMenu;
    public GameObject lobbyDataItemPrefab;
    public GameObject lobbylistContent;
    public GameObject lobbiesButton, hostButton, settingButton, controlButton, exitButton;
    public List<GameObject> listOfLobbies = new List<GameObject>();
    public int maxNumber, minNumber = 0;
    private int playerNumber;
    private string lobbyName;
    public InputField playerNumberText, lobbyNameText;
    public GameObject switchButton;
    public Vector2 handlePosition;
    private bool publicState;
    public Text publicText;
    public int PlayerNumber
    {
        get { return playerNumber; }
        set { playerNumber = value; }
    }
    public string LobbyName
    {
        get { return lobbyName; }
        set { lobbyName = value; }
    }
    private void Awake()
    {
        if (instance == null) { instance = this; }
        SetDefaults();
    }
    public void SetDefaults()
    {
        maxNumber = CustomNetworkManager.singleton.maxConnections;
        minNumber = 4;
        playerNumber = minNumber;
        playerNumberText.text = playerNumber.ToString();
        PlayerNumber = int.Parse(playerNumberText.text);
        handlePosition.x = switchButton.transform.GetComponent<RectTransform>().sizeDelta.y / 2;
        OnSwitchButton(publicState);
    }
    public void DestroyLobbies()
    {
        foreach (GameObject lobbyitem in listOfLobbies)
        {
            Destroy(lobbyitem);
        }
        listOfLobbies.Clear();
    }
    public void DisplayLobbies(List<CSteamID> lobbyIDs, LobbyDataUpdate_t result)
    {
        for (int i = 0; i < lobbyIDs.Count; i++)
        {
            if (lobbyIDs[i].m_SteamID==result.m_ulSteamIDLobby)
            {
                GameObject createdItem = Instantiate(lobbyDataItemPrefab);
                createdItem.GetComponent<LobbyEntryData>().lobbyID = (CSteamID)lobbyIDs[i].m_SteamID;
                createdItem.GetComponent<LobbyEntryData>().lobbyName = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "name");
                createdItem.GetComponent<LobbyEntryData>().SetLobbyData();
                createdItem.transform.SetParent(lobbylistContent.transform);
                createdItem.transform.localScale = Vector3.one;
                listOfLobbies.Add(createdItem);
            }
        }
    }
    public void GetListOfLobbies()
    {
        lobbiesButton.SetActive(false);
        hostButton.SetActive(false);
        settingButton.SetActive(false);
        controlButton.SetActive(false);
        exitButton.SetActive(false);
        lobbiesMenu.SetActive(true);
        createLobbyMenu.SetActive(false);
        SteamLobby.instance.GetLobbyList();
    }
    public void OpenCreateLobby()
    {
        SetDefaults();
        lobbiesButton.SetActive(false);
        hostButton.SetActive(false);
        settingButton.SetActive(false);
        controlButton.SetActive(false);
        exitButton.SetActive(false);
        lobbiesMenu.SetActive(false);
        createLobbyMenu.SetActive(true);
    }
    public void GoBackButton()
    {
        lobbiesButton.SetActive(true);
        hostButton.SetActive(true);
        settingButton.SetActive(true);
        controlButton.SetActive(true);
        exitButton.SetActive(true);
        lobbiesMenu.SetActive(false);
        createLobbyMenu.SetActive(false);
        DestroyLobbies();
    }
    public void NextButton()
    {
        if (playerNumber < maxNumber)
            playerNumber++;
        playerNumberText.text = playerNumber.ToString();
        PlayerNumber = int.Parse(playerNumberText.text);
    }
    public void PrevButton()
    {
        if (playerNumber > minNumber)
            playerNumber--;
        playerNumberText.text = playerNumber.ToString();
        PlayerNumber = int.Parse(playerNumberText.text);
    }
    public void CreateHostLobby()
    {
        if (lobbyName == null)
            Debug.Log("Please enter a looby name");
        else
        {
            PlayerPrefs.SetString("LobbyName", lobbyName.ToUpper());
            if (publicState)
                SteamLobby.instance.HostLobby(ELobbyType.k_ELobbyTypePublic, PlayerNumber);
            else
                SteamLobby.instance.HostLobby(ELobbyType.k_ELobbyTypeFriendsOnly, PlayerNumber);
        }
    }
    public void SwitchButton()
    {
        publicState = !publicState;
        OnSwitchButton(publicState);
    }
    public void OnSwitchButton(bool state)
    {
        if (state)
        {
            switchButton.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = handlePosition * -1;
            publicText.text = "PUBLIC";
            switchButton.transform.GetChild(0).GetComponent<Image>().color = Color.green;
        }
        else
        {
            switchButton.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = handlePosition;
            publicText.text = "PRIVATE";
            switchButton.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        }
    }
}
