using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EleveExe : MonoBehaviour
{
    [SerializeField] TMP_Dropdown ChooseNote;
    [SerializeField] TMP_Text LabelNote;
    [SerializeField] Image image;
    [SerializeField] TMP_Text feedback;
    string imagePath = "";
    string correctAnswer = "";

    void Start()
    {
        ChooseNote.ClearOptions();
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Do"});
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Re"});
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Mi"});
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Fa"});
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Sol"});
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="La"});
        ChooseNote.options.Add (new TMP_Dropdown.OptionData() {text="Si"});
        LabelNote.text = ChooseNote.options[0].text;

        //Cheat without Photon ReceiveExercice
        imagePath = "Images/Ut/Do";
        correctAnswer = "Do";
        image.sprite = Resources.Load<Sprite>(imagePath);
    }

    void ReceiveExercice(){
        feedback.gameObject.SetActive(false);

        //Photon, get infos from ProfExe.cs SendExercice()
        //imagePath = ...
        //correctAnswer = ...
    }

    public void ConfirmAnswer(){
        feedback.gameObject.SetActive(true);
        if (ChooseNote.options[ChooseNote.value].text == correctAnswer){
            feedback.text = "Good answer";
            feedback.color = Color.green; 
        } else {
            feedback.text = "Wrong answer";
            feedback.color = Color.yellow;
        }

        //Photon, send my answer to professor
    }
}
