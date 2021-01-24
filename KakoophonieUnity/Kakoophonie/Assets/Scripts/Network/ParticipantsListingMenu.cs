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
    [SerializeField] private GameObject _startButton = null;

    private Dictionary<int, ParticipantItem> ParticipantsList = new Dictionary<int, ParticipantItem>();

    private void Start() {
        RefreshList();
        _startButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ParticipantItem participant = Instantiate(_participantItem, _content);
        ParticipantsList[newPlayer.ActorNumber] = participant;
        participant.SetParticipantInfo(newPlayer.NickName);
    }

    public void RefreshList() {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(!ParticipantsList.ContainsKey(player.ActorNumber)) {
                ParticipantItem participant = Instantiate(_participantItem, _content);
                ParticipantsList[player.ActorNumber] = participant;
                participant.SetParticipantInfo(player.NickName);
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(ParticipantsList[otherPlayer.ActorNumber].gameObject);
        ParticipantsList.Remove(otherPlayer.ActorNumber);
    }
}
