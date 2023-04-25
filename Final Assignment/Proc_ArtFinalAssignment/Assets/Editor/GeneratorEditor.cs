using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AGeneratable), true)]
public class GeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AGeneratable generator = (AGeneratable)target;
        if (GUILayout.Button("Regenerate"))
        {
            generator.DeGenerate();
            generator.Generate();
        }
    }
}
