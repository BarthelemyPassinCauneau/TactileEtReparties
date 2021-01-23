using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParticipantItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _name = null;

    public void SetParticipantInfo(string name) {
        _name.text = name;
    }
}
