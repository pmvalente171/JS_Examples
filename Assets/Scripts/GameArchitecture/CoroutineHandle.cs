using System.Collections;
using UnityEngine;

namespace GameArchitecture
{
    public class CoroutineHandle : IEnumerator
    {
        public bool IsDone { get; private set; }
        public bool MoveNext() => !IsDone;
        public void Reset() { }
        public object Current { get; }

        public CoroutineHandle(MonoBehaviour owner, IEnumerator coroutine)
        {
            Current = owner.StartCoroutine(Wrap(coroutine));
        }
        
        private IEnumerator Wrap(IEnumerator coroutine)
        {
            yield return coroutine;
            IsDone = true;
        }
    }
}