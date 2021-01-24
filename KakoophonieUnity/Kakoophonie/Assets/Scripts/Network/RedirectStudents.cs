using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class RedirectStudents : MonoBehaviour
{
    [SerializeField] Canvas Professor;
    [SerializeField] Canvas Student;
    void Start()
    {
        if(!PhotonNetwork.IsMasterClient) {
            Professor.gameObject.SetActive(false);
        } else {
            Student.gameObject.SetActive(false);
        }
    }
}
