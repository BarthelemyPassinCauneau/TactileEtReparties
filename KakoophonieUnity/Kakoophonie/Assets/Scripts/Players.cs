using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace Exe{
    public class Players {
        public Player player;
        public string answer;
        public int correctAnswers;
        public int questionsAnswered;
        public List<float> time;

        public Players(Player player) {
            this.player = player;
            this.answer = "";
            this.correctAnswers = 0;
            this.questionsAnswered = 0;
            this.time = new List<float>();
        }
        
        public double getAverageTime() {
            if(time.Count == 0)
                return 0;
            return Math.Round(time.Sum(t => t)/time.Count, 2);
        }

        public void ResetAnswers() {
            this.correctAnswers = 0;
            this.questionsAnswered = 0;
            this.time.Clear();
        }
    }
}