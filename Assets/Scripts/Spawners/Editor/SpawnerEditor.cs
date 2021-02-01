using System;
using Pathfinding;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditorInternal;
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
        
        private ReorderableList prefabsList;
        private SerializedProperty pooledPrefabs;
        private SerializedProperty percentageToSpawn;

        private SerializedProperty startingAmount;
        private SerializedProperty startWithPerlinNoise;
        private Rect objRect;
        private float total;

        private SerializedProperty useOverFrameSpawning;
        private SerializedProperty overFrameTimeout;
            
        private void OnEnable()
        {
            serializedObject.Update();
            spawner = target as Spawner;
            
            addIcon = EditorGUIUtility.IconContent("Toolbar Plus@2x");
            removeIcon = EditorGUIUtility.IconContent("Toolbar Minus@2x");

            startingAmount = serializedObject.FindProperty("startingAmount");
            startWithPerlinNoise = serializedObject.FindProperty("usePerlinNoise");

            pooledPrefabs = serializedObject.FindProperty("pooledPrefabs");
            prefabsList = new ReorderableList(serializedObject, pooledPrefabs, true, true, true, true); 
            percentageToSpawn = serializedObject.FindProperty("percentageToSpawn");
            
            useOverFrameSpawning = serializedObject.FindProperty("useOverFrameSpawning");
            overFrameTimeout = serializedObject.FindProperty("overFrameTimeout");
            
            percentageToSpawn.arraySize = pooledPrefabs.arraySize;
            
            prefabsList.onAddCallback += list =>
            {
                list.serializedProperty.arraySize++;
                percentageToSpawn.arraySize = list.serializedProperty.arraySize;
            };

            prefabsList.onRemoveCallback += list =>
            {
                list.serializedProperty.arraySize--;
                percentageToSpawn.arraySize = list.serializedProperty.arraySize;
            };
            
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            // base.OnInspectorGUI();

            
            
            EditorGUILayout.PropertyField(useOverFrameSpawning);
            if (useOverFrameSpawning.boolValue) EditorGUILayout.PropertyField(overFrameTimeout);
            
            
            total = 0;
            prefabsList.DoLayoutList();
            prefabsList.drawElementCallback = (rect, index, active, focused) =>
            {
                var serVal = percentageToSpawn.GetArrayElementAtIndex(index);
                objRect = new Rect(rect.position, new Vector2(rect.width * .4f, EditorGUIUtility.singleLineHeight));
                EditorGUI.PropertyField(objRect, pooledPrefabs.GetArrayElementAtIndex(index), GUIContent.none);
                serVal.floatValue = EditorGUI.Slider(new Rect(objRect.x + objRect.width+ rect.width * .04f, rect.y, rect.width * .55f, EditorGUIUtility.singleLineHeight),
                    serVal.floatValue, 0, 1);
                
                serVal.floatValue = Mathf.Min(serVal.floatValue, 1 - total);
                total = Mathf.Min(1f, total + serVal.floatValue);
            };
            prefabsList.drawHeaderCallback += rect => EditorGUI.DropShadowLabel(rect, "Prefabs List and Percentages"); 
            
            percentageToSpawn.GetArrayElementAtIndex(percentageToSpawn.arraySize - 1).floatValue += 1 - total;
            
            DrawRandomNoiseField();

            // DrawSpawnLine(remainingToPool, "Spawn Objects", i => spawner.SpawnRandom(i, false), ref ranAmount);
            // DrawSpawnLine(remainingToPool, "Spawn Noise", i=>spawner.SpawnNoise(i, false), ref noiseAmount);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSpawnLine(int remainingToPool, string header, Action<int> spawnFunction, ref int amount)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(header, GUILayout.Width(EditorGUIUtility.labelWidth));
            amount = EditorGUILayout.IntField(amount, GUILayout.Width(EditorGUIUtility.fieldWidth*0.75f));
            if (GUILayout.Button(addIcon, GUILayout.Height(EditorGUIUtility.singleLineHeight),
                GUILayout.Width(EditorGUIUtility.singleLineHeight + 1)))
                amount++;
            if (GUILayout.Button(removeIcon, GUILayout.Height(EditorGUIUtility.singleLineHeight),
                GUILayout.Width(EditorGUIUtility.singleLineHeight + 1)))
                amount--;
            ranAmount = Mathf.Clamp(ranAmount, 0, remainingToPool);
            if (GUILayout.Button("Spawn")) spawnFunction(amount);

            EditorGUILayout.EndHorizontal();
        }

        private void DrawRandomNoiseField()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(startingAmount);
            EditorGUILayout.LabelField("Noise", GUILayout.Width(EditorGUIUtility.fieldWidth * 0.75f));
            EditorGUILayout.PropertyField(startWithPerlinNoise, GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }

    }
}