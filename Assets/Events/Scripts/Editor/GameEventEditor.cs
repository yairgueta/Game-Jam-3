using System;
using UnityEditor;
using UnityEngine;

namespace Events
{
    [CustomEditor(typeof(GameEvent))] [CanEditMultipleObjects]
    public class GameEventEditor : UnityEditor.Editor
    {
        private GameEvent gameEvent;
        private void OnEnable()
        {
            gameEvent = (GameEvent) target;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Raise"))
            {
                gameEvent.Raise();
            }
            GUILayout.Label("Listeners:");
            foreach (var listener in gameEvent.Listeners)
            {
                GUILayout.Label(listener.gameObject.name);
            }
            
            base.OnInspectorGUI();
        }
    }
}