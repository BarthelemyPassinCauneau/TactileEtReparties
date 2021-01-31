using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

namespace Exe{
    public class PlayerList : MonoBehaviour
    {
        [SerializeField] private PlayerItem _participantItem = null;
        [SerializeField] private Transform _content = null;
        public List<Players> playerList = new List<Players>();
        public List<PlayerItem> PlayerItemList = new List<PlayerItem>();

        public void CreateList(){
            foreach(Players p in playerList){
                PlayerItem PI = Instantiate(_participantItem, _content);
                PI.SetPlayerInfo(p.player.NickName, p.answer);
            }
        }
        public void PlayerEnteredRoom()
        {
            //List updated was received before this function
            Players p = playerList[playerList.Count-1];
            PlayerItem PI = Instantiate(_participantItem, _content);
            PlayerItemList.Add(PI);
            PI.SetPlayerInfo(p.player.NickName, p.answer);
        }
        public void PlayerLeftRoom(Players p)
        {
            foreach(PlayerItem PI in PlayerItemList){
                if(PI.name == p.player.NickName){
                    Destroy(PI.gameObject);
                    PlayerItemList.Remove(PI);
                }
            }
        }
        public void ResetList(){
            foreach(PlayerItem PI in PlayerItemList){
                Destroy(PI.gameObject);
                PlayerItemList.Remove(PI);
            }
        }
        public void ApplyAnswerColor(Players p, Color c){
            foreach(PlayerItem PI in PlayerItemList){
                if(PI.name == p.player.NickName){
                    PI._answer.color = c;
                }
            }
        }
        public void ResetAnswerColor(){
            foreach(PlayerItem PI in PlayerItemList){
                PI.image.color = Color.black;
            }
        }
        public void ApplySelectColor(Players p){
            foreach(PlayerItem PI in PlayerItemList){
                if(PI.name == p.player.NickName){
                    PI.image.color = Color.blue;
                }
            }
        }
        public void ResetSelectColor(){
            foreach(PlayerItem PI in PlayerItemList){
                PI.image.color = Color.gray;
            }
        }
    }
}
