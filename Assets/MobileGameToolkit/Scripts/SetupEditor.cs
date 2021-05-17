using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class SetupEditor : EditorWindow
{
    string socialMediaURL;
    bool startEnabled;
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    string add = "Add";
    string remove = "Remove";

    string buttonTitle = "";

    [MenuItem("Window/Mobile Setup")]
    public static void ShowWindow()
    {
        GetWindow<SetupEditor>("Setup Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Menu setup", EditorStyles.boldLabel);

        if(GUILayout.Button(buttonTitle + " Start Menu"))
        {
            buttonTitle = (startEnabled) ? add : remove;
            startEnabled = !startEnabled;
            Debug.Log("pressed");
        }
        EditorGUILayout.BeginFadeGroup(Convert.ToInt32(startEnabled));
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndFadeGroup();

        if (GUILayout.Button("Add Shop Menu"))
        {
            Debug.Log("pressed");
        }
        if (GUILayout.Button("Add Settings Menu"))
        {
            Debug.Log("pressed");
        }
        if (GUILayout.Button("Add GameOver Menu"))
        {
            Debug.Log("pressed");
        }
        socialMediaURL = EditorGUILayout.TextField("Social Media URL", "");
    }
}
