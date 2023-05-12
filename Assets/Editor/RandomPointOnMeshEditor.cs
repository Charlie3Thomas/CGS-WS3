using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomPointOnMesh))]
public class RandomPointOnMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RandomPointOnMesh script = (RandomPointOnMesh)target;
        if (GUILayout.Button("Generate Buildings"))
        {
            script.GenerateRandomPositions();
        }
    }
}
