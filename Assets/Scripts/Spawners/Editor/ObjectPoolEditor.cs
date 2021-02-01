using System;
using UnityEditor;
using UnityEngine;

namespace Spawners.Editor
{
    [CustomEditor(typeof(ObjectPool))]
    public class ObjectPoolEditor : UnityEditor.Editor
    {
        private ObjectPool objectPool;
        private GameObject prefab;
        
        private SerializedProperty pooledPrefab;
        private SerializedProperty totalPool;
        
        private void Awake()
        {
            objectPool = target as ObjectPool;

            pooledPrefab = serializedObject.FindProperty("pooledPrefab");
            totalPool = serializedObject.FindProperty("totalPoolAmount");
            
            prefab = pooledPrefab.objectReferenceValue as GameObject;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button($"Instantiate All Pool ({totalPool.intValue - objectPool.transform.childCount})"))
            {
                pooledPrefab = serializedObject.FindProperty("pooledPrefab");
                prefab = pooledPrefab.objectReferenceValue as GameObject;

                for (int i = objectPool.transform.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(objectPool.transform.GetChild(i).gameObject);
                }

                for (int i = 0; i < totalPool.intValue; i++)
                {
                    PrefabUtility.InstantiatePrefab(prefab, objectPool.transform);
                }
                foreach (Transform child in objectPool.transform) child.gameObject.SetActive(false);
            }
            
            
            EditorGUILayout.HelpBox($"Pooled {objectPool.transform.childCount} out of {totalPool}." +
                                    $"\nThe rest will be instantiated during gameplay!", MessageType.Info);

            
        }
    }
}