using System;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public class GameEventListener : MonoBehaviour
    {
        [Header("Game Event Listener References")]
        public GameEvent gameEvent;
        public UnityEvent<EventArgs> response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public virtual void OnEventRaised(EventArgs args)
        {
            response?.Invoke(args);
        }
    }
}