using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardGenerator))]
public class BoardEditor : Editor
{   
    private int size = 4;
    private int depth = 5;
    private bool connector = true;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BoardGenerator boardGenerator = (BoardGenerator)target;

        size = EditorGUILayout.IntField("Size", size);
        depth = EditorGUILayout.IntField("Depth", depth);
        connector = EditorGUILayout.Toggle("Connector", connector);

        if(GUILayout.Button("Generate")) {
            boardGenerator.Create(size, depth, connector);
        }

        if(GUILayout.Button("Generate Random")) {
            boardGenerator.CreateRandom();
        }
    }
}
