using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ChangeScene : MonoBehaviour {

    public void StartGame() {
        PhotonNetwork.LoadLevel("Exercise");
    }
    public void LoadMenu(){
        SceneManager.LoadScene("Menu");
    }
}
