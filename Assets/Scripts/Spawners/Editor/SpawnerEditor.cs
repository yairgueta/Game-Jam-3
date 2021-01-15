using System;
using UnityEditor;
using UnityEngine;

namespace Spawners.Editor
{
    [CustomEditor(typeof(Spawner))]
    public class SpawnerEditor : UnityEditor.Editor
    {
        private Spawner spawner;
        private int amount;
        private GUIContent addIcon;
        private GUIContent removeIcon;
        private SerializedProperty totalPool;

        private void OnEnable()
        {
            spawner = target as Spawner;
            addIcon = EditorGUIUtility.IconContent("Toolbar Plus@2x");
            removeIcon = EditorGUIUtility.IconContent("Toolbar Minus@2x");
            totalPool = serializedObject.FindProperty("totalPoolAmount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Spawn Objects", GUILayout.Width(EditorGUIUtility.labelWidth));
            amount = EditorGUILayout.IntField(amount, GUILayout.Width(EditorGUIUtility.fieldWidth*0.75f));
            if (GUILayout.Button(addIcon, GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(EditorGUIUtility.singleLineHeight + 1)))
                amount++;
            if (GUILayout.Button(removeIcon,  GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(EditorGUIUtility.singleLineHeight + 1)))
                amount--;
            amount = Mathf.Clamp(amount, 0, totalPool.intValue - spawner.CurrentPooled);
            if (GUILayout.Button("Spawn"))
            {
                spawner.SpawnRandom(amount);
                amount = 0;
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Despawn All (" + spawner.CurrentPooled + ")")) spawner.DespawnAll();

            MessageType msgT;
            if (totalPool.intValue - spawner.CurrentPooled > 0) msgT = MessageType.Info;
            else if (totalPool.intValue - spawner.CurrentPooled == 0) msgT = MessageType.Warning;
            else msgT = MessageType.Error;
            EditorGUILayout.HelpBox("Pooled " + spawner.CurrentPooled + " out of " + totalPool.intValue +".\n"+spawner.CurrentPooled+"/" + totalPool.intValue, msgT);

            serializedObject.ApplyModifiedProperties();
        }
    }
}