using System;
using UnityEditor;
using UnityEngine;

namespace Spawners.Editor
{
    [CustomEditor(typeof(Spawner)), CanEditMultipleObjects]
    public class SpawnerEditor : UnityEditor.Editor
    {
        private Spawner spawner;
        private int ranAmount;
        private int noiseAmount;
        private GUIContent addIcon;
        private GUIContent removeIcon;
        private SerializedProperty totalPool;

        private SerializedProperty startingAmount;
        private SerializedProperty startWithPerlinNoise;

        private void OnEnable()
        {
            spawner = target as Spawner;
            addIcon = EditorGUIUtility.IconContent("Toolbar Plus@2x");
            removeIcon = EditorGUIUtility.IconContent("Toolbar Minus@2x");
            totalPool = serializedObject.FindProperty("totalPoolAmount");

            startingAmount = serializedObject.FindProperty("startingAmount");
            startWithPerlinNoise = serializedObject.FindProperty("usePerlinNoise");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var remainingToPool = totalPool.intValue - spawner.CurrentPooled;
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(startingAmount);
            EditorGUILayout.LabelField("Noise", GUILayout.Width(EditorGUIUtility.fieldWidth*0.75f));
            EditorGUILayout.PropertyField(startWithPerlinNoise, GUIContent.none);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Spawn Objects", GUILayout.Width(EditorGUIUtility.labelWidth));
            ranAmount = EditorGUILayout.IntField(ranAmount, GUILayout.Width(EditorGUIUtility.fieldWidth*0.75f));
            if (GUILayout.Button(addIcon, GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(EditorGUIUtility.singleLineHeight + 1)))
                ranAmount++;
            if (GUILayout.Button(removeIcon,  GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(EditorGUIUtility.singleLineHeight + 1)))
                ranAmount--;
            ranAmount = Mathf.Clamp(ranAmount, 0, remainingToPool);
            if (GUILayout.Button("Spawn"))
            {
                spawner.SpawnRandom(ranAmount);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Spawn Noise", GUILayout.Width(EditorGUIUtility.labelWidth));
            noiseAmount = EditorGUILayout.IntField(noiseAmount, GUILayout.Width(EditorGUIUtility.fieldWidth*0.75f));
            if (GUILayout.Button(addIcon, GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(EditorGUIUtility.singleLineHeight + 1)))
                noiseAmount++;
            if (GUILayout.Button(removeIcon,  GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(EditorGUIUtility.singleLineHeight + 1)))
                noiseAmount--;
            noiseAmount = Mathf.Clamp(noiseAmount, 0, remainingToPool);
            if (GUILayout.Button("Spawn"))
            {
                spawner.SpawnNoise(noiseAmount);
            }
            EditorGUILayout.EndHorizontal();


            if (GUILayout.Button("Respawn All (" + spawner.CurrentPooled + ")"))
            {
                spawner.DespawnAll();
                if(startWithPerlinNoise.boolValue) spawner.SpawnNoise(totalPool.intValue);
                else spawner.SpawnRandom(totalPool.intValue);
            }
            if (GUILayout.Button("Despawn All (" + spawner.CurrentPooled + ")")) spawner.DespawnAll();

            MessageType msgT;
            if (remainingToPool > 0) msgT = MessageType.Info;
            else if (remainingToPool == 0) msgT = MessageType.Warning;
            else msgT = MessageType.Error;
            EditorGUILayout.HelpBox("Pooled " + spawner.CurrentPooled + " out of " + totalPool.intValue +".\n"+spawner.CurrentPooled+"/" + totalPool.intValue, msgT);

            serializedObject.ApplyModifiedProperties();
        }
    }
}