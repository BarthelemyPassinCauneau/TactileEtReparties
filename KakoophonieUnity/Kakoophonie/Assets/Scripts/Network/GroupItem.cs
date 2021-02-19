using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Exe;
using Photon.Realtime;

public class GroupItem : MonoBehaviour
{
    [SerializeField] public TMP_Text _name = null;
    [SerializeField] public TMP_Text _correct = null;
    [SerializeField] public TMP_Text _wrong = null;
    [SerializeField] public TMP_Text _nbPlayer = null;
    [SerializeField] public Image image = null;
    public Players player;

    public void SetPlayerInfo(Players player) {
        this.player = player;
        _name.text = player.player.NickName;
        _correct.text = player.answer;
        _wrong.text = player.answer;
        _nbPlayer.text = player.answer;
    }

    public void addAnswer(string answer){
        this.player.answer = answer;
        _correct.text = answer;
    }
}
