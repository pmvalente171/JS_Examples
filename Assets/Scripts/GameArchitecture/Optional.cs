using System;
using UnityEngine;

namespace GameArchitecture
{
    [Serializable]
    public struct Optional<T>
    {
        [SerializeField] private bool enabled;
        [SerializeField] private T value;

        public Optional(T initValue)
        {
            enabled = true;
            value = initValue;
        }

        public bool Enabled => enabled;
        public T Value => value;
    }
}