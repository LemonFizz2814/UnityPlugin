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

    int textSelected = 0;
    int gameSelected = 0;
    int toolbarInt = 0;
    int enemyDamage = 1;
    int coinPoints = 10;
    int playerLives = 3;
    int enemySpawnFrequency = 0;
    int coinSpawnFrequency = 0;
    int lootBoxPrice = 100;
    Vector2 scrollPos;

    TMP_FontAsset textFont;

    Sprite buttonSprite;
    Sprite playerSprite;
    Sprite enemySprite;
    Sprite coinSprite;
    Sprite platformSprite;

    DataSave dataSave;

    string[] buttonTypes = new string[8] { "Play", "Exit", "Shop", "Back", "SocialMedia", "Settings", "Advert", "Lootbox" };
    string[] buttonInfo = new string[8] { "Starts game when presed", "Exits application when pressed", "Opens shop menu when pressed", "Goes back to start menu when pressed", "Opens a link to social media when pressed", "Opens the setting menu when pressed", "Plays an advertisement when pressed", "Opens a lootbox when pressed" };
    string[] menuTypes = new string[5] { "Start", "GameOver", "Settings", "Shop", "GamePlay" };
    string[] menuTitle = new string[5] { "Add", "Add", "Add", "Add", "Add" };
    string[] textTypes = new string[4] { "Info", "Currency", "Score", "Lives" };
    string[] textTypeAdd = new string[5] { "", ": ", "Score: ", "Lives: ", "" };
    string[] gameType = new string[2] { "Endless Runner", "Own Game~" };

    string[] toolbarStrings = new string[4] { "Menus setup", "Buttons setup", "Gameplay menu", "Lootbox setup" };

    bool[] startEnabled = new bool[5] { false, false, false, false, false };

    bool startup = true;

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
    List<bool> lootboxDropDown = new List<bool>();
    List<bool> buttonListDropDown = new List<bool>();
    List<string> buttonTextField = new List<string>();
    List<float> buttonScale = new List<float>();
    List<int> buttonSelected = new List<int>();
    List<int> buttonParent = new List<int>();

    [MenuItem("Window/Mobile Setup")]
    public static void ShowWindow()
    {
        Debug.Log("check");
        GetWindow<SetupEditor>("Setup Editor");
    }

    private void OnGUI()
    {
        if(startup)
        {
            dataSave = GameObject.FindGameObjectWithTag("DataSave").GetComponent<DataSave>();
            dataSave.LoadData();
            Debug.Log("dataSave.gameplaySetupObject.lives " + dataSave.gameplaySetupObject.lives);
            playerLives = 3;//dataSave.gameplaySetupObject.lives;
            Debug.Log("playerLives " + playerLives);

            startup = false;
        }

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

                EditorGUILayout.Space();

                if (GUILayout.Button("Add New Button"))
                {
                    buttonList.Add(AddButton(0, 0, 1, "New Text", buttonSprite));
                }
                if (GUILayout.Button("Clear All"))
                {
                    for (int i = 0; i < buttonList.Count; i++)
                    {
                        DestroyImmediate(buttonList[i]);
                    }
                    buttonList.Clear();
                }

                EditorGUILayout.BeginVertical();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(600), GUILayout.Height(300));

                for (int i = 0; i < buttonList.Count; i++)
                {
                    buttonListDropDown[i] = EditorGUILayout.Foldout(buttonListDropDown[i], "Button " + i);
                    if (buttonListDropDown[i])
                    {
                        EditorGUI.indentLevel++;
                        ShowButtonEditOptions(i);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();

                break;

            case 2: //gameplay setup
                GUILayout.Label("Gameplay Menu", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                gameSelected = EditorGUILayout.Popup("Game Type", gameSelected, gameType);

                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(700), GUILayout.Height(500));

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
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

                if(GUI.changed)
                {
                    dataSave.SaveGamePlaySetup(playerLives);
                }
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

        buttonSelected[_i] = EditorGUILayout.Popup("Button Type", buttonSelected[_i], buttonTypes);
        buttonParent[_i] = EditorGUILayout.Popup("Menu", buttonParent[_i], menuTypes);
        GUILayout.Label(buttonInfo[buttonSelected[_i]], EditorStyles.label);
        buttonScale[_i] = EditorGUILayout.Slider("Scale", buttonScale[_i], 0, 4);

        buttonSprite = (Sprite)EditorGUILayout.ObjectField("Button Sprite", buttonSprite, typeof(Sprite), true);

        buttonTextField[_i] = EditorGUILayout.TextField("Button Text", buttonTextField[_i]);

        //additional changes for buttons
        if (EditorGUILayout.BeginFadeGroup(Convert.ToInt32(buttonSelected[_i] == 4))) //social media
        {
            GUILayout.Label("Social Media Options", EditorStyles.label);
            socialMediaURL = EditorGUILayout.TextField("Social Media URL", "");
        }
        EditorGUILayout.EndFadeGroup();
        //EditorGUILayout.EndFadeGroup();

        if (GUILayout.Button("Delete Button " + _i))
        {
            DestroyButton(_i);
        }
        if (GUI.changed)//GUILayout.Button("UpdateButton"))
        {
            UpdateButton(buttonSelected[_i], buttonScale[_i], buttonTextField[_i], buttonSprite, buttonList[_i], buttonParent[_i]);
        }

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

        int totalPercentage = 0;

        for (int i = 0; i < lootboxList.Count; i++)
        {
            totalPercentage += lootboxList[i].chances;
        }

        if (GUILayout.Button("Add New Item To Lootbox"))
        {
            lootboxList.Add(new LootBoxItem(1, null, false));
            lootboxDropDown.Add(false);
        }
        if (GUILayout.Button("Clear All Lootbox"))
        {
            lootboxList.Clear();
            lootboxDropDown.Clear();
        }

        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(600), GUILayout.Height(300));

        for (int i = 0; i < lootboxList.Count; i++)
        {
            lootboxDropDown[i] = EditorGUILayout.Foldout(lootboxDropDown[i], "Item " + i);
            if (lootboxDropDown[i])
            {
                EditorGUI.indentLevel++;
                //int ss = EditorGUILayout.IntField("Chance", lootboxList[i].chances);
                lootboxList[i] = new LootBoxItem(EditorGUILayout.IntField("Chance " + lootboxList[i].chances + "/" + totalPercentage, lootboxList[i].chances), (Sprite)EditorGUILayout.ObjectField("Player Sprite Award", lootboxList[i].sprite, typeof(Sprite), true), lootboxList[i].beenPurchased);

                if (GUILayout.Button("Delete Item " + i))
                {
                    lootboxDropDown.RemoveAt(i);
                    lootboxList.RemoveAt(i);
                }
                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
    }


    // BUTTONS

    void UpdateButton(int _type, float _scale, string _text, Sprite _sprite, GameObject _buttonPrefab, int _parent)
    {
        _buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (_text == "") ? buttonTypes[_type] : _text;
        if (GUI.changed) {EditorUtility.SetDirty(_buttonPrefab.transform.GetChild(0));}

        _buttonPrefab.GetComponent<ButtonScript>().SetType((CanvasManager.ButtonType)_type);

        _buttonPrefab.GetComponent<Image>().sprite = (_sprite == null) ? Resources.Load<Sprite>("DefaultSprite") : _sprite;
        _buttonPrefab.GetComponent<RectTransform>().localScale = new Vector3(_scale, _scale, 1);

        _buttonPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(_parent).GetChild(0));
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
        buttonSelected.Add(_type);
        buttonParent.Add(_type);

        return buttonPrefab;
    }

    void DestroyButton(int _i)
    {
        DestroyImmediate(buttonList[_i]);
        buttonList.RemoveAt(_i);
        buttonTextField.RemoveAt(_i);
        buttonScale.RemoveAt(_i);
        buttonSelected.RemoveAt(_i);
        buttonParent.RemoveAt(_i);
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