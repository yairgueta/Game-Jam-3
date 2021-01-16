using System;
using UnityEditor;
using UnityEngine;

namespace Events
{
    [CustomEditor(typeof(GameEvent))] [CanEditMultipleObjects]
    public class GameEventEditor : UnityEditor.Editor
    {
        private GameEvent gameEvent;
        private SerializedProperty eventName;
        private SerializedProperty category;
        
        private void OnEnable()
        {
            gameEvent = (GameEvent) target;
            eventName = serializedObject.FindProperty("eventName");
            category = serializedObject.FindProperty("category");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (GUILayout.Button("Open All Events Window"))
            {
                EventsWindow.ShowWindow();
            }
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Event Category:");
            EditorGUILayout.LabelField(category.stringValue);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Event Name:");
            EditorGUILayout.LabelField(eventName.stringValue);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Raise"))
            {
                gameEvent.Raise();
            }
            GUILayout.Label("Listeners:");
            foreach (var listener in gameEvent.Listeners)
            {
                GUILayout.Label(listener.gameObject.name);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}