using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _waitingRoomCanvas = null;
    [SerializeField] private GameObject _menuCanvas = null;
    [SerializeField] private TMP_InputField _name = null;

    public void ButtonCreateRoom() {
        PhotonNetwork.NickName = _name.text;
        PhotonNetwork.CreateRoom("Room test", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
        _waitingRoomCanvas.SetActive(true);
        _menuCanvas.SetActive(false);
    }
}
