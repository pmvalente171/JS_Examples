using UnityEngine;

namespace GameArchitecture
{
    // [CreateAssetMenu(fileName = "AnimationFunction", menuName = "Animation/Default", order = 0)]
    public class AnimationFunction : ScriptableObject
    {
        protected static float Sqr (float x) => x * x;
        protected static float Cube (float x) => x * x * x;
        protected static float Sqrt (float x) => Mathf.Sqrt(x);
        protected static float Cbrt(float x) => Mathf.Pow(x, 1f / 3f);
        protected static float Sin (float x) => Mathf.Sin(x);
        protected static float Cos (float x) => Mathf.Cos(x);
        protected static float Flip (float x) => 1 - x;
        protected static float SmoothStop (float x) => Flip(Sqr(Flip(x)));
        protected static float SmoothStart (float x) => Sqr(x);
        protected static float Mix(float a, float b, float weightB) => a + weightB * (b - a);
        protected static float SmoothStep(float x) => Mix(SmoothStart(x), SmoothStop(x), x);
        protected static float Scale (float x, float t) => x * t;
        protected static float ReverseScale (float x, float t) => x * (1 - t);
        protected static float Arch2(float x) => Scale(Flip(x), x);
        protected static float SmoothStartArch3(float x) => Scale(Arch2(x), x);
        protected static float SmoothStopArch3(float x) => ReverseScale(Arch2(x), x);
    
        public virtual float Evaluate(float x) => x;
    }
}