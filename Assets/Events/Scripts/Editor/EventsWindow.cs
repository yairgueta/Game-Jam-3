using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Events
{
    public class EventsWindow : EditorWindow
    {
        private List<GameEvent> events;
        private List<bool> foldouts;
        private Vector2 scrollPos;
        private List<KeyValuePair<string, List<GameEvent>>> categorizedEvents;

        private void Awake()
        {
            foldouts = new List<bool>();
        }

        private void OnFocus()
        {
            events = new List<GameEvent>();
            var guids = AssetDatabase.FindAssets("t:GameEvent", new[] {"Assets/Events"});
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                events.Add(AssetDatabase.LoadAssetAtPath<GameEvent>(path));
            }

            Dictionary<string, List<GameEvent>>categorizedDict = new Dictionary<string, List<GameEvent>>();

            foreach (var ge in events)
            {
                if (categorizedDict.TryGetValue(ge.Category, out var catEve))
                    catEve.Add(ge);
                else
                    categorizedDict[ge.Category] = new List<GameEvent> {ge};
            }

            categorizedEvents = (from entry in categorizedDict orderby entry.Value.Count descending select entry).ToList();
        }

        [MenuItem("Tools/Show All Events")]
        public static void ShowWindow()
        {
            var window = GetWindow<EventsWindow>();
            window.titleContent = new GUIContent("All Events");
            window.Show();
        }

        private void OnGUI()
        {
            var ident = 10;
            int i = 0;
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            foreach (var cat in categorizedEvents)
            {
                if (i >= foldouts.Count) foldouts.Add(false);
                foldouts[i] = EditorGUILayout.BeginFoldoutHeaderGroup(foldouts[i], $"{cat.Key} ({cat.Value.Count})");
                if (foldouts[i])
                {

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space(ident, false);
                    EditorGUILayout.BeginVertical();
                    cat.Value.ForEach(ShowGameEvent);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
                i++;
            }

            EditorGUILayout.EndScrollView();
        }

        private void ShowGameEvent(GameEvent ge)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(ge.EventName);
            GUILayout.Label("Listeners: "+ge.Listeners.Count(), GUILayout.Width(EditorGUIUtility.fieldWidth*1.5f));
            if (GUILayout.Button("Select", GUILayout.Width(EditorGUIUtility.fieldWidth*.9f)))
                UnityEditor.Selection.activeObject = ge;
            if (GUILayout.Button("Raise", GUILayout.Width(EditorGUIUtility.fieldWidth*.9f)))
                ge.Raise();
            EditorGUILayout.EndHorizontal();
        }
    }
}