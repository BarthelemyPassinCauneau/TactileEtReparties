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
    public Groups group;

    public void SetGroupInfo(Groups group) {
        this.group = group;
        _name.text = "Groupe "+(group.ID+1);
        _correct.text = group.correct.ToString();
        _wrong.text = group.wrong.ToString();
        _nbPlayer.text = group.players.Count.ToString();
    }

    public void updateNumbers(int correct, int wrong, int nbPlayer){
        this._correct.text = correct.ToString();
        this._wrong.text = wrong.ToString();
        this._nbPlayer.text = nbPlayer.ToString();
    }

    public void updateColor(){
        
    }
}
