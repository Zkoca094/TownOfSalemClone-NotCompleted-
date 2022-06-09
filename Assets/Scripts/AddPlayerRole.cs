using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayerRole : MonoBehaviour
{
    public List<PlayerRole> PlayerRoles = new List<PlayerRole>();
    public List<PlayerRole> randomRoleChosen = new List<PlayerRole>();
    public PlayerRoleDatabase roleDatabase;
    public void Awake()
    {
        RoleCasting();
    }
    public bool CheckingRandomRole(PlayerRole role)
    {
        if (role.Id == -1)
            return true;
        else
            return false;
    }
    public void CreatePlayerRole()
    {
        foreach (PlayerRole Role in GameManager.singleton.CreateRole)
        {
            if (CheckingRandomRole(Role))
            {
                randomRoleChosen.Clear();
                foreach (PlayerRole item in roleDatabase.roleObjects)
                {
                    if (item.roleType == Role.roleType)
                    {
                        randomRoleChosen.Add(item);
                    }
                }
                int rndNumber = Random.Range(0, randomRoleChosen.Count);
                PlayerRole newRole = randomRoleChosen[rndNumber];
                PlayerRoles.Add(newRole);
            }
            else 
            {
                PlayerRoles.Add(Role);
            }
        }
    }
    public PlayerRole ChoseRole()
    {
        var rnd = Random.Range(0, PlayerRoles.Count);
        Debug.Log(PlayerRoles[rnd]);
        return PlayerRoles[rnd];
    }
    public void RoleCasting()
    {
        if (GameManager.singleton == null)
            return;

        CreatePlayerRole();
        foreach (PlayerObjectController player in GameManager.singleton.Manager.GamePlayers)
        {
            PlayerRole role = ChoseRole();
            GameManager.singleton.playerRole.Add(player, role);
        }
    }
}
