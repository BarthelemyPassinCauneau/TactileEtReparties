using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;

public class ParticipantsListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private ParticipantItem _participantItem = null;
    [SerializeField] private Transform _content = null;
    [SerializeField] private GameObject _startButton = null;
    [SerializeField] TMP_Text ProfTitle = null;
    [SerializeField] TMP_Text NbStudentsText = null;

    private Dictionary<Player, ParticipantItem> ParticipantsList = new Dictionary<Player, ParticipantItem>();

    private void Start() {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(player.IsMasterClient) {
                ProfTitle.text += player.NickName;
            } else {
                ParticipantItem participant = Instantiate(_participantItem, _content);
                ParticipantsList[player] = participant;
                participant.SetParticipantInfo(player.NickName);
            }
        }
        NbStudentsText.text = "Nb d'élèves : " + (PhotonNetwork.PlayerList.Length-1);
        _startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    // FONCTIONS SURCHARGEES FOURNIES PAR PHOTON
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ParticipantItem participant = Instantiate(_participantItem, _content);
        ParticipantsList[newPlayer] = participant;
        participant.SetParticipantInfo(newPlayer.NickName);
        NbStudentsText.text = "Nb d'élèves : " + (PhotonNetwork.PlayerList.Length-1);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(ParticipantsList[otherPlayer].gameObject);
        ParticipantsList.Remove(otherPlayer);
        NbStudentsText.text = "Nb d'élèves : " + (PhotonNetwork.PlayerList.Length-1);
    }
}
