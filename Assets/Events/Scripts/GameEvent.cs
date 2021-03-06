using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [CreateAssetMenu(menuName = "New Event", fileName = "Event@Category@New Event", order = 0)]
    public class GameEvent : ScriptableObject
    {
        public string Category => category;
        public string EventName => eventName;
        
        [SerializeField] private string category, eventName;
        
        private readonly List<GameEventListener> listeners = new List<GameEventListener>();
        public  IEnumerable<GameEventListener> Listeners => listeners;
        
        private void OnEnable()
        {
            var s = name.Split('@');
            category = s.ElementAtOrDefault(1);
            eventName = s.ElementAtOrDefault(2);
        }

        public void Raise(object args = null)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(args);
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

        public UnityEvent<object> Register(GameObject obj)
        {
            var listener = obj.AddComponent<GameEventListener>();
            listener.InitEvent(this);
            return listener.response;
        }

        public void Register(GameObject obj, UnityAction<object> func)
        {
            var listener = obj.AddComponent<GameEventListener>();
            listener.InitEvent(this);
            listener.response.AddListener(func);
        }
    }
}