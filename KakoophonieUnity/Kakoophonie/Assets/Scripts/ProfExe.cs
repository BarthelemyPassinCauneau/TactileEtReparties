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
        [SerializeField] List<TMP_Text> nameList = null;
        [SerializeField] List<TMP_Text> answerList = null;
        [SerializeField] TMP_Text title = null;
        [SerializeField] TMP_Text groupLabel = null;
        [SerializeField] VoiceConnection voiceConnection = null;
        [SerializeField] Button speakButton = null;
        [SerializeField] PlayerList playerList = null;
        List<string> imagePath = new List<string>();
        List<string> correctAnswer = new List<string>();
        List<TMPro.TMP_Dropdown.OptionData> key = new List<TMP_Dropdown.OptionData>();
        List<TMPro.TMP_Dropdown.OptionData> note = new List<TMP_Dropdown.OptionData>();
        List<string> selected = new List<string>();
        List<List<Players>> group = new List<List<Players>>();
        int currentGroup = 0;
        int groupCount = 2;

        void Start()
        {
            InitComp();
            InitDropdown();
            GetStudentList();
            DisplayStudents();
            playerList.playerList = group[currentGroup];
            playerList.CreateList();
        }

        void Update()
        {
            if ("Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text != imagePath[currentGroup]){
                imagePath[currentGroup] = "Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text;
                image.sprite = Resources.Load<Sprite>(imagePath[currentGroup]);
            }
        }

        void InitComp(){
            title.text = "Kakoophonie - Professeur "+ PhotonNetwork.NickName;
            groupLabel.text = "Groupe actuel : \nGroupe "+(currentGroup+1);
            for(int i = 0; i < groupCount; i++){
                imagePath.Add("");
                correctAnswer.Add("Do");
                key.Add(null);
                note.Add(null);
                List<Players> lp = new List<Players>();
                group.Add(lp);
                ChooseKey.value = 0;
                ChooseNote.value = 0;
            }
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
            playerList.ResetList();
            playerList.playerList = group[currentGroup];
            playerList.CreateList();
        }

        public void SendExercise(){
            IEnumerator coroutine;
            feedback.text = "Exercice envoyé";
            coroutine = WaitAndHide(2);
            StartCoroutine(coroutine);
            feedback.color = Color.green;
            //Reset student answers
            for(int i = 0; i < group[currentGroup].Count; i++){
                answerList[i].text = "";
                group[currentGroup][i].answer = "";
            }
            //Photon, send imagePath & correctAnswer to students
            key[currentGroup] = ChooseKey.options[ChooseKey.value];
            note[currentGroup] = ChooseNote.options[ChooseNote.value];
            correctAnswer[currentGroup] = ChooseNote.options[ChooseNote.value].text;
            foreach(Players p in group[currentGroup])
                photonView.RPC("ReceiveExercise", p.player, key[currentGroup].text, note[currentGroup].text);
        }

        public IEnumerator WaitAndHide(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            feedback.text = "";
        }

        [PunRPC]
        public void ReceiveAnswer(string answer, PhotonMessageInfo info){
            //Photon update answerList
            UpdateAnswer(info.Sender, answer);
        }

        private void UpdateAnswer(Player current, string answer){
            int cptP=0;
            int cptL=0;
            foreach(Player player in PhotonNetwork.PlayerList){
                if (player == current){
                    foreach (List<Players> lp in group){
                        foreach(Players p in lp){
                            if(p.player == current){
                                p.answer = answer;
                                if(currentGroup == cptL){
                                    answerList[cptP].text = answer;
                                    if(answer == correctAnswer[currentGroup]){
                                        answerList[cptP].color = Color.green;
                                    } else {
                                        answerList[cptP].color = Color.yellow;
                                    }    
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
        }

        private void DisplayStudents(){
            for(int i = 0; i < group[currentGroup].Count; i++){
                nameList[i].gameObject.SetActive(true);
                answerList[i].gameObject.SetActive(true);
                answerList[i].text = group[currentGroup][i].answer;
                if(group[currentGroup][i].answer == correctAnswer[currentGroup]){
                    answerList[i].color = Color.green;
                } else {
                    answerList[i].color = Color.yellow;
                }
                nameList[i].text = group[currentGroup][i].player.NickName;
            }
        }

        private void ResetDisplay(){
            for(int i = 0; i < 6; i++){
                answerList[i].text = "";
                nameList[i].text = "";
            }
        }

        private void GetStudentList(){
            foreach (Player player in PhotonNetwork.PlayerList){
                if (player != PhotonNetwork.PlayerList[0]) {
                    Players p = new Players(player); 
                    group[currentGroup].Add(p);
                }
            }
        }

        public void LeaveRoom() {
            photonView.RPC("LeaveRoom", RpcTarget.Others);
            PhotonNetwork.LeaveRoom();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer) {
            if(!otherPlayer.IsMasterClient) {
                ResetDisplay();
                foreach (Players p in group[currentGroup]){
                    if (selected.Contains(p.player.NickName)){
                        selected.Remove(p.player.NickName);
                    }
                    if (p.player == otherPlayer)
                        group[currentGroup].Remove(p);
                        break;
                }
                DisplayStudents();
            }
        }

        public override void OnPlayerEnteredRoom(Player otherPlayer){
            ResetDisplay();
            Players p = new Players(otherPlayer);
            group[0].Add(p);
            DisplayStudents();
        }

        public override void OnLeftRoom() {
            PhotonNetwork.LoadLevel("Menu");
        }

        public void SelectPlayer(TMP_Text text){
            if(text.color == Color.white){
                text.color = Color.black;
                selected.Add(text.text);
            } else {
                text.color = Color.white;
                selected.Remove(text.text);
            }
        }

        public void TransferPlayers(){
            int newG = 0;
            if (currentGroup == 0){ 
                newG = 1;
            }  else {
                newG = 0;
            }
            foreach (Players p in group[currentGroup]){
                if (selected.Contains(p.player.NickName)){
                    group[newG].Add(p);
                    selected.Remove(p.player.NickName);
                }
            }
            foreach (Players p in group[newG]){
                if (group[currentGroup].Contains(p)){
                    group[currentGroup].Remove(p);
                }
            }
            foreach(TMP_Text t in nameList){
                t.color = Color.white;
            }
            ResetDisplay();
            DisplayStudents();
        }

        public void ChangeGroup(int direction){
            int newG = currentGroup + direction;
            if (newG < groupCount && newG >= 0){
                ResetDisplay();
                if(imagePath[newG] != "" && key[newG] != null && note[newG] != null){
                    image.sprite = Resources.Load<Sprite>(imagePath[newG]);
                }
                currentGroup = newG;
                DisplayStudents();
                groupLabel.text = "Groupe actuel : \nGroupe "+(currentGroup+1);
            }
        }

        public void SpeakToClass() {
            voiceConnection.PrimaryRecorder.TransmitEnabled = !voiceConnection.PrimaryRecorder.TransmitEnabled;
            speakButton.image.color = voiceConnection.PrimaryRecorder.TransmitEnabled? new Color(0, 255, 0) : new Color(255, 255, 255);
        }

        [PunRPC] 
        public void StudentRaiseHand(PhotonMessageInfo info) {
            // Afficher le bouton
        }
    }
}