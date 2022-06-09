using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoleType
{
    Innocent,
    Imposter,
    Neutral
}
[CreateAssetMenu(fileName = "New Role", menuName = "Player Role/Role Object")]
public class PlayerRole : ScriptableObject
{
    public int Id;
    public string roleName;
    public Sprite roleIcon;
    public RoleType roleType;
    public bool killing;
   [TextArea] public string description;
}
