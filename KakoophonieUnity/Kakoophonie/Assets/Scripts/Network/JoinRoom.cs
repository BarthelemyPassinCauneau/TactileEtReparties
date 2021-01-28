using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class JoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _name = null;

    public void ButtonJoinRoom() {
        PhotonNetwork.NickName = _name.text;
        PhotonNetwork.JoinRoom("Room test");
    }

    //OnJoinedRoom implementé dans CreateRoom.cs
}
