using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections.Concurrent;
using GameArchitecture.ScriptablePatterns;

namespace GameArchitecture.ScriptablePatterns
{
    [System.Serializable]
    public class Event : UnityEvent {}
    
    [System.Serializable]
    public class FloatEvent : UnityEvent<float>{}


    
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvents/GameEvent", order = 0)]
    public class GameEvent : ScriptableObject
    {
        private HashSet<EventListener> mEventListeners = new HashSet<EventListener>();
        private ConcurrentQueue<EventListener> mRemoveQueue = new ConcurrentQueue<EventListener>();

        public void Listen(EventListener listener)
        {
            if (mEventListeners.Contains(listener)) return;

            mEventListeners.Add(listener);
        }

        public void Deafen(EventListener listener)
        {
            if(!mEventListeners.Contains(listener)) return;
            
            mRemoveQueue.Enqueue(listener);
        }

        public void Invoke()
        {
            foreach (var listener in mEventListeners)
            {
                listener.Invoke();
            }
            CleanQueue();
        }

        private void CleanQueue()
        {
            while (!mRemoveQueue.IsEmpty)
            {
                EventListener listener;
                if (mRemoveQueue.TryDequeue(out listener))
                {
                    mEventListeners.Remove(listener);
                }
            }
        }
    }
}