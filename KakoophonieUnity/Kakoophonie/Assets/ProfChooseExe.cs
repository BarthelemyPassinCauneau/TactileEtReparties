using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfChooseExe : MonoBehaviour
{
    [SerializeField] TMP_Dropdown ChooseKey;
    [SerializeField] TMP_Dropdown ChooseNote;
    [SerializeField] Image image;
    string imagePath = "";

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

    }

    void Update()
    {
        if ("Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text != imagePath){
            imagePath = "Images/"+ChooseKey.options[ChooseKey.value].text+"/"+ChooseNote.options[ChooseNote.value].text;
            image.sprite = Resources.Load<Sprite>(imagePath);
        }
    }

    public void SendExercice(){
        if (imagePath != ""){
            //Photon code, send img path & correct answer to students
        } 
    }
}
