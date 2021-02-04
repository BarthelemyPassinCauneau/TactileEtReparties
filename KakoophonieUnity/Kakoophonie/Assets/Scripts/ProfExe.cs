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
        [SerializeField] Button speakButton = null;
        [SerializeField] PlayerList playerList = null;
        List<string> imagePath = new List<string>();
        List<string> correctAnswer = new List<string>();
        List<TMPro.TMP_Dropdown.OptionData> key = new List<TMP_Dropdown.OptionData>();
        List<TMPro.TMP_Dropdown.OptionData> note = new List<TMP_Dropdown.OptionData>();
        public List<string> selected = new List<string>();
        List<List<Players>> group = new List<List<Players>>();
        List<int> groupWrong = new List<int>();
        List<int> groupRight = new List<int>();
        int currentGroup = 0;
        int groupCount = 2;

        //TO Change into ListView
        [SerializeField] List<TMP_Text> wrong;
        [SerializeField] List<TMP_Text> correct;
        [SerializeField] List<TMP_Text> count;

        void Start()
        {
            InitComp();
            InitDropdown();
            GetStudentList();
            playerList.playerList = group[currentGroup];
            playerList.UnmuteStudentEvent.AddListener(SpeakToClass);
            playerList.CreateList();
            DisplayStudents();
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
                groupWrong.Add(0);
                groupRight.Add(0);
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

        void DisplayGroupList(){
            //TO Change into ListView
            for (int i = 0; i < groupCount; i++){
                wrong[i].text = groupWrong[i].ToString();
                correct[i].text = groupRight[i].ToString();
                count[i].text = group[i].Count.ToString();
            }
        }

        public void SendExercise(){
            IEnumerator coroutine;
            feedback.text = "Exercice envoyé";
            coroutine = WaitAndHide(2);
            StartCoroutine(coroutine);
            feedback.color = Color.green;
            //Reset student answers
            for(int i = 0; i < group[currentGroup].Count; i++){
                group[currentGroup][i].answer = "";
            }
            playerList.ResetAnswerColor();
            //Reset answers count
            for(int i = 0; i < groupCount; i++){
                groupRight[i] = 0;
                groupWrong[i] = 0;
            }
            UpdateList();
            DisplayGroupList();
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
                                    if(answer == correctAnswer[currentGroup]){
                                        playerList.ApplyAnswerColor(p, Color.green);
                                    } else {
                                        playerList.ApplyAnswerColor(p, Color.red);
                                    }    
                                }
                            }
                            if(answer == correctAnswer[cptL]){
                                groupRight[cptL]++;
                            } else {
                                groupWrong[cptL]++;
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
            DisplayGroupList();
        }

        private void DisplayStudents(){
            for(int i = 0; i < group[currentGroup].Count; i++){
                if(group[currentGroup][i].answer == correctAnswer[currentGroup]){
                    playerList.ApplyAnswerColor(group[currentGroup][i], Color.green);
                } else {
                    playerList.ApplyAnswerColor(group[currentGroup][i], Color.red);
                }
            }
        }

        private void GetStudentList(){
            foreach (Player player in PhotonNetwork.PlayerList){
                if (player != PhotonNetwork.PlayerList[0]) {
                    Players p = new Players(player); 
                    group[currentGroup].Add(p);
                }
            }
            DisplayGroupList();
        }

        public void LeaveRoom() {
            photonView.RPC("LeaveRoom", RpcTarget.Others);
            PhotonNetwork.LeaveRoom();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer) {
            if(!otherPlayer.IsMasterClient) {
                foreach(List<Players> lp in group){
                    foreach (Players p in lp){
                        if (selected.Contains(p.player.NickName)){
                            selected.Remove(p.player.NickName);
                        }
                        if (p.player == otherPlayer)
                            group[currentGroup].Remove(p);
                            playerList.PlayerLeftRoom(p);
                            break;
                    }
                }
                UpdateList();
                DisplayGroupList();
                DisplayStudents();
            }
        }

        public override void OnPlayerEnteredRoom(Player otherPlayer){
            Players p = new Players(otherPlayer);
            group[0].Add(p);
            UpdateList();
            DisplayGroupList();
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
            UpdateList();
            DisplayGroupList();
            DisplayStudents();
        }

        public void ChangeGroup(int direction){
            int newG = currentGroup + direction;
            if (newG < groupCount && newG >= 0){
                if(imagePath[newG] != "" && key[newG] != null && note[newG] != null){
                    image.sprite = Resources.Load<Sprite>(imagePath[newG]);
                }
                currentGroup = newG;
                UpdateList();
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
            playerList.RaiseHand(info.Sender);
        }

        public void SpeakToClass(Player p) {
            photonView.RPC("SpeakToClass", p);
        }
    }
}