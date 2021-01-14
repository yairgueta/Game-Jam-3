using System;
using System.Reflection;
using Pathfinding;
using UnityEngine;

namespace UI
{
    public class AttributesUI : MonoBehaviour
    {
        [SerializeField] private ScriptableObject[] settingsObjects;
        private Vector2 view;

        private void OnGUI()
        {
            GUILayout.Space(100);
            // GUILayout.BeginHorizontal();
            view = GUILayout.BeginScrollView(view, GUIStyle.none, new GUIStyle{alignment = TextAnchor.UpperLeft},GUILayout.Width(Screen.width*.15f), GUILayout.Height(Screen.height*.75f));
            foreach (var settings in settingsObjects)
            {
                // GUILayout.BeginVertical();
                FieldInfo[] fields = settings.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var f in fields)
                {
                    GUILayout.Label(f.Name);
                }

                PropertyInfo[] propeties = settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var p in propeties)
                {
                    GUILayout.Label(p.Name);
                }
                // GUILayout.EndVertical();
            }
            // GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }
    }
}
