using UnityEngine;

namespace GameArchitecture.AnimationBlends
{
    [CreateAssetMenu(fileName = "AnimationLinear", menuName = "AnimationFunction/Linear", order = 0)]
    public class AnimationLinear : AnimationFunction
    {
        public override float Evaluate(float x) => x;
    }
}