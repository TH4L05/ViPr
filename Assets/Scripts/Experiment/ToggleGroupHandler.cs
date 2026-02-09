/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.UI;

namespace eccon_lab.vipr.experiment
{
    public class ToggleGroupHandler : MonoBehaviour
    {
        [SerializeField] private Toggle[] toggles;
        

        public string GetActiveValue()
        {
            foreach (var item in toggles)
            {
                if (!item.isOn) continue;
                Text t = item.GetComponentInChildren<Text>();
                return t.text;
            }
            return "-";
        }

        public int GetActiveIndex()
        {
            int index = 0;  
            foreach (var item in toggles)
            {
                if (item.isOn)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
    }
}




