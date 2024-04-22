using System;
using Game;
using UnityEngine;

namespace EXAMPLES.EXAMPLE_1_GUI.Scripts
{
    [Serializable]
    public class MenuReferences
    {
        public RectTransform GreenBar;
        public RectTransform Marker;
    }
    
    public class GUIFishing : MonoBehaviour
    {
        [Header("References")]
        public GUIFade TextFade;
        public GUIFade MenuFade;
        [Space] public MenuReferences menuReferences;
        
        [Header("Values")] public float maxY = -496f;
        private Coroutine _animationRoutine;
        
        private void Start()
        {
            var y = FishingManager.FishingTimerWindow * 2f; // Value between 0 and 1
            menuReferences.GreenBar.localScale = new Vector3(1f, y, 1f);
        }

        private void Update()
        {
            Vector3 start = new Vector3();
            Vector3 end = new Vector2(0, maxY);
            menuReferences.Marker.anchoredPosition = 
                Vector3.Lerp(start, end, FishingManager.FishingTimer);
        }

        public void Open()
        {
            TextFade.FadeOut();
            MenuFade.FadeIn();
        }

        public void Close()
        {
            TextFade.FadeIn();
            MenuFade.FadeOut();
        }
    }
}