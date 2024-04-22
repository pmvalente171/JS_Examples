using Game;
using GameArchitecture.Util;
using UnityEngine;

namespace EXAMPLES.EXAMPLE_1_GUI.Scripts
{
    // Example of a GUIFishList class that uses an Animator component
    [RequireComponent(typeof(Animator))]
    public class GUIFishList : MonoBehaviour
    {
        [Header("References")]
        public RectTransform contextMenu;
        public GUIFish fishPrefab;
        
        [Header("Values")]
        public float duration = 0.5f;
        public bool startActiveFlag = false;
        
        private float _animationF;
        private bool _isOpen;
        
        private Coroutine _animationRoutine;
        private Animator _animator;
        private static readonly int OpenBlend = Animator.StringToHash("OpenBlend");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _isOpen = startActiveFlag;
            _animationF = startActiveFlag ? 1f : 0f;
            _animator.SetFloat(OpenBlend, _animationF);
        }
        
        public void Open()
        {
            if (_animationRoutine is not null)
                StopCoroutine(_animationRoutine);
            
            _isOpen = true;
            UpdateFishList();
            _animationRoutine = StartCoroutine(AnimationUtil.Animate(f =>
            {
                _animationF = f;
                _animator.SetFloat(OpenBlend, Mathf.SmoothStep(0f, 1f, f));
            }, _animationF, duration, 1));
        }
        
        public void Close()
        {
            if (_animationRoutine is not null)
                StopCoroutine(_animationRoutine);
            
            _isOpen = false;
            _animationRoutine = StartCoroutine(AnimationUtil.Animate(f =>
            {
                _animationF = f;
                _animator.SetFloat(OpenBlend, Mathf.SmoothStep(0f, 1f, f));
            }, _animationF, duration, -1));
        }
        
        public void Toggle()
        {
            if (_isOpen) Close();
            else Open();
        }
        
        public void UpdateFishList()
        {
            if (!_isOpen)
                return;
            
            // Clear the list
            foreach (Transform child in contextMenu)
                Destroy(child.gameObject);
            
            // Add the fish to the list
            foreach (var fish in FishingManager.FishCaught)
            {
                var fishUI = Instantiate(fishPrefab, contextMenu);
                fishUI.SetFish(fish);
            }
        }
    }
}