using NUnit.Framework;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

namespace Cycles.Editor
{
    [CustomEditor(typeof(CyclesManager))]
    public class CyclesManagerEditor : UnityEditor.Editor
    {
        private string[] cycleNames = {"Day", "Night", "Eclipse"};
        
        private SerializedProperty cyclesSettings;
        private ReorderableList cyclesOrder;
        private GUIStyle headerStyle;
        private void OnEnable()
        {
            cyclesOrder =  new ReorderableList(serializedObject, serializedObject.FindProperty("cyclesOrder"), true, true, false, false);
            cyclesSettings = serializedObject.FindProperty("cyclesSettings");
            cyclesSettings.arraySize = 3;
            headerStyle = new GUIStyle{fontSize = 11, fontStyle = FontStyle.Bold, normal = {background = null, textColor = Color.white}};

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            cyclesOrder.DoLayoutList();
            cyclesOrder.drawElementCallback = (rect, index, active, focused) =>
            {
                var e = cyclesOrder.serializedProperty.GetArrayElementAtIndex(index).intValue;
                EditorGUI.LabelField(new Rect(rect.position, new Vector2(50, EditorGUIUtility.singleLineHeight)), cycleNames[e]);
                EditorGUI.PropertyField(new Rect(rect.x+50, rect.y, rect.width-50, EditorGUIUtility.singleLineHeight), 
                    cyclesSettings.GetArrayElementAtIndex(e), GUIContent.none);
            };
            cyclesOrder.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Cycles Order and ScriptableObjects Settings", headerStyle);
            serializedObject.ApplyModifiedProperties();
            DrawDefaultInspector();
        }
    }
}