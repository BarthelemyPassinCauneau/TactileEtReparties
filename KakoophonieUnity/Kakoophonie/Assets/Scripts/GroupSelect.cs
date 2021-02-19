using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Exe{
    public class GroupSelect : MonoBehaviour
    {
        [SerializeField] Image group;
        [SerializeField] Color selected;
        private Color unselected;
        private ProfExe prof;
        private Button btn;
        private Image img;
        private GroupItem groupItem;

        void Start () {
            btn = group.GetComponent<Button>();
            img = group.GetComponent<Image>();
            groupItem = group.GetComponent<GroupItem>();
            unselected = img.color;
            prof = FindObjectOfType<ProfExe>();
            btn.onClick.AddListener(TaskOnClick);
        }

        void TaskOnClick(){
            if(img.color == unselected){
                prof.ModifySelectedGroup(groupItem.group);
                img.color = selected;
            } else {
                prof.currentGroupSelected = null;
                img.color = unselected;
            }
        }
    }
}