using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FastShadow))]
public class EditorBtn : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FastShadow screenshot = (FastShadow)target;

        if (GUILayout.Button("Capture"))
        {
            screenshot.CaptureAndSetAsSprite();
        }
    }
}

