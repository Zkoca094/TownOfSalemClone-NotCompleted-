using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoleInfo : MonoBehaviour
{
    public Image roleIcon;
    public PlayerRole role;

    public void SetRoleDesign(PlayerRole newRole)
    {
        role = newRole;
        roleIcon.sprite = newRole.roleIcon;
    }
}
