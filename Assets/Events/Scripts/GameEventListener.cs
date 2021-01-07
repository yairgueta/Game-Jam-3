using System;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public class GameEventListener : MonoBehaviour
    {
        [Header("Game Event Listener References")]
        public GameEvent gameEvent;
        public UnityEvent<object> response = new UnityEvent<object>();

        private void OnEnable()
        {
            gameEvent?.RegisterListener(this);
        }

        public void InitEvent(GameEvent ge)
        {
            gameEvent = ge;
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent?.UnregisterListener(this);
        }

        public virtual void OnEventRaised(object args)
        {
            response.Invoke(args);
        }
    }
}