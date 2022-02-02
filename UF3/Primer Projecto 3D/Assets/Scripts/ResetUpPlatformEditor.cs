using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResetUpPlatform))]
public class ResetUpPlatformEditor : Editor
{
    ResetUpPlatform reset;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
        }

        if (GUILayout.Button("Reset Up"))
        {
            reset.Reset();
        }
    }

    private void OnEnable()
    {
        reset = (ResetUpPlatform)target;
    }
}
