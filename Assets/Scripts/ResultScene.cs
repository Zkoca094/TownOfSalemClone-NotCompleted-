using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultScene : MonoBehaviour
{
    public GameObject panelItemPrefab;
    void Start()
    {
        if (GameManager.singleton != null)
        {
            SetPanel();
        }
    }
    public void SetPanel()
    {
        foreach (PlayerObjectController player in GameManager.singleton.Manager.GamePlayers)
        {
            GameObject GO = Instantiate(panelItemPrefab);
            PlayerRole role = GameManager.singleton.GetPlayerRole(player);
            GO.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.singleton.GetPlayerIcon(player);
            GO.transform.GetChild(1).GetComponent<Text>().text = player.playerName;
            GO.transform.GetChild(2).GetComponent<Image>().sprite = role.roleIcon;
            GO.transform.GetChild(3).GetComponent<Text>().text = role.roleName;
        }
    }
    public void LeaveLobby()
    { 
    
    }
    public void ReturnLobby()
    {

    }
}
