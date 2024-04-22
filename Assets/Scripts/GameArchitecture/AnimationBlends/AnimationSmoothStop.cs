using UnityEngine;

namespace GameArchitecture.AnimationBlends
{
    [CreateAssetMenu(fileName = "SmoothStop", menuName = "AnimationFunction/SmoothStop", order = 0)]
    public class AnimationSmoothStop : AnimationFunction
    {
        public override float Evaluate(float x) => SmoothStop(x);
    }
}