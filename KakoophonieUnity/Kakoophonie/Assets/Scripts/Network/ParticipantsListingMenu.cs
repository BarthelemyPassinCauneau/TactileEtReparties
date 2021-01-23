using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class ParticipantsListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private ParticipantItem _participantItem = null;
    [SerializeField] private Transform _content = null;

    private Dictionary<string, ParticipantItem> ParticipantsList = new Dictionary<string, ParticipantItem>();
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ParticipantItem participant = Instantiate(_participantItem, _content);
        ParticipantsList[newPlayer.NickName] = participant;
        participant.SetParticipantInfo(newPlayer.NickName);
    }

    public void RefreshList() {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(!ParticipantsList.ContainsKey(player.NickName)) {
                ParticipantItem participant = Instantiate(_participantItem, _content);
                ParticipantsList[player.NickName] = participant;
                participant.SetParticipantInfo(player.NickName);
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(ParticipantsList[otherPlayer.NickName].gameObject);
        ParticipantsList.Remove(otherPlayer.NickName);
    }
}
