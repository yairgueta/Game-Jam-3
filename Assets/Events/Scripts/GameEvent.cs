using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "New Event", fileName = "Event@Category@New Event", order = 0)]
    public class GameEvent : ScriptableObject
    {
        public string Category => category;
        public string EventName => eventName;
        
        [SerializeField] private string category, eventName;
        
        private readonly HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();
        public  IEnumerable<GameEventListener> Listeners => listeners;

        private void Awake()
        {
            var s = name.Split('@');
            category = s.ElementAtOrDefault(1);
            eventName = s.ElementAtOrDefault(2);
        }

        public void Raise(object args = null)
        {
            foreach (var listener in listeners)
            {
                listener.OnEventRaised(args);
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }
        
        public void UnregisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}