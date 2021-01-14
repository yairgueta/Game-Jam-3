using System.Reflection;
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
            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            view = GUILayout.BeginScrollView(view, GUIStyle.none, new GUIStyle{},GUILayout.Width(Screen.width*.15f), GUILayout.Height(Screen.height*.75f));
            foreach (var settings in settingsObjects)
            {
                // GUILayout.BeginVertical();
                FieldInfo[] fields = settings.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var f in fields)
                {
                    GUILayout.Label(f.Name);
                }

                PropertyInfo[] properties = settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var p in properties)
                {
                    GUILayout.Label(p.Name);
                }
                // GUILayout.EndVertical();
            }

            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }
    }
}
