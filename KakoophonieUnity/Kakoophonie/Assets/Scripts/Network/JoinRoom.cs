using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JoinRoom : MonoBehaviourPunCallbacks
{
    static private int index = 1;

    [SerializeField] private GameObject _waitingRoomCanvas = null;
    [SerializeField] private GameObject _menuCanvas = null;
    [SerializeField] private GameObject _startButton = null;
    [SerializeField] private ParticipantsListingMenu _listing = null;

    public void ButtonJoinRoom() {
        PhotonNetwork.NickName = "Eleve "+index;
        index++;
        PhotonNetwork.JoinRoom("Room test");
    }

    public override void OnJoinedRoom() {
        _waitingRoomCanvas.SetActive(true);
        _menuCanvas.SetActive(false);
        //_startButton.SetActive(false);
        _listing.RefreshList();
    }
}
