using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class TestConnect : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _text = null;
    private void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Photon connected");
        _text.text = "Connecté !";
        _text.color = new Color(0, 255, 0);
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
}
