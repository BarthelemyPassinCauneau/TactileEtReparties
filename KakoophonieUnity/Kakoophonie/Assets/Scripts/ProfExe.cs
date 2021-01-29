using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime; 

namespace Exe{
public class ProfExe : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Dropdown ChooseKey = null;
    [SerializeField] TMP_Dropdown ChooseNote = null;
    [SerializeField] TMP_Text LabelKey = null;
    [SerializeField] TMP_Text LabelNote = null;
    [SerializeField] Image image = null;
    [SerializeField] TMP_Text feedback = null;
    [SerializeField] List<TMP_Text> nameList = null;
    [SerializeField] List<TMP_Text> answerList = null;
    [SerializeField] TMP_Text title = null;
    [SerializeField] TMP_Text group = null;
    string imagePath1 = "";
    string imagePath2 = "";
    TMPro.TMP_Dropdown.OptionData key1 = null;
    TMPro.TMP_Dropdown.OptionData note1 = null;
    string correctAnswer1 = "Do";
    string correctAnswer2 = "Do";
    TMPro.TMP_Dropdown.OptionData key2 = null;
    TMPro.TMP_Dropdown.OptionData note2 = null;
    List<string> selected = new List<string>();
    List<Players> group1 = new List<Players>();
    List<Players> group2 = new List<Players>();
    int currentGroup = 1;

    void Start()
    {
        title.text = "Kakoophonie - Professeur "+ PhotonNetwork.NickName;
        group.text = "Groupe actuel : \nGroupe "+currentGroup;
        InitDropdown();
        GetStudentList();
        DisplayStudents();
    }

    void Update()
    {
        if(currentGroup == 1){
            if ("Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text != imagePath1){
                imagePath1 = "Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text;
                image.sprite = Resources.Load<Sprite>(imagePath1);
                }
        } else if (currentGroup == 2){
            if ("Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text != imagePath2){
                imagePath2 = "Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text;
                image.sprite = Resources.Load<Sprite>(imagePath2);
            }
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

    public void SendExercise(){
        IEnumerator coroutine;
        feedback.text = "Exercice envoyé";
        coroutine = WaitAndHide(2);
        StartCoroutine(coroutine);
        feedback.color = Color.green;
        //Reset student answers
        for(int i = 0; i < group1.Count; i++){
            answerList[i].text = "";
            group1[i].answer = "";
        }
        
        //Photon, send imagePath & correctAnswer to students
        if(currentGroup == 1){
            key1 = ChooseKey.options[ChooseKey.value];
            note1 = ChooseNote.options[ChooseNote.value];
            correctAnswer1 = ChooseNote.options[ChooseNote.value].text;
            foreach(Players p in group1)
            photonView.RPC("ReceiveExercise", p.player, key1.text, note1.text);
        } else if (currentGroup == 2){
            key2 = ChooseKey.options[ChooseKey.value];
            note2 = ChooseNote.options[ChooseNote.value];
            correctAnswer2 = ChooseNote.options[ChooseNote.value].text;
            foreach(Players p in group2)
            photonView.RPC("ReceiveExercise", p.player, key2.text, note2.text);
        }
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
        int cpt=0;
        foreach(Player player in PhotonNetwork.PlayerList){
            if (player == current){
                foreach (Players p in group1){
                    if(p.player == current){
                        p.answer = answer;
                        if(currentGroup == 1){
                            answerList[cpt].text = answer;
                            if(answer == correctAnswer1){
                                    answerList[cpt].color = Color.green;
                                } else {
                                    answerList[cpt].color = Color.yellow;
                                }   
                            cpt++;    
                        }
                    }
                }
                foreach (Players p in group2){
                    if(p.player == current){
                        p.answer = answer;
                        if(currentGroup == 2){
                            answerList[cpt].text = answer;
                            if(answer == correctAnswer2){
                                answerList[cpt].color = Color.green;
                            } else {
                                answerList[cpt].color = Color.yellow;
                            }
                            cpt++;
                        }
                    }
                }
            }
        }
    }

    private void DisplayStudents(){
        if (currentGroup == 1){
            for(int i = 0; i < group1.Count; i++){
                nameList[i].gameObject.SetActive(true);
                answerList[i].gameObject.SetActive(true);
                answerList[i].text = group1[i].answer;
                if(group1[i].answer == correctAnswer1){
                    answerList[i].color = Color.green;
                } else {
                    answerList[i].color = Color.yellow;
                }
                nameList[i].text = group1[i].player.NickName;
            }
        } else if (currentGroup == 2) {
            for(int i = 0; i < group2.Count; i++){
                nameList[i].gameObject.SetActive(true);
                answerList[i].gameObject.SetActive(true);
                answerList[i].text = group2[i].answer;
                if(group2[i].answer == correctAnswer2){
                    answerList[i].color = Color.green;
                } else {
                    answerList[i].color = Color.yellow;
                }
                nameList[i].text = group2[i].player.NickName;
            }
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
                group1.Add(p);
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
            foreach (Players p in group1){
                if (selected.Contains(p.player.NickName)){
                    selected.Remove(p.player.NickName);
                }
                if (p.player == otherPlayer)
                    group1.Remove(p);
                    break;
            }
            DisplayStudents();
        }
    }

    public override void OnPlayerEnteredRoom(Player otherPlayer){
        ResetDisplay();
        Players p = new Players(otherPlayer);
        group1.Add(p);
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
        if (currentGroup == 1){
            foreach (Players p in group1){
                if (selected.Contains(p.player.NickName)){
                    group2.Add(p);
                    selected.Remove(p.player.NickName);
                }
            }
            foreach (Players p in group2){
                if (group1.Contains(p)){
                    group1.Remove(p);
                }
            }

        } else if (currentGroup == 2){
            foreach (Players p in group2){
                if (selected.Contains(p.player.NickName)){
                    group1.Add(p);
                    selected.Remove(p.player.NickName);
                }
            } 
            foreach (Players p in group1){
                if (group2.Contains(p)){
                    group2.Remove(p);
                }
            }
        }
        foreach(TMP_Text t in nameList){
            t.color = Color.white;
        }
        ResetDisplay();
        DisplayStudents();
    }

    public void ChangeGroup(){
        ResetDisplay();
        if (currentGroup == 1) {
            if(imagePath2 != ""){
                ChooseKey.options[ChooseKey.value] = key2;
                ChooseNote.options[ChooseNote.value] = note2;
                image.sprite = Resources.Load<Sprite>(imagePath2);
            }
            currentGroup++;
        } else {
            ChooseKey.options[ChooseKey.value] = key1;
            ChooseNote.options[ChooseNote.value] = note1;
            image.sprite = Resources.Load<Sprite>(imagePath1);
            currentGroup--;
        }
        DisplayStudents();   
        group.text = "Groupe actuel : \nGroupe "+currentGroup;
    }

}
}