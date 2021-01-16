using System;
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
        
        private SerializedProperty totalPool;
        private ReorderableList prefabsList;
        private SerializedProperty prefabsArray;
        private SerializedProperty percentageToSpawn;

        private SerializedProperty startingAmount;
        private SerializedProperty startWithPerlinNoise;
        private Rect objRect;
        private float total;

        private void OnEnable()
        {
            serializedObject.Update();
            spawner = target as Spawner;
            addIcon = EditorGUIUtility.IconContent("Toolbar Plus@2x");
            removeIcon = EditorGUIUtility.IconContent("Toolbar Minus@2x");
            totalPool = serializedObject.FindProperty("totalPoolAmount");

            startingAmount = serializedObject.FindProperty("startingAmount");
            startWithPerlinNoise = serializedObject.FindProperty("usePerlinNoise");

            prefabsArray = serializedObject.FindProperty("pooledPrefab");
            prefabsList = new ReorderableList(serializedObject, prefabsArray, true, true, true, true); 
            percentageToSpawn = serializedObject.FindProperty("percentageToSpawn");
            
            percentageToSpawn.arraySize = prefabsArray.arraySize;
            
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
            var remainingToPool = totalPool.intValue - spawner.CurrentPooled;
            base.OnInspectorGUI();
            total = 0;
            prefabsList.DoLayoutList();
            prefabsList.drawElementCallback = (rect, index, active, focused) =>
            {
                var serVal = percentageToSpawn.GetArrayElementAtIndex(index);
                objRect = new Rect(rect.position, new Vector2(rect.width * .4f, EditorGUIUtility.singleLineHeight));
                EditorGUI.PropertyField(objRect, prefabsArray.GetArrayElementAtIndex(index), GUIContent.none);
                serVal.floatValue = EditorGUI.Slider(new Rect(objRect.x + objRect.width+ rect.width * .04f, rect.y, rect.width * .55f, EditorGUIUtility.singleLineHeight),
                    serVal.floatValue, 0, 1);
                
                serVal.floatValue = Mathf.Min(serVal.floatValue, 1 - total);
                total = Mathf.Min(1f, total + serVal.floatValue);
            };
            
            percentageToSpawn.GetArrayElementAtIndex(percentageToSpawn.arraySize - 1).floatValue += 1 - total;
            
            DrawRandomNoiseField();

            DrawSpawnLine(remainingToPool, "Spawn Objects", spawner.SpawnRandom, ref ranAmount);
            DrawSpawnLine(remainingToPool, "Spawn Noise", spawner.SpawnNoise, ref noiseAmount);
            
            DrawRespawnsButtons();

            MessageType msgT;
            if (remainingToPool > 0) msgT = MessageType.Info;
            else if (remainingToPool == 0) msgT = MessageType.Warning;
            else msgT = MessageType.Error;
            EditorGUILayout.HelpBox("Pooled " + spawner.CurrentPooled + " out of " + totalPool.intValue +".\n"+spawner.CurrentPooled+"/" + totalPool.intValue, msgT);

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

        private void DrawRespawnsButtons()
        {
            if (GUILayout.Button("Respawn All (" + spawner.CurrentPooled + ")"))
            {
                spawner.DespawnAll();
                if (startWithPerlinNoise.boolValue) spawner.SpawnNoise(totalPool.intValue);
                else spawner.SpawnRandom(totalPool.intValue);
            }

            if (GUILayout.Button("Despawn All (" + spawner.CurrentPooled + ")")) spawner.DespawnAll();
        }
    }
}