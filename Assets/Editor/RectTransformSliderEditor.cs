using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RectTransformSlider))]
public class RectTransformSliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RectTransformSlider myScript = (RectTransformSlider)target;
        if (GUILayout.Button("Test"))
        {
            myScript.Expand();
        }
    }
}