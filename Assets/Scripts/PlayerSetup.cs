using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using System;

public class PlayerSetup : NetworkBehaviour
{
    public PlayerRole myRole;
    public GameObject uiPrefab;
    private GameObject uiGO;
    public Camera playerCamera;
    public TextMesh voteNumberText;
    public Sprite[] modelIcon;
    public bool voted = false;
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    [SerializeField]
    string localPlayerTagName = "LocalPlayer";
    [SerializeField]
    string localPlayerLayerName = "LocalPlayer";
    void Start()
    {
        myRole = GameManager.singleton.GetPlayerRole(GetComponent<PlayerObjectController>());
        voteNumberText.text = "";
    }
    private void Update()
    {
        if (isLocalPlayer)
        {
            SetPlayerCamera();
            if (uiGO == null)
            {
                uiGO = Instantiate(uiPrefab);
                uiGO.GetComponent<MyPlayerUI>().myPlayer = GetComponent<PlayerObjectController>();
            }
            if (transform.tag != localPlayerTagName)
            {
                SetLayerRecursively(gameObject, LayerMask.NameToLayer(localPlayerLayerName));
                SetTagRecursively(gameObject, localPlayerTagName);
                ActiveComponents(true);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                VotePlayer();
                voted = true;
            }
            if (Input.GetMouseButtonDown(0))
            {
                VoteRemovePlayer();
                voted = false;
            }
        }
        else 
        {
            SetLayerRecursively(gameObject, LayerMask.NameToLayer(remoteLayerName));
            ActiveComponents(false);
        }
        if (!GameManager.singleton.playerIcon.ContainsKey(GetComponent<PlayerObjectController>()))
        {
            if (myRole != null)
            {
                GameManager.singleton.playerIcon.Add(GetComponent<PlayerObjectController>(), modelIcon[GetComponent<PlayerObjectController>().PlayerCharacther]);
                uiGO.GetComponent<MyPlayerUI>().CreateRoleCastingPanel(myRole, modelIcon[GetComponent<PlayerObjectController>().PlayerCharacther]);
            }
        }
        voteNumberText.text = VoteManager.instance.GetVoteNumber(GetComponent<PlayerObjectController>()).ToString();
    }
    private void VoteRemovePlayer()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if (selection.tag == "Player")
            {
                VoteManager.instance.RemoveVotedPlayer(GetComponent<PlayerObjectController>());
            }
        }
    }
    public void VotePlayer()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if (selection.tag == "Player")
            {
                VoteManager.instance.AddVotedPlayer(selection.GetComponent<PlayerObjectController>(), GetComponent<PlayerObjectController>());
            }
        }
    }
    void SetTagRecursively(GameObject obj,string newTag)
    {
        obj.tag= newTag;

        foreach (Transform child in obj.transform)
        {
            SetTagRecursively(child.gameObject, newTag);
        }
    }
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    void ActiveComponents(bool state)
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = state;
        }
    }
    void SetPlayerCamera()
    {
        if (playerCamera.GetComponent<CameraSwitcher>() == null)
            return;

        if (playerCamera.GetComponent<CameraSwitcher>().playerCamera != null)
            return;

        playerCamera.GetComponent<CameraSwitcher>().SetPlayerCamera(playerCamera);
    }
}
