using UnityEngine;

namespace GameArchitecture.ScriptablePatterns
{
    public abstract class ScriptableVar<T> : ScriptableObject
    {
        [SerializeField] private T defaultValue = default(T);
        [SerializeField] private bool isConstant = false;
        [HideInInspector] public bool hasValueChanged = false;

        private T m_Value = default;
        public T Value
        {
            get
            {
                if (isConstant) return defaultValue;
                hasValueChanged = false;
                return m_Value;
            }
            set
            {
                if (isConstant) return;
                hasValueChanged = true;
                m_Value = value;
            }
        }
    }
}