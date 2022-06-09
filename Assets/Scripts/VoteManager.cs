using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteManager : MonoBehaviour
{
    public static VoteManager instance;
    public List<PlayerObjectController> skipVote = new List<PlayerObjectController>();
    public Dictionary<PlayerObjectController, PlayerObjectController> VotingPlayer = new Dictionary<PlayerObjectController, PlayerObjectController>();
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public int GetVoteNumber(PlayerObjectController Player)
    {
        int sayac = 0;
        foreach (PlayerObjectController player in VotingPlayer.Values)
        {
            if (player == Player)
            {
                sayac++;
            }
        }
        return sayac;
    }
    public PlayerObjectController GetLocalPlayer()
    {
        return GameObject.Find("LocalGamePlayer").GetComponent<PlayerObjectController>();
    }
    public void AddVotedPlayer(PlayerObjectController votedPlayer,PlayerObjectController player)
    {
        if (!VotingPlayer.ContainsKey(player))
        {
            VotingPlayer.Add(player, votedPlayer);
        }
    }
    public void RemoveVotedPlayer(PlayerObjectController player)
    {
        if (VotingPlayer.ContainsKey(player))
        {
            VotingPlayer.Remove(player);
        }
    }
}
