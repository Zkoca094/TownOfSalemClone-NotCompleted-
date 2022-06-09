using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum SceneTime 
{
    Preparation,
    Morning,
    Night,
    Action
}
public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public List<PlayerRole> CreateRole = new List<PlayerRole>();
    public List<GameObject> HousePosition = new List<GameObject>();
    public Dictionary<GameObject, PlayerObjectController> playerHouse = new Dictionary<GameObject, PlayerObjectController>();
    public Dictionary<PlayerObjectController, PlayerRole> playerRole = new Dictionary<PlayerObjectController, PlayerRole>();
    public Dictionary<PlayerObjectController, PlayerObjectController> ActionToPlayer = new Dictionary<PlayerObjectController, PlayerObjectController>();
    public Dictionary<PlayerObjectController, Sprite> playerIcon = new Dictionary<PlayerObjectController, Sprite>();
    public int votingDuration, rolUseDuration, actionDuration, preparationDuration;
    public int minPlayer, maxPlayer;
    public SceneTime sceneTime;
    public CustomNetworkManager manager;
    public CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
                return manager;
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }

    }
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        DontDestroyOnLoad(this);

        votingDuration = 180;
        rolUseDuration = 15;

        manager = Manager;
        minPlayer = 4;
        maxPlayer = Manager.maxConnections;

        actionDuration = 15;
        preparationDuration = 10;
    }
    public bool CanStartGame()
    {
        int playerNumber = Manager.GamePlayers.Count;
        if (playerNumber < minPlayer && playerNumber > maxPlayer)
        {
            return false;
        }
        if (playerNumber < CreateRole.Count && playerNumber > CreateRole.Count)
        {
            return false;
        }      
        return true;
    }
    public PlayerObjectController GetPlayer(GameObject position)
    {
        return playerHouse[position];
    }
    public Sprite GetPlayerIcon(PlayerObjectController player)
    {
        return playerIcon[player];
    }
    public PlayerRole GetPlayerRole(PlayerObjectController player)
    {
        return playerRole[player];
    }
    public int GetRoleAmount(PlayerRole newRole)
    {
        int sayac = 0;
        for (int i = 0; i < CreateRole.Count; i++)
        {
            if (newRole == CreateRole[i])
            {
                sayac++;
            }
        }
        return sayac;
    }
    public int GetAllRoleNumber()
    {
        return CreateRole.Count;
    }
    public int GetRoleNumber(RoleType type)
    {
        int sayac = 0;
        foreach (PlayerRole item in CreateRole)
        {
            if (item.roleType == type)
            {
                sayac++;
            }
        }
        return sayac;
    }
    public GameObject GetPlayerHouse(PlayerObjectController Player)
    {
        GameObject found = null;
        foreach (KeyValuePair<GameObject,PlayerObjectController> item in playerHouse)
        {
            if (item.Value == Player)
                found = item.Key;
        }
        return found;
    }
    public PlayerObjectController GetOwnerOfTheHouse(GameObject house)
    {
        return playerHouse[house];
    }
    public void AddPlayerAction(PlayerObjectController localplayer, PlayerObjectController actionPlayer)
    {
        ActionToPlayer.Add(localplayer, actionPlayer);
    }

}
