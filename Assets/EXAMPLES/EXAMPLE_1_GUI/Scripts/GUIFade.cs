using GameArchitecture.Util;
using UnityEngine;

namespace EXAMPLES.EXAMPLE_1_GUI.Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GUIFade : MonoBehaviour
    {
        [Header("Values")] public float duration = 0.5f;
        public bool startActiveFlag = false;
        
        private CanvasGroup _canvasGroup;

        private Coroutine _fadeRoutine;

        private float _animationF;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _animationF = startActiveFlag ? 1f : 0f;
            _canvasGroup.alpha = _animationF;
        }

        private void Animate(int direction)
        {
            if (_fadeRoutine is not null)
                StopCoroutine(_fadeRoutine);

            bool isDirPositive = direction > 0;
            direction = isDirPositive ? 1 : -1;
            
            // Set the initial value for the fade direction
            float initialValue = _animationF;
            _fadeRoutine = StartCoroutine(AnimationUtil.Animate(f =>
            {
                _animationF = f;
                _canvasGroup.alpha = _animationF;
            }, initialValue, duration, direction));
        }

        public void FadeIn() => Animate(1);

        public void FadeOut() => Animate(-1);
    }
}