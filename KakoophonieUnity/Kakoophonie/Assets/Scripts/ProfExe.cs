using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity; 

namespace Exe{
    public class ProfExe : MonoBehaviourPunCallbacks {
        [SerializeField] TMP_Dropdown ChooseKey = null;
        [SerializeField] TMP_Dropdown ChooseNote = null;
        [SerializeField] TMP_Text LabelKey = null;
        [SerializeField] TMP_Text LabelNote = null;
        [SerializeField] Image image = null;
        [SerializeField] TMP_Text feedback = null;
        [SerializeField] TMP_Text title = null;
        [SerializeField] TMP_Text groupLabel = null;
        [SerializeField] VoiceConnection voiceConnection = null;
        [SerializeField] Button speakToGroupButton = null;
        [SerializeField] Button speakToClassButton = null;
        [SerializeField] Button privateCallButton = null;
        [SerializeField] PlayerList playerList = null;
        [SerializeField] GroupList groupList = null;
        [SerializeField] GameObject groupInfo = null;
        [SerializeField] GameObject groupFrame = null;
        [SerializeField] Button addGroup = null;
        [SerializeField] List<TMP_Text> wrong;
        [SerializeField] List<TMP_Text> correct;
        [SerializeField] List<TMP_Text> count;
        [SerializeField] Button transferButton;
        List<string> imagePath = new List<string>();
        List<string> correctAnswer = new List<string>();
        List<TMPro.TMP_Dropdown.OptionData> key = new List<TMP_Dropdown.OptionData>();
        List<TMPro.TMP_Dropdown.OptionData> note = new List<TMP_Dropdown.OptionData>();
        public List<Players> selected = new List<Players>();
        List<Players> studentsInPrivateCall = new List<Players>();
        List<Groups> group = new List<Groups>();
        int currentGroup = 0;
        public Groups currentGroupSelected = null;

        void Start()
        {
            InitComp();
            InitDropdown();
            GetStudentList();
            groupList.groupList = group;
            groupList.CreateList();
            playerList.playerList = group[currentGroup].players;
            playerList.ProfExeHandClickedEvent.AddListener(StudentSpeakToGroup);
            playerList.ProfExeMuteClickedEvent.AddListener(MuteStudent);
            playerList.ProfExeUnMuteClickedEvent.AddListener(UnMuteStudent);
            playerList.CreateList();
            DisplayStudents();
            ChooseKey.onValueChanged.AddListener(delegate {DropdownValueChanged(ChooseKey.GetComponent<Dropdown>());});
            ChooseNote.onValueChanged.AddListener(delegate {DropdownValueChanged(ChooseNote.GetComponent<Dropdown>());});
            image.sprite = Resources.Load<Sprite>("Images/Sol/Do");
        }

        private void Update() {
            if(currentGroupSelected == null || currentGroupSelected.ID == currentGroup || selected.Count < 1){
                transferButton.interactable = false;
            } else {
                transferButton.interactable = true;
            }
        }

        void DropdownValueChanged(Dropdown dd){
            imagePath[currentGroup] = "Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text;
            image.sprite = Resources.Load<Sprite>(imagePath[currentGroup]);
        }

        void InitComp(){
            title.text = "Professeur "+ PhotonNetwork.NickName;
            groupLabel.text = "Groupe actuel : \nGroupe "+(currentGroup+1);
            imagePath.Add("Images/Sol/Do");
            correctAnswer.Add("Do");
            key.Add(null);
            note.Add(null);
            List<Players> p = new List<Players>();
            Groups g = new Groups(0, 0, 0, 0, p);
            group.Add(g);
            ChooseKey.value = 0;
            ChooseNote.value = 0;
        }

