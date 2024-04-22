using System;
using System.Collections;
using System.Collections.Generic;
using GameArchitecture.Util;
using UnityEngine;

namespace GameArchitecture
{
    public class AnimationManager : MonoBehaviour
    {
        [Serializable]
        private class FloatEvent : UnityEngine.Events.UnityEvent<float> { }
    
        [Serializable]
        private class AnimationSettings
        {
            [SerializeField] private FloatEvent animationEvent;
            [Space] [SerializeField] private Optional<AnimationFunction> animationFunction;
        
            // Lerps the scale of the transform between the initial and final scale
            public void SetValue(float scale)
            {
                if (!animationFunction.Enabled)
                {
                    animationEvent.Invoke(scale);
                    return;
                }
            
                animationEvent.Invoke(animationFunction.Value.Evaluate(scale));
            }

            // Sets the scale of the transform to the initial scale
            public void Init() => SetValue(0f);
        }

        [Header("Scale Settings")]
        [SerializeField] private List<AnimationSettings> scaleSettings;
    
        [Header("Values")]
        [SerializeField] private Optional<AnimationFunction> animationFunction;

        [Range(0f, 1f)] public float _value;
        private bool _isScaling;
        private Coroutine _scaleCoroutine;
    
        public float Scale
        {
            get => _value;
            set => SetValue(value);
        }
    
        private void Start()
        {
            _value = 0f;
            foreach (var setting in scaleSettings)
                setting.Init();
        }
    
#if UNITY_EDITOR
        private void Update()
        {
            foreach (var setting in scaleSettings)
                setting.SetValue(_value);
        }
#endif
    
        private void SetValue(float scale)
        {
            _value = scale;
            _value = Mathf.Clamp01(_value);
            foreach (var setting in scaleSettings)
                setting.SetValue(scale);
        }
    
        private IEnumerator SetScaleAnimationCoroutine(float duration, int dir = 1)
        {
            _isScaling = true;
            if (animationFunction.Enabled)
                yield return AnimationUtil.Animate(SetValue,animationFunction.Value, _value, duration, dir);
            else
                yield return AnimationUtil.Animate(SetValue, _value, duration, dir);
            _isScaling = false;
        }

        public void SetScaleAnimation(float duration, int dir = 1)
        {
            if (_isScaling)
                StopCoroutine(_scaleCoroutine);
            _scaleCoroutine = StartCoroutine(
                SetScaleAnimationCoroutine(duration, dir));
        }
    }
}