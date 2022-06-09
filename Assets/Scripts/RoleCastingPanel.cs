using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoleCastingPanel : MonoBehaviour
{
    public Transform playerIcon;
    public Text playerRoleName;
    public Image playerRoleIcon;
    public void SetRoleCasting(PlayerRole playerRole, Sprite modelIcon)
    {
        for (int i = 0; i < playerIcon.childCount; i++)
        {
            playerIcon.GetChild(i).GetComponent<Image>().sprite = modelIcon;
        }
        if (playerRole != null)
        {
            playerRoleIcon.sprite = playerRole.roleIcon;

            if (playerRole.roleType == RoleType.Imposter)
                playerRoleName.color = Color.red;
            if (playerRole.roleType == RoleType.Innocent)
                playerRoleName.color = Color.green;
            if (playerRole.roleType == RoleType.Neutral)
                playerRoleName.color = Color.blue;

            playerRoleName.text = playerRole.roleName;
        }
        StartCoroutine(StartMorning());
    }

    public IEnumerator StartMorning()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        new WaitForSeconds(2);
        GameManager.singleton.sceneTime = SceneTime.Morning;
    }
}
