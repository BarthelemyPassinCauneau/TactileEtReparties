using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _waitingRoomCanvas = null;
    [SerializeField] private GameObject _menuCanvas = null;
    [SerializeField] private ParticipantsListingMenu _listing = null;

    public void ButtonCreateRoom() {
        PhotonNetwork.NickName = "Professeur";
        PhotonNetwork.CreateRoom("Room test", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
        _waitingRoomCanvas.SetActive(true);
        _menuCanvas.SetActive(false);
        _listing.RefreshList();
    }
}
