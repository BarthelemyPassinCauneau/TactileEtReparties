using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfExe : MonoBehaviour
{
    [SerializeField] TMP_Dropdown ChooseKey;
    [SerializeField] TMP_Dropdown ChooseNote;
    [SerializeField] TMP_Text LabelKey;
    [SerializeField] TMP_Text LabelNote;
    [SerializeField] Image image;
    [SerializeField] TMP_Text feedback;
    string imagePath = "";
    string correctAnswer = "";

    void Start()
    {
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

    void Update()
    {
        if ("Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text != imagePath){
            imagePath = "Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text;
            correctAnswer = ChooseNote.options[ChooseNote.value].text;
            image.sprite = Resources.Load<Sprite>(imagePath);
        }
    }

    public void SendExercice(){
        feedback.gameObject.SetActive(true);
        feedback.text = "Exercice sent";
        feedback.color = Color.green;
        
        //Photon, send imagePath & correctAnswer to students
    }

}
