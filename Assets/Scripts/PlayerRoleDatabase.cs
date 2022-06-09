using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Database", menuName = "Player Role/Role Database")]
public class PlayerRoleDatabase : ScriptableObject
{
    public PlayerRole[] roleObjects;
    private void OnValidate()
    {
        for (int i = 0; i < roleObjects.Length; i++)
        {
            roleObjects[i].Id = i;
        }
    }
}
