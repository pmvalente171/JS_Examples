using UnityEngine;

namespace GameArchitecture.AnimationBlends
{
    [CreateAssetMenu(fileName = "AnimationSmoothStart", menuName = "AnimationFunction/SmoothStart", order = 0)]
    public class AnimationSmoothStart : AnimationFunction
    {
        public override float Evaluate(float x) => SmoothStart(x);
    }
}