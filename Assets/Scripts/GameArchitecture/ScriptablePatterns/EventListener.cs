using UnityEngine;
using UnityEngine.Events;
using Event = GameArchitecture.ScriptablePatterns.Event;

namespace GameArchitecture.ScriptablePatterns
{
    public class EventListener : MonoBehaviour
    {
        [Header("Events")] [SerializeField] public GameEvent gameEvent = default;

        [SerializeField] private Event localEvent = new Event();

        private void Awake() => gameEvent.Listen(this);

        private void OnDestroy() => gameEvent.Deafen(this);

        private void OnDisable() => gameEvent.Deafen(this);

        public void Invoke() => localEvent.Invoke();

        public void AddListener(UnityAction call) => localEvent.AddListener(call);
    }
}