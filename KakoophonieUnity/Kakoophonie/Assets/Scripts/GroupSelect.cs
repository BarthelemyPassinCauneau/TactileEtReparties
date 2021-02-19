using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Exe{
    public class GroupSelect : MonoBehaviour
    {
        [SerializeField] Image player;
        [SerializeField] Color selected;
        private Color unselected;
        private ProfExe prof;
        private Button btn;
        private Image img;
        private PlayerItem playerItem;

        void Start () {
            btn = player.GetComponent<Button>();
            img = player.GetComponent<Image>();
            playerItem = player.GetComponent<PlayerItem>();
            unselected = img.color;
            prof = FindObjectOfType<ProfExe>();
            btn.onClick.AddListener(TaskOnClick);
        }

        void TaskOnClick(){
            if(img.color == unselected){
                img.color = selected;
                prof.selected.Add(playerItem.player);
            } else {
                img.color = unselected;
                prof.selected.Remove(playerItem.player);
            }
        }
    }
}