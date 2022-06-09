using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySettings : MonoBehaviour
{
    public Text votingDurationText, rolUseDurationText,nameText;
    public int votingDuration, rolUseDuration;
    public Transform charactherContent;
    public int charactherIndex = 0;
    public Button nextButton, prevButton;
    bool readyState = false;
    public Text innocentHeaderText, imposterHeaderText, neutralHeaderText, gamePlayerHeaderText;

    void Start()
    {
        votingDuration = GameManager.singleton.votingDuration;
        rolUseDuration = GameManager.singleton.rolUseDuration;
        votingDurationText.text = votingDuration.ToString();
        rolUseDurationText.text = rolUseDuration.ToString();
        PlayerPrefs.DeleteAll();
        charactherIndex = PlayerPrefs.GetInt("charactherIndex", 0);
        SetActive(charactherIndex);
    }
    public void ReadyPlayer()
    {
        readyState = !readyState;
        nextButton.enabled = !readyState;
        prevButton.enabled = !readyState;
    }
    private void Update()
    {
        innocentHeaderText.text = string.Format("INNOCENTS ROLE ({0})", GameManager.singleton.GetRoleNumber(RoleType.Innocent));
        imposterHeaderText.text = string.Format("IMPOSTERS  ROLE ({0})", GameManager.singleton.GetRoleNumber(RoleType.Imposter));
        neutralHeaderText.text = string.Format("NEUTRALS  ROLE ({0})", GameManager.singleton.GetRoleNumber(RoleType.Neutral));
        gamePlayerHeaderText.text = string.Format("GAME ROLES ({0})", GameManager.singleton.GetAllRoleNumber());
    }
    public void AddVoteDuration()
    {
        if (votingDuration < 360)
            votingDuration += 30;
        votingDurationText.text = votingDuration.ToString();
        GameManager.singleton.votingDuration = votingDuration;
    }
    public void LessVoteDuration()
    {
        if (votingDuration > 150)
            votingDuration -= 30;
        votingDurationText.text = votingDuration.ToString();
        GameManager.singleton.votingDuration = votingDuration;
    }
    public void AddRoleUseDuration()
    {
        if (rolUseDuration < 60)
            rolUseDuration += 5;
        rolUseDurationText.text = rolUseDuration.ToString();
        GameManager.singleton.rolUseDuration = rolUseDuration;
    }
    public void LessRoleUseDuration()
    {
        if (rolUseDuration > 10)
            rolUseDuration -= 5;
        rolUseDurationText.text = rolUseDuration.ToString();
        GameManager.singleton.rolUseDuration = rolUseDuration;
    }
    public void SetActive(int index)
    {
        for (int i = 0; i < charactherContent.childCount; i++)
        {
            charactherContent.GetChild(i).gameObject.SetActive(false);
        }
        charactherContent.GetChild(index).gameObject.SetActive(true);
        nameText.text = charactherContent.GetChild(index).name;
        PlayerPrefs.SetInt("charactherIndex", index);
    }
    public void NextColor()
    {
        if (charactherIndex < charactherContent.childCount - 1)
        {
            charactherIndex++;
            SetActive(charactherIndex);
        }
    }
    public void PrevColor()
    {
        if (charactherIndex > 0)
        {
            charactherIndex--;
            SetActive(charactherIndex);
        }
    }
}
