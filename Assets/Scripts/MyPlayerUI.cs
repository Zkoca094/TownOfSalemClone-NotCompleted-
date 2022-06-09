using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MyPlayerUI : MonoBehaviour
{
    public PlayerObjectController myPlayer;
    public Transform roleContent;
    public GameObject roleItemPrefab;
    private GameObject roleCastingGO;
    public GameObject logPanel;
    public GameObject roleCastingPanelPrefab;
    public GameObject sunIcon, moonIcon;
    public GameObject tooltipPrefab;
    private GameObject toolTipCanvas, toolTip;
    public Text timeText, skipVotedText;
    public Button skipButton;
    public float voteTime, roleUseTime,actionTime, preparationDuration;
    private bool state = false;
    public SceneTime sceneTime;
    void Start()
    {
        voteTime = GameManager.singleton.votingDuration;
        roleUseTime = GameManager.singleton.rolUseDuration;
        actionTime = GameManager.singleton.actionDuration;
        preparationDuration = GameManager.singleton.preparationDuration;
        timeText.text = "";
        skipVotedText.text = string.Format("SKIP ({0})", VoteManager.instance.skipVote.Count);
        skipButton.gameObject.SetActive(false);
        if (GameObject.Find("GameManager") != null)
        {
            foreach (PlayerRole role in GameManager.singleton.CreateRole)
            {
                GameObject roleItemGO = Instantiate(roleItemPrefab, roleContent);
                RoleInfo script = roleItemGO.GetComponent<RoleInfo>();
                script.SetRoleDesign(role);
            }
        }
    }
    void Update()
    {
        if (GameObject.Find("GameManager") != null)
        {
            SetTimer();
            if (GameManager.singleton.sceneTime == SceneTime.Night)
                FindHouse();
        }
    }
    public void SetTimer()
    {
        sceneTime = GameManager.singleton.sceneTime;
        if (sceneTime == SceneTime.Preparation)
        {
            if (preparationDuration > 0)
                preparationDuration -= Time.deltaTime;
            timeText.text = preparationDuration.ToString("n0");
            moonIcon.SetActive(false);
            sunIcon.SetActive(true);
        }
        if (sceneTime == SceneTime.Night)
        {
            if (roleUseTime > 0)
                roleUseTime -= Time.deltaTime;
            timeText.text = roleUseTime.ToString("n0");
            moonIcon.SetActive(true);
            sunIcon.SetActive(false);
        }
        if (sceneTime == SceneTime.Morning)
        {
            if (voteTime > 0)
                voteTime -= Time.deltaTime;
            timeText.text = voteTime.ToString("n0");
            moonIcon.SetActive(false);
            sunIcon.SetActive(true);
            VoteManager.instance.skipVote.Clear();
        }
        if (sceneTime == SceneTime.Action)
        {
            if (actionTime > 0)
                actionTime -= Time.deltaTime;
            timeText.text = actionTime.ToString("n0");
            moonIcon.SetActive(true);
            sunIcon.SetActive(false);
        }
        if (voteTime <= 0)
        {
            GameManager.singleton.sceneTime = SceneTime.Night;
            voteTime = GameManager.singleton.votingDuration;
        }

        if (roleUseTime <= 0)
        {
            roleUseTime = GameManager.singleton.rolUseDuration;
            GameManager.singleton.sceneTime = SceneTime.Action;
        }
        if (actionTime <= 0)
        {
            GameManager.singleton.sceneTime = SceneTime.Preparation;
            actionTime = GameManager.singleton.actionDuration;
        }
        if (preparationDuration <= 0)
        {
            GameManager.singleton.sceneTime = SceneTime.Morning;
            preparationDuration = GameManager.singleton.preparationDuration;
        }
        if (GameManager.singleton.sceneTime == SceneTime.Morning)
            skipButton.gameObject.SetActive(true);
        else
            skipButton.gameObject.SetActive(false);
    }
    public void CreateRoleCastingPanel(PlayerRole playerRole, Sprite modelIcon)
    {
        if (roleCastingGO == null)
        {
            roleCastingGO = Instantiate(roleCastingPanelPrefab, transform);
            roleCastingGO.GetComponent<RoleCastingPanel>().SetRoleCasting(playerRole, modelIcon);
        }
    }
    public void OpenClose()
    {
        logPanel.SetActive(!logPanel.activeSelf);
    }
    public void SkipButton()
    {
        state = !state;
        if (state)
        {
            VoteManager.instance.skipVote.Add(myPlayer);
        }
        else
        {
            VoteManager.instance.skipVote.Remove(myPlayer);
        }
        skipVotedText.text = string.Format("SKIP ({0})", VoteManager.instance.skipVote.Count);
        if (VoteManager.instance.skipVote.Count == GameManager.singleton.maxPlayer)
        {
            GameManager.singleton.sceneTime = SceneTime.Night;
            voteTime = GameManager.singleton.votingDuration;
        }
    }
    public void FindHouse()
    {
        RaycastHit hit;
        Ray ray = myPlayer.GetComponent<PlayerSetup>().playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Transform selection = hit.transform;
            if (selection.tag == "PlayerHouse")
            {
                PlayerObjectController chosenPlayer = GameManager.singleton.GetOwnerOfTheHouse(selection.gameObject);
                if (chosenPlayer != null)
                {
                    if (chosenPlayer == myPlayer)
                        return;
                    EnterHouse(chosenPlayer);
                }
            }
            else
                ExitHouse();
        }
    }
    public void EnterHouse(PlayerObjectController chosenPlayer)
    {
        if (toolTipCanvas == null)
        {
            toolTipCanvas = Instantiate(tooltipPrefab);
            toolTip = toolTipCanvas.transform.GetChild(0).gameObject;
            Vector3 anchoredPosition = Input.mousePosition / toolTipCanvas.transform.localScale.x;
            toolTip.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
            toolTipCanvas.GetComponent<RoleActionCanvas>().SetPanel(1, myPlayer, chosenPlayer);
        }
    }
    public void ExitHouse()
    {
        if (toolTipCanvas != null)
        {
            Destroy(toolTipCanvas);
            toolTipCanvas = null;
        }
    }
}
