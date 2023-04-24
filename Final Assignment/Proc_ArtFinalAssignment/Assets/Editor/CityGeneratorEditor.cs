using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CityGenerator))]
public class CityGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CityGenerator generator = (CityGenerator)target;
        if (GUILayout.Button("Regenerate"))
        {
            generator.UnGenerate();
            generator.Generate();
        }
    }
}
