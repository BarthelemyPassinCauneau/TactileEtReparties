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
    string imagePath = "";
    string correctAnswer = "";
    List<Players> studentList = new List<Players>();

    void Start()
    {
        InitDropdown();
        GetStudentList();
        DisplayStudents();
    }

    void Update()
    {
        if ("Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text != imagePath){
            imagePath = "Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text;
            correctAnswer = ChooseNote.options[ChooseNote.value].text;
            image.sprite = Resources.Load<Sprite>(imagePath);
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
        for(int i = 0; i < studentList.Count; i++){
            answerList[i].text = "";
            studentList[i].answer = "";
        }
        //Photon, send imagePath & correctAnswer to students
        photonView.RPC("ReceiveExercise", RpcTarget.Others, ChooseKey.options[ChooseKey.value].text, ChooseNote.options[ChooseNote.value].text);
    }

    private IEnumerator WaitAndHide(float waitTime)
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
        for(int i = 0; i < studentList.Count; i++){
            if (PhotonNetwork.PlayerList[i+1] == current){
                studentList[i].answer = answer;
                answerList[i].text = answer;
                if(answer == correctAnswer){
                    answerList[i].color = Color.green;
                } else {
                    answerList[i].color = Color.yellow;
                }
            }
        }
    }

    private void DisplayStudents(){
        for(int i = 0; i < studentList.Count; i++){
            nameList[i].gameObject.SetActive(true);
            answerList[i].gameObject.SetActive(true);
            answerList[i].text = studentList[i].answer;
            if(studentList[i].answer == correctAnswer){
                answerList[i].color = Color.green;
            } else {
                answerList[i].color = Color.yellow;
            }
            nameList[i].text = studentList[i].player.NickName;
        }
    }

    private void ResetDisplay(){
        for(int i = 0; i < studentList.Count; i++){
            answerList[i].text = "";
            nameList[i].text = "";
            nameList[i].gameObject.SetActive(false);
            answerList[i].gameObject.SetActive(false);
        }
    }




    private void GetStudentList(){
        foreach (Player player in PhotonNetwork.PlayerList){
            if (player != PhotonNetwork.PlayerList[0]) {
                Players p = new Players(player); 
                studentList.Add(p);
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
            foreach (Players p in studentList){
                if (p.player == otherPlayer)
                    Debug.Log("yes");
                    studentList.Remove(p);
                    break;
            }
            DisplayStudents();
        }
    }

    public override void OnPlayerEnteredRoom(Player otherPlayer){
        ResetDisplay();
        Players p = new Players(otherPlayer);
        studentList.Add(p);
        DisplayStudents();
    }

    public override void OnLeftRoom() {
        PhotonNetwork.LoadLevel("Menu");
    }

}
}