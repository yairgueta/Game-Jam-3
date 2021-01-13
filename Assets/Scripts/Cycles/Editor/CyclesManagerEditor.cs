using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace Cycles.Editor
{
    [CustomEditor(typeof(CyclesManager))]
    public class CyclesManagerEditor : UnityEditor.Editor
    {
        private SerializedProperty cycleOrder;
        private void OnEnable()
        {
            cycleOrder = serializedObject.FindProperty("cycleOrder");
            
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            

            serializedObject.ApplyModifiedProperties();
        }
    }
}