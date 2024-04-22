using System.Collections;
using UnityEngine;

namespace GameArchitecture
{
    public class CoroutineWithData
    {
        public Coroutine Coroutine { get; private set; }
        public object result;
        private readonly IEnumerator _target;
        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            _target = target;
            Coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (_target.MoveNext())
            {
                result = _target.Current;
                yield return result;
            }
        }
    }
}