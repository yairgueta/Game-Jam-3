using System;
using System.Text;
using UnityEngine;

namespace UI
{
    public class FPSDisplay : MonoBehaviour
    {
        float deltaTime = 0.0f;
        GUIStyle style = new GUIStyle();
        private Rect rect;
        // private StringBuilder sb;

        private void Start()
        {
            int w = Screen.width, h = Screen.height;
            rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = Color.white;
            // sb = new StringBuilder("{0:0.0} ms ({1:0.} fps)");
        }

        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }
 
        void OnGUI()
        {
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            // string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            var sb = new StringBuilder(18,18);
            sb.AppendFormat("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, sb.ToString(), style);
        }
    }
}