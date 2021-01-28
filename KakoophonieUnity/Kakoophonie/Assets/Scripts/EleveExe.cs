using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class EleveExe : MonoBehaviourPun
{
    [SerializeField] TMP_Dropdown ChooseNote = null;
    [SerializeField] TMP_Text LabelNote = null;
    [SerializeField] Image image = null;
    [SerializeField] TMP_Text feedback = null;
    [SerializeField] Button confirm = null;
    string imagePath = "";
    string correctAnswer = "";

    void Start()
    {
        InitDropdown();
        confirm.interactable = false;
    }

    void InitDropdown(){
        ChooseNote.ClearOptions();
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Do"});
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Re"});
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Mi"});
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Fa"});
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Sol"});
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="La"});
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Si"});
        LabelNote.text = ChooseNote.options[0].text;
    }

    [PunRPC]
    public void ReceiveExercise(string key, string note){
        confirm.interactable = true;
        feedback.gameObject.SetActive(false);
        imagePath = "Images/"+key+"/"+note;
        correctAnswer = note;
        image.sprite = Resources.Load<Sprite>(imagePath);
    }

    public void ConfirmAnswer(){
        confirm.interactable = false;
        feedback.gameObject.SetActive(true);
        if (ChooseNote.options[ChooseNote.value].text == correctAnswer){
            feedback.text = "Bonne répense";
            feedback.color = Color.green; 
        } else {
            feedback.text = "Mauvaise réponse";
            feedback.color = Color.yellow;
        }
        //Photon, send my answer to professor
        photonView.RPC("ReceiveAnswer", RpcTarget.MasterClient, ChooseNote.options[ChooseNote.value].text);
    }
}
