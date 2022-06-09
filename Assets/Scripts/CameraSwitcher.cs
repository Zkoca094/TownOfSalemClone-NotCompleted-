using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraSwitcher : MonoBehaviour
{
    public Camera playerCamera;
    public Camera mainCamera;
    public Camera actionCamera;
    public SceneTime sceneTime;
    public void SetPlayerCamera(Camera newCamera)
    {
        playerCamera = newCamera;
    }
    public void SetMainCamera()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }
    public void SetActionCamera()
    {
        actionCamera = GameObject.FindWithTag("ActionCamera").GetComponent<Camera>();
    }
    void Update()
    {
        if (GameObject.FindWithTag("MainCamera") != null)
            SetMainCamera();

        if (GameObject.FindWithTag("ActionCamera") != null)
            SetActionCamera();

        if (playerCamera == null)
            return;

        if (GameManager.singleton != null)
        {
            sceneTime = GameManager.singleton.sceneTime;
            if (sceneTime == SceneTime.Morning)
            {
                playerCamera.enabled = true;
                playerCamera.GetComponent<SmoothMouseLook>().enabled = true;
                mainCamera.enabled = false;
                actionCamera.enabled = false;
            }
            if (sceneTime == SceneTime.Night)
            {
                playerCamera.enabled = false;
                playerCamera.GetComponent<SmoothMouseLook>().enabled = false;
                mainCamera.enabled = true;
                actionCamera.enabled = false;
            }
            if (sceneTime == SceneTime.Action)
            {
                playerCamera.enabled = false;
                playerCamera.GetComponent<SmoothMouseLook>().enabled = false;
                mainCamera.enabled = false;
                actionCamera.enabled = true;
            }
        }
    }
}
