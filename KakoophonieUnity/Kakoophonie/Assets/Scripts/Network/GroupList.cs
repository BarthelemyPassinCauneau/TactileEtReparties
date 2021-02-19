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
        [SerializeField] private Color selected;
        public List<Groups> groupList = new List<Groups>();
        public Dictionary<Groups, GroupItem> GroupItemList = new Dictionary<Groups, GroupItem>();
        private Color unselected;

        private void Start() {
            unselected = _participantItem.image.color;
        }

        public void CreateList(){
            foreach(Groups g in groupList){
                GroupItem GI = Instantiate(_participantItem, _content);
                GI.SetGroupInfo(g);
                GroupItemList[g] = (GI);
            }
        }

        public void ResetList(){
            foreach(GroupItem GI in GroupItemList.Values){
                Destroy(GI.gameObject);
            }
            GroupItemList.Clear();
        }

        public void AddGroup(Groups g)
        {
            GroupItem GI = Instantiate(_participantItem, _content);            
            GI.SetGroupInfo(g);
            GroupItemList[g] = GI;
        }
        public void SelectedColor(Groups g){
            foreach(GroupItem GI in GroupItemList.Values){
                if (GI.group == g){
                    GI.image.color = selected;
                }
            }
        }
        public void ResetSelectColor(){
            foreach(GroupItem GI in GroupItemList.Values){
                GI.image.color = unselected;
            }
        }

    }
}
