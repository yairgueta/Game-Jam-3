using UnityEditor;
using UnityEngine;

namespace Selections
{
    [AddComponentMenu("Selectable")]
    public class SelectableInitializerComponent : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Reset()
        {
            var child = new GameObject("Selectable", typeof(BoxCollider2D));
            
            child.transform.parent = transform;
            child.transform.localPosition = Vector3.zero;
            child.transform.localRotation = Quaternion.identity;

            child.layer = LayerMask.NameToLayer("Selectable");

            var sr = GetComponent<SpriteRenderer>();
            child.AddComponent<Selectable>().spriteRenderer = sr;
            
            
            Selection.activeGameObject = child;
            DestroyImmediate(this);
        }
#endif
        
    }
}