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
    string currencyName;

    float textScale = 1.0f;
    float playerJump = 400.0f;

    int buttonSelected = 0;
    int textSelected = 0;
    int gameSelected = 0;
    int toolbarInt = 0;
    int enemyDamage = 0;
    int coinPoints = 0;
    int playerLives = 0;
    int enemySpawnFrequency = 0;
    int coinSpawnFrequency = 0;
    int lootBoxPrice = 100;

    TMP_FontAsset textFont;

    Sprite buttonSprite;
    Sprite playerSprite;
    Sprite enemySprite;
    Sprite coinSprite;
    Sprite platformSprite;

    string[] buttonTypes = new string[8] { "Play", "Exit", "Shop", "Back", "SocialMedia", "Settings", "Advert", "Lootbox" };
    string[] buttonInfo = new string[8] { "Starts game when presed", "Exits application when pressed", "Opens shop menu when pressed", "Goes back to start menu when pressed", "Opens a link to social media when pressed", "Opens the setting menu when pressed", "Plays an advertisement when pressed", "Opens a lootbox when pressed" };
    string[] menuTypes = new string[5] { "Start", "GameOver", "Settings", "Shop", "GamePlay" };
    string[] menuTitle = new string[5] { "Add", "Add", "Add", "Add", "Add" };
    bool[] startEnabled = new bool[5] { false, false, false, false, false };
    string[] textTypes = new string[4] { "Info", "Currency", "Score", "Lives" };
    string[] textTypeAdd = new string[5] { "", ": ", "Score: ", "Lives: ", "" };
    string[] gameType = new string[2] { "Endless Runner", "Own Game~" };

    string[] toolbarStrings = new string[4] { "Menus setup", "Buttons setup", "Gameplay menu", "Lootbox setup" };

    public struct LootBoxItem
    {
        public int chances;
        public Sprite sprite;
        public bool beenPurchased;

        public LootBoxItem(int _chances, Sprite _sprite, bool _beenPurchased)
        {
            this.chances = _chances;
            this.sprite = _sprite;
            this.beenPurchased = _beenPurchased;
        }
    }

    List<LootBoxItem> lootboxList = new List<LootBoxItem>();

    List<GameObject> buttonList = new List<GameObject>();
    List<bool> buttonListDropDown = new List<bool>();
    List<bool> lootboxDropDown = new List<bool>();
    List<string> buttonTextField = new List<string>();
    List<float> buttonScale = new List<float>();

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

                /*if (GUILayout.Button(buttonTitle + " Start Menu"))
                {
                    //ShowNotification(new GUIContent("Start Menu Added"));

                    buttonTitle = (startEnabled) ? add : remove;
                    startEnabled = !startEnabled;
                }

                if (EditorGUILayout.BeginFadeGroup(Convert.ToInt32(startEnabled)))
                {
                    EditorGUI.indentLevel++;

                    //ShowButtonEditOptions();
                    //------------------------------------------------

                    //Add new text
                    ShowTextEditOptions();

                    EditorGUILayout.Space();
                    var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                    EditorGUILayout.LabelField("------------------------------------------------------------------------", style, GUILayout.ExpandWidth(true));
                    EditorGUILayout.Space();
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFadeGroup();*/

                for (int i = 0; i < menuTypes.Length; i++)
                {
                    if (GUILayout.Button("" + menuTitle[i] + " " + menuTypes[i] + " Menu"))
                    {
                        AddMenu(i);
                        menuTitle[i] = (startEnabled[i]) ? "Add" : "Remove";
                        startEnabled[i] = !startEnabled[i];
                    }
                }

                EditorGUILayout.Space();

                if (GUILayout.Button("Add New Menu Type"))
                {
                    Debug.Log("pressed");
                }
                break;

            case 1: //button setup
                GUILayout.Label("Buttons Menu", EditorStyles.boldLabel);
                for (int i = 0; i < buttonList.Count; i++)
                {
                    buttonListDropDown[i] = EditorGUILayout.Foldout(buttonListDropDown[i], "Button " + i);
                    if (buttonListDropDown[i])
                    {
                        EditorGUI.indentLevel++;
                        ShowButtonEditOptions(i);

                        if (GUILayout.Button("Delete Button " + i))
                        {
                            DestroyButton(i);
                        }
                        if (GUILayout.Button("UpdateButton"))
                        {
                            UpdateButton(buttonSelected, buttonScale[i], buttonTextField[i], buttonSprite, buttonList[i]);
                        }
                        EditorGUI.indentLevel--;
                    }
                }

                EditorGUILayout.Space();

                if (GUILayout.Button("Add New Button"))
                {
                    buttonList.Add(AddButton(buttonSelected, 0, 1, "New Text", buttonSprite));
                }
                if (GUILayout.Button("Clear All"))
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

                gameSelected = EditorGUILayout.Popup("Game Type", gameSelected, gameType);

                EditorGUILayout.Space();

                // EDIT PLAYER
                GUILayout.Label("Edit Player", EditorStyles.largeLabel);
                EditorGUI.indentLevel++;
                playerLives = EditorGUILayout.IntField("Lives", playerLives);
                playerJump = EditorGUILayout.FloatField("Jump Height", playerJump);
                playerSprite = (Sprite)EditorGUILayout.ObjectField("Player Sprite", playerSprite, typeof(Sprite), true);
                EditorGUI.indentLevel--;
                //EditorGUILayout.Space();

                // EDIT ENEMY
                GUILayout.Label("Edit Enemy", EditorStyles.largeLabel);
                EditorGUI.indentLevel++;
                enemyDamage = EditorGUILayout.IntField("Damage", enemyDamage);
                enemySpawnFrequency = EditorGUILayout.IntField("Spawn Frequency", enemySpawnFrequency);
                enemySprite = (Sprite)EditorGUILayout.ObjectField("Enemy Sprite", enemySprite, typeof(Sprite), true);
                EditorGUI.indentLevel--;
                //EditorGUILayout.Space();

                // EDIT COIN
                GUILayout.Label("Edit Coin", EditorStyles.largeLabel);
                EditorGUI.indentLevel++;
                coinPoints = EditorGUILayout.IntField("Score Awarded", coinPoints);
                coinSpawnFrequency = EditorGUILayout.IntField("Spawn Frequency", coinSpawnFrequency);
                coinSprite = (Sprite)EditorGUILayout.ObjectField("Coin Sprite", coinSprite, typeof(Sprite), true);
                EditorGUI.indentLevel--;
                //EditorGUILayout.Space();

                // EDIT PLATFORM
                GUILayout.Label("Edit Platforms", EditorStyles.largeLabel);
                EditorGUI.indentLevel++;
                platformSprite = (Sprite)EditorGUILayout.ObjectField("Platform Sprite", platformSprite, typeof(Sprite), true);
                EditorGUI.indentLevel--;
                //EditorGUILayout.Space();

                /*if (GUILayout.Button("Add Player"))
                {
                    Debug.Log("added player");
                    Instantiate(Resources.Load<GameObject>("PlayerPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
                }*/
                break;
            case 3: //lootbox setup
                GUILayout.Label("Lootbox Setup", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                ShowLootBoxEditOptions();
                break;
        }
    }


    // OPTIONS

    void ShowButtonEditOptions(int _i)
    {
        //Add new button

        buttonSelected = EditorGUILayout.Popup("Button Type", buttonSelected, buttonTypes);
        GUILayout.Label(buttonInfo[buttonSelected], EditorStyles.label);
        buttonScale[_i] = EditorGUILayout.Slider("Scale", buttonScale[_i], 0, 4);

        buttonSprite = (Sprite)EditorGUILayout.ObjectField("Button Sprite", buttonSprite, typeof(Sprite), true);

        buttonTextField[_i] = EditorGUILayout.TextField("Button Text", buttonTextField[_i]);

        //additional
        if (EditorGUILayout.BeginFadeGroup(Convert.ToInt32(buttonSelected == 4))) //social media
        {
            GUILayout.Label("Social Media Options", EditorStyles.label);
            socialMediaURL = EditorGUILayout.TextField("Social Media URL", "");
        }
        EditorGUILayout.EndFadeGroup();
        if (EditorGUILayout.BeginFadeGroup(Convert.ToInt32(buttonSelected == 7))) //lootbox
        {
            GUILayout.Label("Lootbox Options", EditorStyles.label);
            ShowLootBoxEditOptions();
        }
        EditorGUILayout.EndFadeGroup();

        EditorGUILayout.Space();
    }

    void ShowTextEditOptions()
    {
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
    }

    void ShowLootBoxEditOptions()
    {
        lootBoxPrice = EditorGUILayout.IntField("Price for lootbox", lootBoxPrice);

        if (GUILayout.Button("Add New Item To Lootbox"))
        {
            lootboxList.Add(new LootBoxItem(1, null, false));
            lootboxDropDown.Add(false);
        }

        int totalPercentage = 0;

        for (int i = 0; i < lootboxList.Count; i++)
        {
            totalPercentage += lootboxList[i].chances;
        }

        for (int i = 0; i < lootboxList.Count; i++)
        {
            lootboxDropDown[i] = EditorGUILayout.Foldout(lootboxDropDown[i], "Item " + i);
            if (lootboxDropDown[i])
            {
                EditorGUI.indentLevel++;
                //int ss = EditorGUILayout.IntField("Chance", lootboxList[i].chances);
                lootboxList[i] = new LootBoxItem(EditorGUILayout.IntField("Chance " + lootboxList[i].chances + "/" + totalPercentage, lootboxList[i].chances), (Sprite)EditorGUILayout.ObjectField("Player Sprite", lootboxList[i].sprite, typeof(Sprite), true), lootboxList[i].beenPurchased);

                if (GUILayout.Button("Delete Item " + i))
                {
                    lootboxDropDown.RemoveAt(i);
                    lootboxList.RemoveAt(i);
                }
                EditorGUI.indentLevel--;
            }
        }
        if (GUILayout.Button("Clear All Lootbox"))
        {
            lootboxList.Clear();
            lootboxDropDown.Clear();
        }

        EditorGUILayout.Space();
    }


    // BUTTONS

    void UpdateButton(int _type, float _scale, string _text, Sprite _sprite, GameObject _buttonPrefab)
    {
        _buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (_text == "") ? buttonTypes[_type] : _text;
        if (GUI.changed) {EditorUtility.SetDirty(_buttonPrefab.transform.GetChild(0));}

        _buttonPrefab.GetComponent<ButtonScript>().SetType((CanvasManager.ButtonType)_type);

        _buttonPrefab.GetComponent<Image>().sprite = (_sprite == null) ? Resources.Load<Sprite>("DefaultSprite") : _sprite;
        _buttonPrefab.GetComponent<RectTransform>().localScale = new Vector3(_scale, _scale, 1);
    }

    GameObject AddButton(int _type, int _menuType, float _scale, string _text, Sprite _sprite)
    {
        GameObject buttonPrefab = Instantiate(Resources.Load<GameObject>("ButtonPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
        buttonPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(_menuType).GetChild(0));
        buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (_text == "") ? buttonTypes[_type] : _text;
        
        buttonPrefab.GetComponent<ButtonScript>().SetType((CanvasManager.ButtonType)_type);

        buttonPrefab.GetComponent<Image>().sprite = (_sprite == null) ? Resources.Load<Sprite>("DefaultSprite") : _sprite;
        buttonPrefab.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        buttonPrefab.GetComponent<RectTransform>().localScale = new Vector3(_scale, _scale, 1);

        buttonTextField.Add("new text");
        buttonScale.Add(_scale);
        buttonListDropDown.Add(false);

        return buttonPrefab;
    }

    void DestroyButton(int _i)
    {
        DestroyImmediate(buttonList[_i]);
        buttonList.RemoveAt(_i);
        buttonTextField.RemoveAt(_i);
        buttonScale.RemoveAt(_i);
    }


    // TEXT

    void AddText(int _type, int _menuType, string _text, float _size)
    {
        GameObject textPrefab = Instantiate(Resources.Load<GameObject>("TextPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
        textPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(_menuType).GetChild(0));
        if(_type ==  1) //currency
        {
            textPrefab.GetComponent<TextMeshProUGUI>().text += currencyName;
        }
        textPrefab.GetComponent<TextMeshProUGUI>().text += textTypeAdd[_type];
        textPrefab.GetComponent<TextMeshProUGUI>().text += _text;
        textPrefab.GetComponent<TextMeshProUGUI>().fontSize = _size;
    }


    // MENU

    void AddMenu(int _i)
    {
        GameObject menuObj = GameObject.FindGameObjectWithTag(menuTypes[_i] + "Menu");

        if (menuObj == null)
        {
            Debug.Log(menuTypes[_i] + " Menu Added");
            //instantiate
            GameObject menuPrefab = Instantiate(Resources.Load<GameObject>("MenuPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
            menuPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            menuPrefab.transform.tag = menuTypes[_i] + "Menu";
        }
        else
        {
            //destroy object
            Debug.Log(menuTypes[_i] + " Menu Removed");
            DestroyImmediate(menuObj);
        }
    }
}