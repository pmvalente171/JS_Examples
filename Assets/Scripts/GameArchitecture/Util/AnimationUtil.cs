using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameArchitecture.Util
{
    [Serializable]
    public class AnimationSettingsCurve
    {
        public float animationDuration;
        public AnimationCurve animationCurve;
            
        public List<GameObject> objectsToHide;
            
        [HideInInspector] public float animationTimer;
    }
    
    [Serializable]
    public class AnimationSettingsFunction
    {
        public float animationDuration;
        public AnimationFunction animationCurve;
            
        public List<GameObject> objectsToHide;
            
        [HideInInspector] public float animationTimer;
    }
    
    public static class AnimationUtil
    {
        public static IEnumerator LerpInTimeWindow(float time, Action<float> action)
        {
            float f = 0f;
            var ret = new WaitForEndOfFrame();
            while (f <1f)
            {
                f += Time.deltaTime / time;
                action(f);
                yield return ret;
            }
            action(1f);
        }
        
        public static IEnumerator Animate(Action<float> action, int dir, AnimationSettingsCurve animationSettingsCurve)
        {
            var target = (dir + 1f) / 2f;
            var ret = new WaitForEndOfFrame();
            action(animationSettingsCurve.animationTimer);
            
            while (Mathf.Abs(animationSettingsCurve.animationTimer - target) > 0.01f)
            {
                animationSettingsCurve.animationTimer += dir * Time.deltaTime / animationSettingsCurve.animationDuration;
                animationSettingsCurve.animationTimer = Mathf.Clamp01(animationSettingsCurve.animationTimer);
                action(animationSettingsCurve.animationCurve.Evaluate(animationSettingsCurve.animationTimer));
                yield return ret;
            }
            action(animationSettingsCurve.animationCurve.Evaluate(target));
        }

        public static IEnumerator Animate(Action<float> action, 
            float initialValue, float animationDuration, int dir)
        {
            var val = initialValue;
            var target = (dir + 1f) / 2f;
            var ret = new WaitForEndOfFrame();
            
            action(val);
            while (Mathf.Abs(val - target) > 0.01f)
            {
                val += dir * Time.deltaTime / animationDuration;
                val = Mathf.Clamp01(val);
                action(val);
                yield return ret;
            }
            action(target);
        }
        
        public static IEnumerator Animate(Action<float> action, AnimationFunction animationFunction,
            float initialValue, float animationDuration, int dir)
        {
            var val = initialValue;
            var target = (dir + 1f) / 2f;
            var ret = new WaitForEndOfFrame();
            
            action(val);
            while (Mathf.Abs(val - target) > 0.01f)
            {
                val += dir * Time.deltaTime / animationDuration;
                val = Mathf.Clamp01(val);
                action(animationFunction.Evaluate(val));
                yield return ret;
            }
            action(animationFunction.Evaluate(target));
        }
        
        public static IEnumerator Animate(Action<float> action, int dir, AnimationSettingsFunction animationSettingsCurve)
        {
            var target = (dir + 1f) / 2f;
            var ret = new WaitForEndOfFrame();
            action(animationSettingsCurve.animationTimer);
            
            while (Mathf.Abs(animationSettingsCurve.animationTimer - target) > 0.01f)
            {
                animationSettingsCurve.animationTimer += dir * Time.deltaTime / animationSettingsCurve.animationDuration;
                animationSettingsCurve.animationTimer = Mathf.Clamp01(animationSettingsCurve.animationTimer);
                action(animationSettingsCurve.animationCurve.Evaluate(animationSettingsCurve.animationTimer));
                yield return ret;
            }
            action(target);
        }
        
        public static IEnumerator AnimatorSetFloat(Animator animator, int dir, int animatorBlend, 
            AnimationSettingsCurve animationSettingsCurve)
        {
            var target = (dir + 1f) / 2f;
            var ret = new WaitForEndOfFrame();
            
            while (Mathf.Abs(animationSettingsCurve.animationTimer - target) > 0.01f)
            {
                animationSettingsCurve.animationTimer += dir * Time.deltaTime / animationSettingsCurve.animationDuration;
                animationSettingsCurve.animationTimer = Mathf.Clamp01(animationSettingsCurve.animationTimer);
                animator.SetFloat(animatorBlend, animationSettingsCurve.
                    animationCurve.Evaluate(animationSettingsCurve.animationTimer));
                yield return ret;
            }

            animator.SetFloat(animatorBlend, target);
        }
    }
}