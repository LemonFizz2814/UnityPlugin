using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEditor;

public class SetupEditor : EditorWindow
{
    string socialMediaURL;
    string textField;
    string currencyName;

    string add = "Add";
    string remove = "Remove";
    string buttonTitle = "";

    bool startEnabled;
    bool groupEnabled;

    float buttonScale = 1.0f;

    int buttonSelected = 0;
    int textSelected = 0;

    string[] buttonTypes = new string[7] { "Play", "Exit", "Shop", "Back", "SocialMedia", "Settings", "Advert" };
    string[] menuTypes = new string[5] { "GameOver", "Start", "Settings", "Shop", "GamePlayScreen" };
    string[] textTypes = new string[5] { "Title", "Currency", "Score", "Lives", "Info" };
    string[] textTypeAdd = new string[5] { "", ": ", "Score: ", "Lives: ", "" };

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
            AddMenu();
        }

        if (EditorGUILayout.BeginFadeGroup(Convert.ToInt32(startEnabled)))
        {
            EditorGUI.indentLevel++;

            //Add new button
            buttonSelected = EditorGUILayout.Popup("Button Type", buttonSelected, buttonTypes);
            buttonScale = EditorGUILayout.Slider("Scale", buttonScale, -3, 3);

            Sprite obj = null;
            obj = (Sprite)EditorGUI.ObjectField(new Rect(3, 70, position.width - 6, 20), "Button Sprite", obj, typeof(Sprite), false);

            if (EditorGUILayout.BeginFadeGroup(Convert.ToInt32(buttonSelected == 4))) //social media
            {
                socialMediaURL = EditorGUILayout.TextField("Social Media URL", "");
            }EditorGUILayout.EndFadeGroup();

            if (GUILayout.Button("Add New Button"))
            {
                AddButton(buttonSelected, 0);
            }

            EditorGUILayout.Space();
            //------------------------------------------------

            //Add new text
            textSelected = EditorGUILayout.Popup("Text Type", textSelected, textTypes);
            textField = EditorGUILayout.TextField("Text", "");

            if (EditorGUILayout.BeginFadeGroup(Convert.ToInt32(textSelected == 1))) //currency
            {
                currencyName = EditorGUILayout.TextField("Currency Name", "Currency");
            }
            EditorGUILayout.EndFadeGroup();
            if (GUILayout.Button("Add New Text"))
            {
                AddText(textSelected, 0, textField);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUI.indentLevel--;
        }EditorGUILayout.EndFadeGroup();

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
    }

    void AddMenu()
    {

    }

    void AddButton(int _type, int _menuType)
    {
        GameObject buttonPrefab = Instantiate(Resources.Load<GameObject>("ButtonPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
        buttonPrefab.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(_menuType);
    }

    void AddText(int _type, int _menuType, string _text)
    {
        GameObject textPrefab = Instantiate(Resources.Load<GameObject>("TextPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
        textPrefab.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(_menuType);
        if(_type ==  1) //currency
        {
            textPrefab.GetComponent<TextMeshProUGUI>().text += currencyName;
        }
        textPrefab.GetComponent<TextMeshProUGUI>().text += textTypeAdd[_type];

        textPrefab.GetComponent<TextMeshProUGUI>().text += _text;
    }
}
