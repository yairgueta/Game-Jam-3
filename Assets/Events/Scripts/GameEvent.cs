using System;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/new GameEvent", order = 0)]
    public class GameEvent : ScriptableObject
    {
        private readonly HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();

        public void Raise(EventArgs args = null)
        {
            args ??= EventArgs.Empty;

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