using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SetupEditor : EditorWindow
{
    string socialMediaURL;
    string textField;
    string buttonTextField;
    string currencyName;

    string add = "Add";
    string remove = "Remove";
    string buttonTitle = "";

    bool startEnabled;
    bool groupEnabled;
    bool showPosition;

    float buttonScale = 1.0f;
    float textScale = 1.0f;

    int buttonSelected = 0;
    int textSelected = 0;
    int toolbarInt = 0;

    TMP_FontAsset textFont;

    Sprite buttonSprite;

    string[] buttonTypes = new string[7] { "Play", "Exit", "Shop", "Back", "SocialMedia", "Settings", "Advert" };
    string[] menuTypes = new string[5] { "GameOver", "Start", "Settings", "Shop", "GamePlayScreen" };
    string[] textTypes = new string[5] { "Title", "Currency", "Score", "Lives", "Info" };
    string[] textTypeAdd = new string[5] { "", ": ", "Score: ", "Lives: ", "" };

    string[] toolbarStrings = { "Add Menus setup", "Add Buttons setup", "Add Gameplay menu" };

    List<GameObject> buttonList = new List<GameObject>();
    List<bool> buttonListDropDown = new List<bool>();

    [MenuItem("Window/Mobile Setup")]
    public static void ShowWindow()
    {
        GetWindow<SetupEditor>("Setup Editor");
    }

    private void OnGUI()
    {
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);
        switch (toolbarInt)
        {
            case 0: //menu setup
                GUILayout.Label("Menu setup", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                if (GUILayout.Button(buttonTitle + " Start Menu"))
                {
                    //ShowNotification(new GUIContent("Start Menu Added"));

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


                    //obj = (Sprite)EditorGUI.ObjectField(new Rect(3, 70, position.width - 6, 20), "Button Sprite", obj, typeof(Sprite), false);
                    buttonSprite = (Sprite)EditorGUILayout.ObjectField("Button Sprite", buttonSprite, typeof(Sprite), true);

                    buttonTextField = EditorGUILayout.TextField("Button Text", "new text");

                    if (EditorGUILayout.BeginFadeGroup(Convert.ToInt32(buttonSelected == 4))) //social media
                    {
                        socialMediaURL = EditorGUILayout.TextField("Social Media URL", "");
                    }
                    EditorGUILayout.EndFadeGroup();

                    if (GUILayout.Button("Add New Button"))
                    {
                        AddButton(buttonSelected, 0, buttonScale, buttonTextField, buttonSprite);
                    }

                    EditorGUILayout.Space();
                    //------------------------------------------------

                    //Add new text
                    textSelected = EditorGUILayout.Popup("Text Type", textSelected, textTypes);
                    textField = EditorGUILayout.TextField("Text", "new text");
                    textScale = EditorGUILayout.Slider("Font size", textScale, 0, 400);
                    textFont = (TMP_FontAsset)EditorGUILayout.ObjectField("Button Sprite", textFont, typeof(TMP_FontAsset), true);

                    if (EditorGUILayout.BeginFadeGroup(Convert.ToInt32(textSelected == 1))) //currency
                    {
                        currencyName = EditorGUILayout.TextField("Currency Name", "Currency");
                    }
                    EditorGUILayout.EndFadeGroup();
                    if (GUILayout.Button("Add New Text"))
                    {
                        AddText(textSelected, 0, textField, textScale);
                    }

                    EditorGUILayout.Space();
                    var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                    EditorGUILayout.LabelField("------------------------------------------------------------------------", style, GUILayout.ExpandWidth(true));
                    EditorGUILayout.Space();
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFadeGroup();

                if (GUILayout.Button("Add Shop Menu"))
                {
                    Debug.Log("pressed");
                }
                if (GUILayout.Button("Add Settings Menu"))
                {
                    Debug.Log("pressed");
                }
                break;

            case 1: //button setup
                GUILayout.Label("Buttons Menu", EditorStyles.boldLabel);
                for(int i = 0; i < buttonList.Count; i++)
                {
                    GUILayout.Label("Button number " + i, EditorStyles.boldLabel);

                    buttonListDropDown[i] = EditorGUILayout.Foldout(showPosition, "Expand");
                    if (buttonListDropDown[i])
                    {
                        EditorGUI.indentLevel++;
                        GUILayout.Label("rawr XD", EditorStyles.label);
                        EditorGUI.indentLevel--;
                    }
                }

                if (GUILayout.Button("Add to list"))
                {
                    buttonList.Add(AddButton(buttonSelected, 0, 1, buttonTextField, buttonSprite));
                    buttonListDropDown.Add(false);
                }
                if (GUILayout.Button("Clear"))
                {
                    for (int i = 0; i < buttonList.Count; i++)
                    {
                        DestroyImmediate(buttonList[i]);
                    }
                    buttonList.Clear();
                }

                break;

            case 2: //gameplay setup
                GUILayout.Label("Gameplay Menu", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                if (GUILayout.Button("Add Player"))
                {
                    Debug.Log("added player");
                    Instantiate(Resources.Load<GameObject>("PlayerPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
                }
                if (GUILayout.Button("Add Enemy"))
                {
                    Debug.Log("added enemy");
                    Instantiate(Resources.Load<GameObject>("EnemyPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
                }
                break;
        }

    }

    GameObject AddButton(int _type, int _menuType, float _scale, string _text, Sprite _sprite)
    {
        GameObject buttonPrefab = Instantiate(Resources.Load<GameObject>("ButtonPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
        buttonPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(_menuType));
        buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _text;

        buttonPrefab.GetComponent<ButtonScript>().SetType((CanvasManager.ButtonType)_type);

        buttonPrefab.GetComponent<Image>().sprite = (_sprite == null) ? Resources.Load<Sprite>("DefaultSprite") : _sprite;
        buttonPrefab.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        buttonPrefab.GetComponent<RectTransform>().localScale = new Vector3(_scale, _scale, 1);

        return buttonPrefab;
    }

    void AddText(int _type, int _menuType, string _text, float _size)
    {
        GameObject textPrefab = Instantiate(Resources.Load<GameObject>("TextPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
        textPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(_menuType));
        if(_type ==  1) //currency
        {
            textPrefab.GetComponent<TextMeshProUGUI>().text += currencyName;
        }
        textPrefab.GetComponent<TextMeshProUGUI>().text += textTypeAdd[_type];

        textPrefab.GetComponent<TextMeshProUGUI>().text += _text;

        textPrefab.GetComponent<TextMeshProUGUI>().fontSize = _size;
    }

    void AddMenu()
    {

    }
}