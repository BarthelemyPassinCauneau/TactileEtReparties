using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using System;

namespace Exe{
    public class PlayerList : MonoBehaviour
    {
        [SerializeField] private PlayerItem _participantItem = null;
        [SerializeField] private Transform _content = null;
        public List<Players> playerList = new List<Players>();
        public Dictionary<Players, PlayerItem> PlayerItemList = new Dictionary<Players, PlayerItem>();
        public UnmuteStudentEvent UnmuteStudentEvent = new UnmuteStudentEvent();

        public void CreateList(){
            foreach(Players p in playerList){
                PlayerItem PI = Instantiate(_participantItem, _content);
                PI.SetPlayerInfo(p);
                PI.HandClickedEvent.AddListener(OnClickHand);
                PlayerItemList[p] = (PI);
            }
        }
        public void PlayerEnteredRoom()
        {
            //List updated was received before this function
            Players p = playerList[playerList.Count-1];
            PlayerItem PI = Instantiate(_participantItem, _content);            
            PI.SetPlayerInfo(p);
            PlayerItemList[p] = PI;
        }
        public void PlayerLeftRoom(Players p)
        {
            Destroy(PlayerItemList[p].gameObject);
            PlayerItemList.Remove(p);
        }
        public void ResetList(){
            foreach(PlayerItem PI in PlayerItemList.Values){
                Destroy(PI.gameObject);
            }
            PlayerItemList.Clear();
        }
        public void ApplyAnswerColor(Players p, Color c){
            PlayerItemList[p]._answer.color = c;
        }
        public void ResetAnswerColor(){
            foreach(PlayerItem PI in PlayerItemList.Values){
                PI.image.color = Color.black;
            }
        }
        public void ApplySelectColor(Players p){
            PlayerItemList[p]._answer.color = Color.blue;
        }
        public void ResetSelectColor(){
            foreach(PlayerItem PI in PlayerItemList.Values){
                PI.image.color = Color.gray;
            }
        }

        public void RaiseHand(Player player) {
            foreach(Players p in playerList){
                if(player == p.player) {
                    PlayerItemList[p].RaiseHand();
                }
            }
        }

        public void OnClickHand(Player p) {
            UnmuteStudentEvent.Invoke(p);
        }
    }
}
public class UnmuteStudentEvent : UnityEvent<Player> {}
