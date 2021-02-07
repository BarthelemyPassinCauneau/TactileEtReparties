using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class TestConnect : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _text = null;
    [SerializeField] Button teacher = null;
    [SerializeField] Button student = null;

    private void Start() {
        PhotonNetwork.ConnectUsingSettings();
        teacher.interactable = false;
        student.interactable = false;
    }

    public override void OnConnectedToMaster() {
        _text.text = "Connecté !";
        _text.color = new Color(0, 255, 0);
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        PhotonNetwork.AutomaticallySyncScene = true;
        teacher.interactable = true;
        student.interactable = true;
    }
}
