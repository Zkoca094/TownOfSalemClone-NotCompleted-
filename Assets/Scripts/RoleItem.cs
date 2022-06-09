using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoleItem : MonoBehaviour
{
    public Text roleName;
    public Image roleIcon;
    public Button AddButtonPrefab,LessButtonPrefab;
    public PlayerRole role;
    public Color innocentRoleColor, imposterRoleColor, neutralRoleColor;
    public RoleSettings roleSettings;
    public GameObject tooltipPrefab;
    private GameObject toolTipCanvas,toolTip;
    public void SetAllRolePanelDesign()
    {
        if (role != null)
        {
            if (role.roleType == RoleType.Imposter)
            {
                transform.GetComponent<Image>().color = imposterRoleColor;
            }
            if (role.roleType == RoleType.Innocent)
            {
                transform.GetComponent<Image>().color = innocentRoleColor;
            }
            if (role.roleType == RoleType.Neutral)
            {
                transform.GetComponent<Image>().color = neutralRoleColor;
            }
            roleName.text = role.roleName;
            GameObject addButtonGO = Instantiate(AddButtonPrefab, transform).gameObject;
            addButtonGO.GetComponent<Image>().color = Color.gray;
            addButtonGO.GetComponent<Button>().onClick.AddListener(AddingRole);
        }
    }
    public void AddingRole()
    {
        if (role != null)
        {
            if (role.roleType == RoleType.Imposter)
            {
                roleSettings.AddingImposterRole(role);
            }
            if (role.roleType == RoleType.Innocent)
            {
                roleSettings.AddingInnocentRole(role);
            }
            if (role.roleType == RoleType.Neutral)
            {
                roleSettings.AddingNeutralRole(role);
            }
        }
    }
    public void SetGameRolePanelDesign()
    {
        if (role != null)
        {
            if (role.roleType == RoleType.Imposter)
            {
                transform.GetComponent<Image>().color = imposterRoleColor;
            }
            if (role.roleType == RoleType.Innocent)
            {
                transform.GetComponent<Image>().color = innocentRoleColor;
            }
            if (role.roleType == RoleType.Neutral)
            {
                transform.GetComponent<Image>().color = neutralRoleColor;
            }
            roleName.text = string.Format("{0} ({1})", role.roleName , GameManager.singleton.GetRoleAmount(role));
            GameObject lessButtonGO = Instantiate(LessButtonPrefab, transform).gameObject;
            lessButtonGO.GetComponent<Image>().color = Color.red;
            lessButtonGO.GetComponent<Button>().onClick.AddListener(RemoveRole);
        }
    }
    public void RemoveRole()
    {
        if (GameManager.singleton.GetRoleAmount(role) > 0)
        {
            GameManager.singleton.CreateRole.Remove(role);
            roleName.text = string.Format("{0} ({1})", role.roleName, GameManager.singleton.GetRoleAmount(role));
            if (GameManager.singleton.GetRoleAmount(role) <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    public void Enter()
    {
        if (toolTipCanvas == null)
        {
            toolTipCanvas = Instantiate(tooltipPrefab);
            toolTip = toolTipCanvas.transform.GetChild(0).gameObject;
            Vector3 anchoredPosition = Input.mousePosition + new Vector3(-40, 0, 0) / toolTipCanvas.transform.localScale.x;
            toolTip.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
            SetToolTip();
        }
    }
    public void Exit()
    {
        Destroy(toolTipCanvas);
        toolTipCanvas = null;
    }
    public void SetToolTip()
    {
        toolTip.transform.GetChild(0).GetComponent<Image>().sprite = role.roleIcon;
        toolTip.transform.GetChild(1).GetComponent<Text>().text = role.name;
        toolTip.transform.GetChild(2).GetComponent<Text>().text = role.description;
    }
}
