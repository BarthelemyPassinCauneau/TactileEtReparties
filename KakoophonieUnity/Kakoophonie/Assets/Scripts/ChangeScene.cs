using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
    public void LoadProf(){
        SceneManager.LoadScene("Professeur");
    }

    public void LoadEleve(){
        SceneManager.LoadScene("Eleve");
    }

    public void LoadMenu(){
        SceneManager.LoadScene("Menu");
    }
}
