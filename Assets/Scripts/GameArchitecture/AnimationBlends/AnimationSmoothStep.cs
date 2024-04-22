using UnityEngine;

namespace GameArchitecture.AnimationBlends
{
    [CreateAssetMenu(fileName = "AnimationSmoothStep", menuName = "AnimationFunction/SmoothStep", order = 0)]
    public class AnimationSmoothStep : AnimationFunction
    {
        public override float Evaluate(float x) => SmoothStep(x);
    }
}