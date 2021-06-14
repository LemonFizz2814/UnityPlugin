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

    AudioClip menuBackgroundMusic;
    AudioClip gameBackgroundMusic;
    AudioClip buttonClickSound;
    AudioClip prizeSound;
    AudioClip playerJumpSound;
    AudioClip coinCollectSound;
    AudioClip playerHurtSound;

    //loot box object
    public struct LootBoxItem
    {
        public int chances;
        public Sprite sprite;
        public bool beenPurchased;
        public AudioClip prizeSound;

        public LootBoxItem(int _chances, Sprite _sprite, bool _beenPurchased, AudioClip _prizeSound)
        {
            this.chances = _chances;
            this.sprite = _sprite;
            this.beenPurchased = _beenPurchased;
            this.prizeSound = _prizeSound;
        }
    }

    //buton object
    public struct ButtonItem
    {
        public GameObject obj;
        public bool dropDown;
        public string textField;
        public float scale;
        public int parent;
        public int type;
        public AudioClip buttonClickSound;

        public ButtonItem(GameObject _obj, bool _dropDown, string _textField, float _scale, int _parent, int _type, AudioClip _buttonClickSound)
        {
            this.obj = _obj;
            this.dropDown = _dropDown;
            this.textField = _textField;
            this.scale = _scale;
            this.parent = _parent;
            this.type = _type;
            this.buttonClickSound = _buttonClickSound;
        }
    }

    List<LootBoxItem> lootboxList = new List<LootBoxItem>();
    List<ButtonItem> buttonList2 = new List<ButtonItem>();

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

    //saving variables
    void SaveVariables()
    {
        PlayerPrefs.SetInt("lives", playerLives);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().UpdateVariables(currencyName, playerLives, playerSprite);
    }

    //display GUI
    private void OnGUI()
    {
        //do when editor or game is first opened
        if(startup)
        {
            dataSave = GameObject.FindGameObjectWithTag("DataSave").GetComponent<DataSave>();
            dataSave.LoadData();
            Debug.Log("dataSave.gameplaySetupObject.lives " + dataSave.gameplaySetupObject.lives);
            playerLives = 3;//dataSave.gameplaySetupObject.lives;
            Debug.Log("playerLives " + playerLives);

            startup = false;
        }

        //menu toolbar
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);
        switch (toolbarInt)
        {
            case 0: //menu setup
                GUILayout.Label("Menu setup", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                //display menus
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

                //button to add a new menu
                if (GUILayout.Button("Add New Menu Type"))
                {
                    Debug.Log("pressed");
                }
                break;

            case 1: //button setup
                GUILayout.Label("Buttons Menu", EditorStyles.boldLabel);

                EditorGUILayout.Space();

                //button to add a new button and add create a list of buttons
                if (GUILayout.Button("Add New Button"))
                {
                    buttonList.Add(AddButton(0, 0, 1, "New Text", buttonSprite));
                }
                //clear every button from the list
                if (GUILayout.Button("Clear All"))
                {
                    //destroy objects
                    for (int i = 0; i < buttonList.Count; i++)
                    {
                        DestroyImmediate(buttonList[i]);
                    }
                    buttonList.Clear();
                }

                EditorGUILayout.BeginVertical();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(600), GUILayout.Height(300));

                //display all the buttons in the list
                for (int i = 0; i < buttonList.Count; i++)
                {
                    buttonListDropDown[i] = EditorGUILayout.Foldout(buttonListDropDown[i], "Button " + i);
                    //if dropdown opened then display buttons edit contents
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

                menuBackgroundMusic = (AudioClip)EditorGUILayout.ObjectField("Menu Background Music", menuBackgroundMusic, typeof(AudioClip), true);
                gameBackgroundMusic = (AudioClip)EditorGUILayout.ObjectField("Game Background Music", gameBackgroundMusic, typeof(AudioClip), true);

                // EDIT PLAYER OPTIONS
                GUILayout.Label("Edit Player", EditorStyles.largeLabel);
                EditorGUI.indentLevel++;
                playerLives = EditorGUILayout.IntField("Lives", playerLives);
                playerJump = EditorGUILayout.FloatField("Jump Height", playerJump);
                playerSprite = (Sprite)EditorGUILayout.ObjectField("Player Sprite", playerSprite, typeof(Sprite), true);
                playerHurtSound = (AudioClip)EditorGUILayout.ObjectField("Player Hurt Sound", playerHurtSound, typeof(AudioClip), true);
                playerJumpSound = (AudioClip)EditorGUILayout.ObjectField("Player Jump Sound", playerJumpSound, typeof(AudioClip), true);
                EditorGUI.indentLevel--;
                //EditorGUILayout.Space();

                // EDIT ENEMY OPTIONS
                GUILayout.Label("Edit Enemy", EditorStyles.largeLabel);
                EditorGUI.indentLevel++;
                enemyDamage = EditorGUILayout.IntField("Damage", enemyDamage);
                enemySpawnFrequency = EditorGUILayout.IntField("Spawn Frequency", enemySpawnFrequency);
                enemySprite = (Sprite)EditorGUILayout.ObjectField("Enemy Sprite", enemySprite, typeof(Sprite), true);
                EditorGUI.indentLevel--;
                //EditorGUILayout.Space();

                // EDIT COIN OPTIONS
                GUILayout.Label("Edit Coin", EditorStyles.largeLabel);
                EditorGUI.indentLevel++;
                coinPoints = EditorGUILayout.IntField("Score Awarded", coinPoints);
                coinSpawnFrequency = EditorGUILayout.IntField("Spawn Frequency", coinSpawnFrequency);
                coinSprite = (Sprite)EditorGUILayout.ObjectField("Coin Sprite", coinSprite, typeof(Sprite), true);
                coinCollectSound = (AudioClip)EditorGUILayout.ObjectField("Coin Collect Sound", coinCollectSound, typeof(AudioClip), true);
                EditorGUI.indentLevel--;
                //EditorGUILayout.Space();

                // EDIT PLATFORM
                GUILayout.Label("Edit Platforms", EditorStyles.largeLabel);
                EditorGUI.indentLevel++;
                platformSprite = (Sprite)EditorGUILayout.ObjectField("Platform Sprite", platformSprite, typeof(Sprite), true);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                /*if (GUILayout.Button("Add Player"))
                {
                    Debug.Log("added player");
                    Instantiate(Resources.Load<GameObject>("PlayerPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
                }*/
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

                if(GUI.changed)
                {
                    SaveVariables();
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
    //edit options for button objects
    void ShowButtonEditOptions(int _i)
    {
        //Add new button
        buttonSelected[_i] = EditorGUILayout.Popup("Button Type", buttonSelected[_i], buttonTypes);
        buttonParent[_i] = EditorGUILayout.Popup("Menu", buttonParent[_i], menuTypes);
        GUILayout.Label(buttonInfo[buttonSelected[_i]], EditorStyles.label);
        buttonScale[_i] = EditorGUILayout.Slider("Scale", buttonScale[_i], 0, 4);

        buttonSprite = (Sprite)EditorGUILayout.ObjectField("Button Sprite", buttonSprite, typeof(Sprite), true);

        buttonClickSound = (AudioClip)EditorGUILayout.ObjectField("Click Sound", buttonClickSound, typeof(AudioClip), true);

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
            UpdateButton(_i, buttonSelected[_i], buttonScale[_i], buttonTextField[_i], buttonSprite, buttonList[_i], buttonParent[_i]);
        }

        EditorGUILayout.Space();
    }

    //edit options for text objects
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

    //edit options for lootbox objects
    void ShowLootBoxEditOptions()
    {
        lootBoxPrice = EditorGUILayout.IntField("Price for lootbox", lootBoxPrice);

        prizeSound = (AudioClip)EditorGUILayout.ObjectField("Prize Awarded Sound", prizeSound, typeof(AudioClip), true);

        //get a collective of the percentage
        int totalPercentage = 0;
        for (int i = 0; i < lootboxList.Count; i++)
        {
            totalPercentage += lootboxList[i].chances;
        }

        //button for adding new lootbox item to list
        if (GUILayout.Button("Add New Item To Lootbox"))
        {
            lootboxList.Add(new LootBoxItem(1, null, false, null));
            lootboxDropDown.Add(false);
        }
        //button for clear every lootbox item from list
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
                lootboxList[i] = new LootBoxItem(EditorGUILayout.IntField("Chance " + lootboxList[i].chances + "/" + totalPercentage, lootboxList[i].chances), (Sprite)EditorGUILayout.ObjectField("Player Sprite Award", lootboxList[i].sprite, typeof(Sprite), true), lootboxList[i].beenPurchased, lootboxList[i].prizeSound);

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
    //update button variables
    void UpdateButton(int _i, int _type, float _scale, string _text, Sprite _sprite, GameObject _buttonPrefab, int _parent)
    {
        _buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (_text == "") ? buttonTypes[_type] : _text;
        //force update text in inspector
        if (GUI.changed) {EditorUtility.SetDirty(_buttonPrefab.transform.GetChild(0));}

        _buttonPrefab.GetComponent<ButtonScript>().SetType((CanvasManager.ButtonType)_type);

        _buttonPrefab.GetComponent<Image>().sprite = (_sprite == null) ? Resources.Load<Sprite>("DefaultSprite") : _sprite;
        _buttonPrefab.GetComponent<RectTransform>().localScale = new Vector3(_scale, _scale, 1);

        if(GameObject.FindGameObjectWithTag(menuTypes[_parent] + "Menu") != null)
        {
            _buttonPrefab.transform.SetParent(GameObject.FindGameObjectWithTag(menuTypes[_parent] + "Menu").transform.GetChild(0));//GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(_parent).GetChild(0));
        }
        else
        {
            _buttonPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetChild(0));
        }

        buttonList2[_i] = new ButtonItem(_buttonPrefab, buttonList2[_i].dropDown, _text, _scale, _parent, _type, buttonList2[_i].buttonClickSound);
        dataSave.SaveButtons(_i, buttonList2[_i]);
    }

    //add a new button object
    GameObject AddButton(int _type, int _menuType, float _scale, string _text, Sprite _sprite)
    {
        GameObject buttonPrefab = Instantiate(Resources.Load<GameObject>("ButtonPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
        buttonPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(_menuType).GetChild(0));
        buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (_text == "") ? buttonTypes[_type] : _text;
        
        buttonPrefab.GetComponent<ButtonScript>().SetType((CanvasManager.ButtonType)_type);

        buttonPrefab.GetComponent<Image>().sprite = (_sprite == null) ? Resources.Load<Sprite>("DefaultSprite") : _sprite;
        buttonPrefab.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        buttonPrefab.GetComponent<RectTransform>().localScale = new Vector3(_scale, _scale, 1);

        //add to lists of button parameters
        buttonTextField.Add("new text");
        buttonScale.Add(_scale);
        buttonListDropDown.Add(false);
        buttonSelected.Add(_type);
        buttonParent.Add(_type);

        buttonList2.Add(new ButtonItem(buttonPrefab, false, "new text", _scale, _type, _type, null));

        return buttonPrefab;
    }

    //destroy a button object
    void DestroyButton(int _i)
    {
        DestroyImmediate(buttonList[_i]);

        buttonList.RemoveAt(_i);
        buttonTextField.RemoveAt(_i);
        buttonScale.RemoveAt(_i);
        buttonSelected.RemoveAt(_i);
        buttonParent.RemoveAt(_i);

        DestroyImmediate(buttonList2[_i].obj);
        buttonList2.RemoveAt(_i);
    }


    // TEXT
    //add a new text object
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
    //add a new menu object
    void AddMenu(int _i)
    {
        GameObject menuObj = GameObject.FindGameObjectWithTag(menuTypes[_i] + "Menu");

        if (menuObj == null)
        {
            Debug.Log(menuTypes[_i] + " Menu Added");
            //instantiate a prefab
            GameObject menuPrefab = Instantiate(Resources.Load<GameObject>("MenuPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
            menuPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            menuPrefab.transform.tag = menuTypes[_i] + "Menu";
            menuPrefab.name = menuTypes[_i] + "MenuObject";
        }
        else
        {
            //destroy menu object
            Debug.Log(menuTypes[_i] + " Menu Removed");
            DestroyImmediate(menuObj);
        }
    }
}