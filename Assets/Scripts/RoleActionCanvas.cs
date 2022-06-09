using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoleActionCanvas : MonoBehaviour
{
    public GameObject actionButtonPrefab;
    public GameObject[] buttonGO;
    public void SetPanel(int length, PlayerObjectController localPlayer, PlayerObjectController chosenPlayer)
    {
        buttonGO = new GameObject[length];
        if (buttonGO.Length >= 0)
        {
            for (int i = 0; i < length; i++)
            {
                buttonGO[i] = Instantiate(actionButtonPrefab, transform.GetChild(0));
                Button newButton = buttonGO[i].GetComponent<Button>();
                newButton.onClick.AddListener(delegate { ButtonClick(localPlayer, chosenPlayer); });
            }
        }
    }
    public void ButtonClick(PlayerObjectController localPlayer, PlayerObjectController chosenPlayer)
    {
        GameObject house = GameManager.singleton.GetPlayerHouse(chosenPlayer);
        house.transform.GetChild(1).GetComponent<Outline>().enabled = true;
        Debug.Log(localPlayer.playerName + " " + chosenPlayer.playerName);
    }
}
