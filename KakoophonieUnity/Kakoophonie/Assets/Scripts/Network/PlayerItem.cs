using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Exe;
using Photon.Realtime;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] public TMP_Text _name = null;
    [SerializeField] public TMP_Text _answer = null;
    [SerializeField] public Image image = null;
    [SerializeField] public Button hand = null;

    private Players player;
    public HandClickedEvent HandClickedEvent = new HandClickedEvent();

    public void SetPlayerInfo(Players player) {
        this.player = player;
        _name.text = player.player.NickName;
        _answer.text = player.answer;
    }

    public void addAnswer(string answer){
        this.player.answer = answer;
        _answer.text = answer;
    }

    public void RaiseHand() {
        hand.gameObject.SetActive(!hand.gameObject.activeSelf);
    }

    public void OnClickHand() {
        HandClickedEvent.Invoke(player.player);
    }
}

public class HandClickedEvent : UnityEvent<Player> {}
