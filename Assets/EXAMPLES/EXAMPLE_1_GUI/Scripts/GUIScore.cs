using GameArchitecture.ScriptablePatterns;
using TMPro;
using UnityEngine;

namespace EXAMPLES.EXAMPLE_1_GUI.Scripts
{
    public class GUIScore : MonoBehaviour
    {
        [Header("References")]
        public ScriptableString scoreText;
        
        private TextMeshProUGUI _text;
        
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        
        private void Update()
        {
            if (Time.frameCount % 10 != 0)
                return;
            
            if (_text.text != scoreText.Value)
                _text.text = scoreText.Value;
        }
    }
}