using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using System;

namespace Exe{
    public class GroupList : MonoBehaviour
    {
        [SerializeField] private GroupItem _participantItem = null;
        [SerializeField] private Transform _content = null;
        public List<Players> playerList = new List<Players>();
        public Dictionary<Players, GroupItem> PlayerItemList = new Dictionary<Players, GroupItem>();
        public ProfExeHandClickedEvent ProfExeHandClickedEvent = new ProfExeHandClickedEvent();
        public ProfExeMuteClickedEvent ProfExeMuteClickedEvent = new ProfExeMuteClickedEvent();
        public ProfExeUnMuteClickedEvent ProfExeUnMuteClickedEvent = new ProfExeUnMuteClickedEvent();

        public void CreateList(){
            foreach(Players p in playerList){
                GroupItem PI = Instantiate(_participantItem, _content);
                PI.SetPlayerInfo(p);
                PlayerItemList[p] = (PI);
            }
        }
        public void PlayerEnteredRoom()
        {
            //List updated was received before this function
            Players p = playerList[playerList.Count-1];
            GroupItem PI = Instantiate(_participantItem, _content);            
            PI.SetPlayerInfo(p);
            PlayerItemList[p] = PI;
        }
        public void PlayerLeftRoom(Players p)
        {
            Destroy(PlayerItemList[p].gameObject);
            PlayerItemList.Remove(p);
        }
        public void ResetList(){
            foreach(GroupItem PI in PlayerItemList.Values){
                Destroy(PI.gameObject);
            }
            PlayerItemList.Clear();
        }
        public void ApplyAnswerColor(Players p, Color c){
            //PlayerItemList[p]._answer.color = c;
        }
        public void ResetAnswerColor(){
            foreach(GroupItem PI in PlayerItemList.Values){
               //PI.image.color = Color.black;
            }
        }
        public void ApplySelectColor(Players p){
            //PlayerItemList[p]._answer.color = Color.blue;
        }
        public void ResetSelectColor(){
            foreach(GroupItem PI in PlayerItemList.Values){
                //PI.image.color = Color.gray;
            }
        }

    }
}
