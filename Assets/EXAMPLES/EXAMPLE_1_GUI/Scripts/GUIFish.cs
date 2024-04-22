using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EXAMPLES.EXAMPLE_1_GUI.Scripts
{
    public class GUIFish : MonoBehaviour
    {
        [Header("References")]
        public Image fishIcon;
        public TextMeshProUGUI fishName;
        public TextMeshProUGUI fishPrice;
        
        public void SetFish(Fish fish)
        {
            fishIcon.sprite = fish.icon;
            fishName.text = fish.name;
            fishPrice.text = fish.value.ToString("C0");
        }
    }
}