using System;
using System.Reflection;
using UnityEditor;
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
            var scrollViewSize = new Vector2(Screen.width * .15f, Screen.height * .75f);
            // view.y = GUILayout.VerticalScrollbar(view.y, scrollViewSize.y, 0, 500f);
            view = GUILayout.BeginScrollView(view, GUIStyle.none,GUILayout.Width(scrollViewSize.x),
                GUILayout.Height(scrollViewSize.y));
            foreach (var settings in settingsObjects)
            {
                FieldInfo[] fields = settings.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var f in fields)
                {
                    GUILayout.Label(f.Name + f.GetValue(settings));
                }
            
                
                PropertyInfo[] properties =
                    settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var p in properties)
                {
                    GUILayout.Label(p.Name + p.GetValue(settings));
                }
            }
            GUILayout.EndScrollView();
            
            
            
        }

        void DisplayProperty(object o, FieldInfo i)
        {
            if (i.FieldType == typeof(int))
            {
                i.GetValue(o);
                // i.
            }
            
            
            
        }
    }
}
