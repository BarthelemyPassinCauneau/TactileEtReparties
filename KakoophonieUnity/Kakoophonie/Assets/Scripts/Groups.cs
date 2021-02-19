using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace Exe{
    public class Groups {
        public int ID = 0;
        public int correct = 0;
        public int wrong = 0; 
        public int nbPlayer = 0;
        public List<Players> players = new List<Players>();

        public Groups(int ID, int correct, int wrong, int nbPlayer, List<Players> players) {
            this.ID = ID;
            this.correct = correct;
            this.wrong = wrong;
            this.nbPlayer = nbPlayer;
            this.players = players;
        }
    }
}