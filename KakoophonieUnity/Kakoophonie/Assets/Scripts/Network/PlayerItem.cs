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
    [SerializeField] public TMP_Text _score = null;
    [SerializeField] public TMP_Text _averageTime = null;
    [SerializeField] public Image image = null;
    [SerializeField] public Button hand = null;
    [SerializeField] public Button mic = null;
    [SerializeField] public Button muteMic = null;

    public Players player;
    public PlayerListHandClickedEvent PlayerListHandClickedEvent = new PlayerListHandClickedEvent();
    public PlayerListMuteClickedEvent PlayerListMuteClickedEvent = new PlayerListMuteClickedEvent();
    public PlayerListUnMuteClickedEvent PlayerListUnMuteClickedEvent = new PlayerListUnMuteClickedEvent();

    public void SetPlayerInfo(Players player) {
        this.player = player;
        _name.text = player.player.NickName;
        _answer.text = player.answer;
        if(player.questionsAnswered > 0) {
            _score.text = player.correctAnswers + "/" + player.questionsAnswered;
            if(player.getAverageTime() != 0) {
                _averageTime.text = player.getAverageTime() + " s";
            }
            SetScoreColor();
        }
    }

    public void SetScoreInfo(Players player) {
        this.player = player;
        _score.text = player.correctAnswers + "/" + player.questionsAnswered;
        if(player.getAverageTime() != 0) {
            _averageTime.text = player.getAverageTime() + " s";
        }
        SetScoreColor();
    }

    private void SetScoreColor() {
        if(player.questionsAnswered > 0) {
            if(player.correctAnswers == 0) _score.color = Color.red;
            else if(player.correctAnswers == player.questionsAnswered) _score.color = new Color(1, 0.79f, 0);
            else if((float)player.correctAnswers/(float)player.questionsAnswered < 0.5f) _score.color = Color.red;
            else if((float)player.correctAnswers/(float)player.questionsAnswered >= 0.5f) _score.color = Color.green;
        } else {
            _score.color  = Color.black;
        }
    }

    public void addAnswer(string answer){
        this.player.answer = answer;
        _answer.text = answer;
    }

    //Fonction appelée par photon quand l'étudiant lève ou baisse la main, depuis Eleve.Exe
    public void RaiseHand() {
        if(hand.gameObject.activeSelf || mic.gameObject.activeSelf || muteMic.gameObject.activeSelf ) {
            hand.gameObject.SetActive(false);
            mic.gameObject.SetActive(false);
            muteMic.gameObject.SetActive(false);
        } else {
            hand.gameObject.SetActive(true);
        }
    }

    /**
     * Fonctions des boutons
     */
    public void OnClickHand() {
        PlayerListHandClickedEvent.Invoke(player.player);
        hand.gameObject.SetActive(false);
        mic.gameObject.SetActive(true);
    }

    public void OnClickMute() {
        PlayerListMuteClickedEvent.Invoke(player.player);
        mic.gameObject.SetActive(false);
        muteMic.gameObject.SetActive(true);
    }

    public void OnClickUnMute() {
        PlayerListUnMuteClickedEvent.Invoke(player.player);
        mic.gameObject.SetActive(true);
        muteMic.gameObject.SetActive(false);
    }
}

public class PlayerListHandClickedEvent : UnityEvent<Player> {}
public class PlayerListMuteClickedEvent : UnityEvent<Player> {}
public class PlayerListUnMuteClickedEvent : UnityEvent<Player> {}
