using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Voice.Unity; 

public class EleveExe : MonoBehaviourPun
{
    [SerializeField] TMP_Dropdown ChooseNote = null;
    [SerializeField] TMP_Text LabelNote = null;
    [SerializeField] Image image = null;
    [SerializeField] TMP_Text feedback = null;
    [SerializeField] Button confirm = null;
    [SerializeField] TMP_Text title = null;
    [SerializeField] VoiceConnection voiceConnection = null;
    [SerializeField] Button speakButton = null;
    [SerializeField] TMP_Text info = null;
    string imagePath = "";
    string correctAnswer = "";
    bool handRaised = false;
    byte saveGroupNmb = 0;

    void Start()
    {
        InitDropdown();
        title.text = "Kakoophonie - Eleve "+ PhotonNetwork.NickName;
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
        feedback.text = "En attente ...";
        feedback.color = Color.gray; 
    }

    [PunRPC]
    public void ReceiveExercise(string key, string note){
        confirm.interactable = true;
        feedback.text = "";
        imagePath = "Images/"+key+"/"+note;
        correctAnswer = note;
        image.sprite = Resources.Load<Sprite>(imagePath);
        DisplayMessage("Nouvel exercice !");
    }

    [PunRPC]
    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    public void OnLeftRoom() {
        PhotonNetwork.LoadLevel("Menu");
    }

    public void ConfirmAnswer(){
        confirm.interactable = false;
        if (ChooseNote.options[ChooseNote.value].text == correctAnswer){
            feedback.text = "Bonne réponse";
            feedback.color = Color.green; 
        } else {
            feedback.text = "Mauvaise réponse";
            feedback.color = Color.yellow;
        }
        //Photon, send my answer to professor
        photonView.RPC("ReceiveAnswer", RpcTarget.MasterClient, ChooseNote.options[ChooseNote.value].text);
    }

    public void RaiseHand() {
        photonView.RPC("StudentRaiseHand", RpcTarget.MasterClient);
        handRaised = !handRaised;
        speakButton.image.color = handRaised? new Color(1.0f, 0.64f, 0.0f) : new Color(255, 255, 255);
        speakButton.GetComponentInChildren<TMP_Text>().text = handRaised? "Main levée" : "Lever la main";
        voiceConnection.PrimaryRecorder.TransmitEnabled = false;
    }

    private void DisplayMessage(string text) {
        info.text = text;
        StartCoroutine(Coroutine());
    }

    public IEnumerator Coroutine() {
        yield return new WaitForSeconds(2);
        info.text = "";
    }

    [PunRPC]
    public void SpeakToGroup() {
        voiceConnection.PrimaryRecorder.TransmitEnabled = !voiceConnection.PrimaryRecorder.TransmitEnabled;
        speakButton.image.color = voiceConnection.PrimaryRecorder.TransmitEnabled? new Color(0, 255, 0) : new Color(255, 255, 255);
        DisplayMessage("Vous avez la parole");
        speakButton.GetComponentInChildren<TMP_Text>().text = voiceConnection.PrimaryRecorder.TransmitEnabled? "Baisser la main" : "Lever la main";
        handRaised = voiceConnection.PrimaryRecorder.TransmitEnabled;
    }

    [PunRPC]
    public void ProfJoinGroup() {
        if(saveGroupNmb == 0) {
            voiceConnection.PrimaryRecorder.TransmitEnabled = false;
            speakButton.image.color = new Color(255, 255, 255);
            DisplayMessage("Le professeur a rejoint votre groupe");
            speakButton.GetComponentInChildren<TMP_Text>().text = "Lever la main";
            speakButton.interactable = true;
            handRaised = false;
        }
    }

    [PunRPC]
    public void ProfLeftGroup() {
        if(saveGroupNmb == 0) {
            voiceConnection.PrimaryRecorder.TransmitEnabled = false;
            speakButton.image.color = new Color(255, 255, 255);
            DisplayMessage("Le professeur a quitté votre groupe");
            speakButton.GetComponentInChildren<TMP_Text>().text = "Lever la main";
            speakButton.interactable = false;
            handRaised = false;
        }
    }

    [PunRPC]
    public void Mute() {
        if(saveGroupNmb == 0) {
            voiceConnection.PrimaryRecorder.TransmitEnabled = false;
            DisplayMessage("Vous avez été mis en sourdine");
        }
    }

    [PunRPC]
    public void UnMute() {
        if(saveGroupNmb == 0) {
            voiceConnection.PrimaryRecorder.TransmitEnabled = true;
            DisplayMessage("Vous avez la parole");
        }
    }

    [PunRPC]
    public void StartPrivateCall(byte group) {
        voiceConnection.PrimaryRecorder.TransmitEnabled = true;
        info.text = "Vous êtes en appel privé";
        saveGroupNmb = voiceConnection.PrimaryRecorder.AudioGroup;
        SwitchGroup(group);
    }

    [PunRPC]
    public void EndPrivateCall() {
        voiceConnection.PrimaryRecorder.TransmitEnabled = false;
        DisplayMessage("L'appel privé est terminé.");
        SwitchGroup(saveGroupNmb);
        saveGroupNmb = 0;
    }

    [PunRPC]
    public void TransferGroup(byte group, int newG) {
        DisplayMessage("Vous avez été déplacé dans le groupe "+(newG+1));
        speakButton.interactable = false;
        SwitchGroup(group);
    }

    [PunRPC]
    public void SwitchGroup(byte group) {
        if(voiceConnection.PrimaryRecorder.AudioGroup != group) {
            voiceConnection.Client.ChangeAudioGroups(new byte[0], new byte[1] { group });
            voiceConnection.PrimaryRecorder.AudioGroup = group;
        }
    }
}