        void InitDropdown(){
            ChooseKey.ClearOptions();
            ChooseKey.options.Add (new TMP_Dropdown.OptionData() {text="Sol"});
            ChooseKey.options.Add (new TMP_Dropdown.OptionData() {text="Ut"});
            ChooseNote.ClearOptions();
            ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Do"});
            ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Re"});
            ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Mi"});
            ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Fa"});
            ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Sol"});
            ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="La"});
            ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Si"});
            LabelKey.text = ChooseKey.options[0].text;
            LabelNote.text = ChooseNote.options[0].text;
        }

        void UpdateList(){
            groupList.ResetList();
            groupList.groupList = group;
            groupList.CreateList();
            playerList.ResetList();
            playerList.playerList = group[currentGroup].players;
            playerList.CreateList();
        }

        public void SendExercise(){
            IEnumerator coroutine;
            feedback.text = "Exercice envoyé";
            coroutine = WaitAndHide(2);
            StartCoroutine(coroutine);
            feedback.color = Color.green;
            //Reset student answers
            for(int i = 0; i < group[currentGroup].players.Count; i++){
                group[currentGroup].players[i].answer = "";
            }
            playerList.ResetAnswerColor();
            //Reset answers count
            group[currentGroup].correct = 0;
            group[currentGroup].wrong = 0;
            UpdateList();
            //Photon, send imagePath & correctAnswer to students
            key[currentGroup] = ChooseKey.options[ChooseKey.value];
            note[currentGroup] = ChooseNote.options[ChooseNote.value];
            correctAnswer[currentGroup] = ChooseNote.options[ChooseNote.value].text;
            foreach(Players p in group[currentGroup].players) {
                photonView.RPC("ReceiveExercise", p.player, key[currentGroup].text, note[currentGroup].text);
            }
        }

        public IEnumerator WaitAndHide(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            feedback.text = "";
        }

        [PunRPC]
        public void ReceiveAnswer(string answer, double answerTime, PhotonMessageInfo info){
            //Photon update answerList
            Debug.Log(answerTime);
            UpdateAnswer(info.Sender, answer);
        }

        private void UpdateAnswer(Player current, string answer){
            int cptP=0;
            int cptL=0;
            foreach(Player player in PhotonNetwork.PlayerList){
                if (player == current){
                    foreach (Groups lp in group){
                        foreach(Players p in lp.players){
                            if(p.player == current){
                                p.answer = answer;
                                if(currentGroup == cptL){
                                    if(answer == correctAnswer[currentGroup]){
                                        playerList.ApplyAnswerColor(p, Color.green);
                                    } else {
                                        playerList.ApplyAnswerColor(p, Color.red);
                                    }    
                                }
                                if(answer == correctAnswer[cptL]){
                                    group[cptL].correct++;
                                } else {
                                    group[cptL].wrong++;
                                }  
                            }

                            cptP++;
                        }
                        cptL++;
                        cptP=0;
                    }
                }
            }
            UpdateList();
            DisplayStudents();
        }

        private void DisplayStudents(){
            for(int i = 0; i < group[currentGroup].players.Count; i++){
                if(group[currentGroup].players[i].answer == correctAnswer[currentGroup]){
                    playerList.ApplyAnswerColor(group[currentGroup].players[i], Color.green);
                } else {
                    playerList.ApplyAnswerColor(group[currentGroup].players[i], Color.red);
                }
            }
        }

        public void ModifySelectedGroup(Groups g){
            if(currentGroupSelected != null){
                groupList.ResetSelectColor();
                currentGroupSelected = g;
            }
            currentGroupSelected = g;
        }

        private void GetStudentList(){
            foreach (Player player in PhotonNetwork.PlayerList){
                if (player != PhotonNetwork.PlayerList[0]) {
                    Players p = new Players(player); 
                    group[currentGroup].players.Add(p);
                }
            }
        }

        public void LeaveRoom() {
            photonView.RPC("LeaveRoom", RpcTarget.Others);
            PhotonNetwork.LeaveRoom();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer) {
            int cptLP = 0;
            if(!otherPlayer.IsMasterClient) {
                foreach(Groups lp in group){
                    foreach (Players p in lp.players){
                        if (selected.Contains(p)){
                            selected.Remove(p);
                        }
                        if (p.player == otherPlayer)
                            group[cptLP].players.Remove(p);
                            playerList.PlayerLeftRoom(p);
                            break;
                    }
                    cptLP++;
                }
                UpdateList();
                DisplayStudents();
            }
        }

        public override void OnPlayerEnteredRoom(Player otherPlayer){
            Players p = new Players(otherPlayer);
            group[0].players.Add(p);
            UpdateList();
            DisplayStudents();
        }

        public override void OnLeftRoom() {
            PhotonNetwork.LoadLevel("Menu");
        }

        public void TransferPlayers(){
            if(currentGroupSelected != null && group.Contains(currentGroupSelected) && currentGroupSelected.ID != currentGroup){
                foreach (Players p in group[currentGroup].players){
                    if (selected.Contains(p)){
                        group[currentGroupSelected.ID].players.Add(p);
                        selected.Remove(p);
                    }
                }
                foreach (Players p in group[currentGroupSelected.ID].players){
                    photonView.RPC("TransferGroup", p.player, Convert.ToByte(currentGroupSelected.ID+1), currentGroupSelected.ID);
                    if (group[currentGroup].players.Contains(p)){
                        group[currentGroup].players.Remove(p);
                    }
                }
                UpdateList();
                DisplayStudents();
            }
        }

        public void ChangeGroup(){
            int newG = currentGroupSelected.ID;
            if (newG < group.Count && newG >= 0){
                foreach (Players p in group[currentGroup].players){
                    photonView.RPC("ProfLeftGroup", p.player);
                    selected.Remove(p);
                }
                foreach (Players p in group[newG].players){
                    photonView.RPC("ProfJoinGroup", p.player);
                }
                if(imagePath[newG] != ""){
                    image.sprite = Resources.Load<Sprite>(imagePath[newG]);
                }
                currentGroup = newG;
                UpdateList();
                DisplayStudents();
                groupLabel.text = "Groupe actuel : \nGroupe "+(currentGroup+1);
            }
        }

        public void AddGroup(){
            imagePath.Add("");
            correctAnswer.Add("Do");
            key.Add(null);
            note.Add(null);
            List<Players> p = new List<Players>();
            Groups g = new Groups(group.Count, 0, 0, 0, p);
            group.Add(g);
            speakToClassButton.gameObject.SetActive(true);
            UpdateList();
            DisplayGroupItems();
        }

        public void DisplayGroupItems(){
            groupInfo.gameObject.SetActive(true);
            groupFrame.gameObject.SetActive(true);
        }

        /**
         * START VOCAL FONCTIONS
         */

        public void SpeakToEveryone() {
            SwitchVocal(Convert.ToByte(0));
            voiceConnection.PrimaryRecorder.TransmitEnabled = !voiceConnection.PrimaryRecorder.TransmitEnabled;
            speakToClassButton.image.color = voiceConnection.PrimaryRecorder.TransmitEnabled? new Color(0, 255, 0) : new Color(255, 255, 255);
        }

        public void SpeakToGroup() {
            if(!voiceConnection.PrimaryRecorder.TransmitEnabled) {
                SwitchVocal(Convert.ToByte(currentGroup+1));
                foreach (Players p in group[currentGroup].players){ //A AMELIORER : switcher automatiquement les eleves et le prof quand ils ont rejoint le vocal
                    photonView.RPC("SwitchGroup", p.player, Convert.ToByte(currentGroup+1));
                }
            }
            voiceConnection.PrimaryRecorder.TransmitEnabled = !voiceConnection.PrimaryRecorder.TransmitEnabled;
            speakToGroupButton.image.color = voiceConnection.PrimaryRecorder.TransmitEnabled? new Color(0, 255, 0) : new Color(255, 255, 255);
        }

        public void PrivateCall() {
            if(!voiceConnection.PrimaryRecorder.TransmitEnabled && selected.Count > 0) {
                SwitchVocal(Convert.ToByte(42));
                foreach (Players p in selected) {
                    photonView.RPC("StartPrivateCall", p.player, Convert.ToByte(42));
                    studentsInPrivateCall.Add(p);
                }
                voiceConnection.PrimaryRecorder.TransmitEnabled = true;
                privateCallButton.image.color = new Color(0, 255, 0);
                privateCallButton.GetComponentInChildren<TMP_Text>().text = "Appel privé avec "+studentsInPrivateCall.Count+" élève(s)";
            } else if (voiceConnection.PrimaryRecorder.TransmitEnabled && voiceConnection.PrimaryRecorder.AudioGroup == Convert.ToByte(42)) {
                SwitchVocal(Convert.ToByte(currentGroup+1));
                foreach (Players p in studentsInPrivateCall) {
                    photonView.RPC("EndPrivateCall", p.player);
                }
                studentsInPrivateCall.Clear();
                privateCallButton.image.color = new Color(255, 255, 255);
                voiceConnection.PrimaryRecorder.TransmitEnabled = false;
                privateCallButton.GetComponentInChildren<TMP_Text>().text = "Appel privé";
            }
        }

        private void SwitchVocal(byte group) {
            if(voiceConnection.PrimaryRecorder.AudioGroup != group) {
                voiceConnection.Client.ChangeAudioGroups(new byte[0], new byte[1] { group });
                voiceConnection.PrimaryRecorder.AudioGroup = group;
            }
        }

        
        //Fonctions appelées avec les callbacks de PlayerItem
        public void StudentSpeakToGroup(Player p) {
            SwitchVocal(Convert.ToByte(currentGroup+1));
            foreach (Players players in group[currentGroup].players){ //A AMELIORER : switcher automatiquement les eleves et le prof quand ils ont rejoint le vocal
                photonView.RPC("SwitchGroup", players.player, Convert.ToByte(currentGroup+1));
            }
            photonView.RPC("SpeakToGroup", p);
        }
        public void MuteStudent(Player p) {
            photonView.RPC("Mute", p);
        }
        public void UnMuteStudent(Player p) {
            photonView.RPC("UnMute", p);
        }

        /**
         * END VOCAL FONCTIONS
         */

        [PunRPC] 
        public void StudentRaiseHand(PhotonMessageInfo info) {
            playerList.RaiseHand(info.Sender);
        }
    }
}