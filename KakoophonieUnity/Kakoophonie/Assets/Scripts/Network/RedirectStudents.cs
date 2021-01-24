using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class RedirectStudents : MonoBehaviour
{
    void Start()
    {
        if(!PhotonNetwork.IsMasterClient) {
            SceneManager.LoadScene("Eleve");
        }
    }
}
