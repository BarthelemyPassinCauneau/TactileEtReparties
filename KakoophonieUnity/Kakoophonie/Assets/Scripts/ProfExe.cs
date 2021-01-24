using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ProfExe : MonoBehaviour
{
    [SerializeField] TMP_Dropdown ChooseKey;
    [SerializeField] TMP_Dropdown ChooseNote;
    [SerializeField] TMP_Text LabelKey;
    [SerializeField] TMP_Text LabelNote;
    [SerializeField] Image image;
    [SerializeField] TMP_Text feedback;
    [SerializeField] List<TMP_Text> nameList;
    [SerializeField] List<TMP_Text> answerList;
    string imagePath = "";
    string correctAnswer = "";
    List<Player> studentList = new List<Player>();

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

        //Photon cheat, to remove
        ReceiveAnswer();
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
        feedback.gameObject.SetActive(true);
        feedback.text = "Exercice envoyé";
        feedback.color = Color.green;

        //Photon, send imagePath & correctAnswer to students
    }

    public void ReceiveAnswer(){
        //Photon update answerList

        //Cheat without Photon ReceiveExercise, to remove
        Player current = PhotonNetwork.PlayerList[1];
        string answer = "Do";
        UpdateAnswer(current, answer);
    }

    private void UpdateAnswer(Player current, string answer){
        for(int i = 0; i < studentList.Count; i++){
            if (PhotonNetwork.PlayerList[i+1] == current){
                answerList[i].text = answer;
                Debug.Log(answer);
                Debug.Log(correctAnswer);
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
            answerList[i].text = "";
            nameList[i].text = studentList[i].NickName;
        }
    }

    private void GetStudentList(){
        foreach (Player player in PhotonNetwork.PlayerList){
            if (player != PhotonNetwork.PlayerList[0]) {
                studentList.Add(player);
            }
        }
    }

}
