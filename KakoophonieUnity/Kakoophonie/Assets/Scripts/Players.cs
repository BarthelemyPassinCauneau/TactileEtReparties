using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace Exe{
    public class Players {
        public Player player;
        public string answer;

        public Players(Player player) {
            this.player = player;
            this.answer = "";
        }
    }
}