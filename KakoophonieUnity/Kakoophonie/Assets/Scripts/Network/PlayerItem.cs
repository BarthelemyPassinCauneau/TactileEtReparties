using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] public TMP_Text _name = null;
    [SerializeField] public TMP_Text _answer = null;
    [SerializeField] public Image image = null;

    public void SetPlayerInfo(string name, string answer) {
        _name.text = name;
        _answer.text = answer;
    }

    public void addAnswer(string answer){
        _answer.text = answer;
    }
}
