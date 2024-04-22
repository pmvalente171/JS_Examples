using System.Collections;
using UnityEngine;

namespace GameArchitecture.Util
{
    public static class MonoBehaviourExtensions
    {
        public static CoroutineHandle RunCoroutine(
            this MonoBehaviour owner,
            IEnumerator coroutine)
        {
            return new CoroutineHandle(owner, coroutine);
        }
    }
}