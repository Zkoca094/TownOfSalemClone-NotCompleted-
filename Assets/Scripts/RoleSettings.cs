using System.Collections.Generic;
using UnityEngine;


public class RoleSettings : MonoBehaviour
{
    public GameObject addRolePrefab;
    public GameObject addRoleContent, innocentRoleContent, imposterRoleContent, neutralRoleContent;
    public PlayerRoleDatabase roleDatabase;
    public PlayerRole RandomInnocentRole, RandomImposterRole, RandomNeutralRole;
    public void Start()
    {
        CreateRoleListItem();
    }
    public void CreateRoleListItem()
    {
        for (int i = 0; i < roleDatabase.roleObjects.Length; i++)
        {
            GameObject roleItem = Instantiate(addRolePrefab, addRoleContent.transform);
            RoleItem itemScript = roleItem.GetComponent<RoleItem>();
            itemScript.role = roleDatabase.roleObjects[i];
            itemScript.SetAllRolePanelDesign();
            itemScript.roleSettings = this;
        }
    }
    public void AddingInnocentRole(PlayerRole newRole)
    {
        AddRole(innocentRoleContent.transform, newRole);
    }
    public void AddingImposterRole(PlayerRole newRole)
    {
        AddRole(imposterRoleContent.transform, newRole);
    }
    public void AddingNeutralRole(PlayerRole newRole)
    {
        AddRole(neutralRoleContent.transform, newRole);
    }
    public void AddRole(Transform content, PlayerRole newRole)
    {
        if (GameManager.singleton.CreateRole.Count + 1 > GameManager.singleton.maxPlayer)
            return;

        if (content.childCount > 0)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                if (content.GetChild(i).GetComponent<RoleItem>().role == newRole)
                {
                    Destroy(content.GetChild(i).gameObject);
                }
            }
        }
        GameObject roleItem = Instantiate(addRolePrefab, content);
        RoleItem itemScript = roleItem.GetComponent<RoleItem>();
        itemScript.role = newRole;
        itemScript.roleSettings = this;
        GameManager.singleton.CreateRole.Add(newRole);
        itemScript.SetGameRolePanelDesign();
    }
    public void AddingRandomInnocentRole()
    {
        AddRole(innocentRoleContent.transform, RandomInnocentRole);
    }
    public void AddingRandomImposterRole()
    {
        AddRole(imposterRoleContent.transform, RandomImposterRole);
    }
    public void AddingRandomNeutralRole()
    {
        AddRole(neutralRoleContent.transform, RandomNeutralRole);
    }
}
