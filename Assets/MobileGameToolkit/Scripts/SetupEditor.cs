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
    int enemyDamage = 1;
    int coinPoints = 10;
    int playerLives = 2;
    int enemySpawnFrequency = 0;
    int coinSpawnFrequency = 0;
    int lootBoxPrice = 100;
    Vector2 scrollPos;

    TMP_FontAsset defaultFont;

    Sprite defaultButtonSprite;
    Sprite playerSprite;
    Sprite enemySprite;
    Sprite coinSprite;
    Sprite platformSprite;
    Sprite backgroundSprite;

    DataSave dataSave;

    string[] buttonTypes = new string[8] { "Play", "Exit", "Shop", "Back", "SocialMedia", "Settings", "Advert", "Lootbox" };
    string[] buttonInfo = new string[8] { "Starts game when presed", "Exits application when pressed", "Opens shop menu when pressed", "Goes back to start menu when pressed", "Opens a link to social media when pressed", "Opens the setting menu when pressed", "Plays an advertisement when pressed", "Opens a lootbox when pressed" };
    string[] menuTypes = new string[5] { "Start", "GameOver", "Settings", "Shop", "GamePlay" };
    string[] menuTitle = new string[5] { "Add", "Add", "Add", "Add", "Add" };
    string[] textTypes = new string[4] { "Info", "Currency", "Score", "Lives" };
    string[] textTypeAdd = new string[5] { "", ": ", "Score: ", "Lives: ", "" };
    string[] gameType = new string[2] { "Endless Runner", "Own Game~" };

    string[] toolbarStrings = new string[5] { "Gameplay menu", "Menus setup", "Buttons setup", "Text setup", "Lootbox setup" };

    bool[] startEnabled = new bool[5] { false, false, false, false, false };

    bool startup = true;

    AudioClip menuBackgroundMusic;
    AudioClip gameBackgroundMusic;
    AudioClip prizeSound;
    AudioClip playerJumpSound;
    AudioClip coinCollectSound;
    AudioClip playerHurtSound;

    enum ToolBar
    {
        Gameplay,
        Menu,
        Button,
        Text,
        Lootbox
    }

    ToolBar toolbar;

    //loot box object
    public class LootBoxItem
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

    //text object
    public class TextItem
    {
        public int type;
        public TMP_FontAsset textFont;
        public float scale;
        public GameObject obj;
        public bool dropDown;
        public string textField;
        public int parent;
        //public string currenyName;

        public TextItem(int _type, TMP_FontAsset _textFont, float _scale, GameObject _obj, bool _dropDown, string _textField, int _parent)
        {
            this.type = _type;
            this.textFont = _textFont;
            this.scale = _scale;
            this.obj = _obj;
            this.dropDown = _dropDown;
            this.textField = _textField;
            this.parent = _parent;
            //this.currenyName = _currenyName;
        }
    }

    //buton object
    public class ButtonItem
    {
        public GameObject obj;
        public bool dropDown;
        public string textField;
        public float scale;
        public int parent;
        public int type;
        public AudioClip buttonClickSound;
        public Sprite sprite;

        public ButtonItem(GameObject _obj, bool _dropDown, string _textField, float _scale, int _parent, int _type, AudioClip _buttonClickSound, Sprite _sprite)
        {
            this.obj = _obj;
            this.dropDown = _dropDown;
            this.textField = _textField;
            this.scale = _scale;
            this.parent = _parent;
            this.type = _type;
            this.buttonClickSound = _buttonClickSound;
            this.sprite = _sprite;
        }
    }

    List<LootBoxItem> lootboxList = new List<LootBoxItem>();
    List<ButtonItem> buttonList = new List<ButtonItem>();
    List<TextItem> textList = new List<TextItem>();

    List<bool> lootboxDropDown = new List<bool>();
    /*List<GameObject> buttonList = new List<GameObject>();
    List<string> buttonTextField = new List<string>();
    List<float> buttonScale = new List<float>();
    List<int> buttonSelected = new List<int>();
    List<int> buttonParent = new List<int>();*/

    [MenuItem("Window/Mobile Setup")]
    public static void ShowWindow()
    {
        Debug.Log("check");
        GetWindow<SetupEditor>("Setup Editor");
    }

    //saving variables
    void SaveVariables()
    {
        //PlayerPrefs.SetInt("lives", playerLives);
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().UpdateVariables(currencyName, playerLives, playerSprite);
    }

    //load variables from gameplaysetupobject
    void GamePlaySetupObjectLoad()
    {
        playerLives = dataSave.gameplaySetupObject.lives;
        coinCollectSound = dataSave.gameplaySetupObject.coinCollectSound;
        coinPoints = dataSave.gameplaySetupObject.coinPoints;
        coinSpawnFrequency = dataSave.gameplaySetupObject.coinSpawnFrequency;
        currencyName = dataSave.gameplaySetupObject.currencyName;
        enemyDamage = dataSave.gameplaySetupObject.enemyDamage;
        enemySpawnFrequency = dataSave.gameplaySetupObject.enemySpawnFrequency;
        gameBackgroundMusic = dataSave.gameplaySetupObject.gameBackgroundMusic;
        menuBackgroundMusic = dataSave.gameplaySetupObject.menuBackgroundMusic;
        playerHurtSound = dataSave.gameplaySetupObject.playerHurtSound;
        playerJump = dataSave.gameplaySetupObject.playerJump;
        playerJumpSound = dataSave.gameplaySetupObject.playerJumpSound;
    }

    //load variables from menuobject
    void MenuSetupLoad()
    {
        startEnabled = dataSave.menuSetupObject.startEnabled;
    }

    //load variables from buttonobject
    void ButtonSetupLoad()
    {
        buttonList = dataSave.buttonSetupObject.buttonItems;
    }

    //load variables from lootboxobject
    void LootBoxLoad()
    {
        lootboxList = dataSave.lootboxSetupObject.lootBoxItems;
        lootBoxPrice = dataSave.lootboxSetupObject.lootBoxPrice;
        prizeSound = dataSave.lootboxSetupObject.prizeSound;
    }

    //load variables from textobject
    void TextLoad()
    {
        textList = dataSave.textSetupObject.textItems;
    }

    //display GUI
    private void OnGUI()
    {
        //do when editor or game is first opened
        if(startup)
        {
            defaultButtonSprite = Resources.Load<Sprite>("DefaultSprite");
            defaultFont = Resources.Load<TMP_FontAsset>("LEMONMILK-Regular SDF");

            dataSave = GameObject.FindGameObjectWithTag("DataSave").GetComponent<DataSave>();
            dataSave.LoadData();
            Debug.Log("dataSave.gameplaySetupObject.lives " + dataSave.gameplaySetupObject.lives);

            //get and load variables
            GamePlaySetupObjectLoad();
            MenuSetupLoad();
            ButtonSetupLoad();
            LootBoxLoad();
            TextLoad();

            startup = false;
        }

        //menu toolbar
        toolbar = (ToolBar)GUILayout.Toolbar((int)toolbar, toolbarStrings);
        switch (toolbar)
        {
            case ToolBar.Gameplay: //gameplay setup
                GUILayout.Label("Gameplay Menu", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - 100));

                gameSelected = EditorGUILayout.Popup("Game Type", gameSelected, gameType);

                menuBackgroundMusic = (AudioClip)EditorGUILayout.ObjectField("Menu Background Music", menuBackgroundMusic, typeof(AudioClip), true);
                gameBackgroundMusic = (AudioClip)EditorGUILayout.ObjectField("Game Background Music", gameBackgroundMusic, typeof(AudioClip), true);

                backgroundSprite = (Sprite)EditorGUILayout.ObjectField("Background Sprite", backgroundSprite, typeof(Sprite), true);

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

                //when any variables have been changed
                if (GUI.changed)
                {
                    SaveVariables();
                    dataSave.SaveGamePlaySetup(playerLives, coinPoints, enemyDamage, enemySpawnFrequency, coinSpawnFrequency, playerJump, currencyName,
                        menuBackgroundMusic, gameBackgroundMusic, coinCollectSound, playerJumpSound, playerHurtSound, enemySprite, coinSprite, backgroundSprite);
                }
                break;
            case ToolBar.Menu: //menu setup
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

                //when any variables have been changed
                if (GUI.changed)
                {
                    for (int i = 0; i < buttonList.Count; i++)
                    {
                        dataSave.SaveMenuSetup(startEnabled);
                    }
                }
                break;

            case ToolBar.Button: //button setup
                GUILayout.Label("Buttons Menu", EditorStyles.boldLabel);

                EditorGUILayout.Space();

                //button to add a new button and add create a list of buttons
                if (GUILayout.Button("Add New Button"))
                {
                    //buttonList.Add(AddButton(0, 0, 1, "New Text", buttonSprite));
                    AddButton(0, 0, 1, "New Text", defaultButtonSprite);
                }
                //clear every button from the list
                if (GUILayout.Button("Clear All"))
                {
                    //destroy objects
                    for (int i = 0; i < buttonList.Count; i++)
                    {
                        DestroyImmediate(buttonList[i].obj);
                    }
                    buttonList.Clear();
                }

                EditorGUILayout.BeginVertical();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - 100));

                //display all the buttons in the list
                for (int i = 0; i < buttonList.Count; i++)
                {
                    buttonList[i].dropDown = EditorGUILayout.Foldout(buttonList[i].dropDown, "Button " + i);
                    //if dropdown opened then display buttons edit contents
                    if (buttonList[i].dropDown)
                    {
                        EditorGUI.indentLevel++;
                        ShowButtonEditOptions(i);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();

                //when any variables have been changed
                if (GUI.changed)
                {
                    for (int i = 0; i < buttonList.Count; i++)
                    {
                        dataSave.SaveButtons(i, buttonList[i]);
                    }
                }
                break;
            case ToolBar.Text: //text setup
                GUILayout.Label("Text Menu", EditorStyles.boldLabel);

                EditorGUILayout.Space();

                //button to add a new button and add create a list of buttons
                if (GUILayout.Button("Add New Text"))
                {
                    AddText(0, 0, "New Text", 1, defaultFont, 0);
                }
                //clear every button from the list
                if (GUILayout.Button("Clear All"))
                {
                    //destroy objects
                    for (int i = 0; i < textList.Count; i++)
                    {
                        DestroyImmediate(textList[i].obj);
                    }
                    textList.Clear();
                }

                EditorGUILayout.BeginVertical();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - 100));

                //display all the buttons in the list
                for (int i = 0; i < textList.Count; i++)
                {
                    textList[i].dropDown = EditorGUILayout.Foldout(textList[i].dropDown, "Text " + i);
                    //if dropdown opened then display buttons edit contents
                    if (textList[i].dropDown)
                    {
                        EditorGUI.indentLevel++;
                        ShowTextEditOptions(i);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();

                //when any variables have been changed
                if (GUI.changed)
                {
                    for (int i = 0; i < textList.Count; i++)
                    {
                        dataSave.SaveText(i, textList[i]);
                    }
                }
                break;
            case ToolBar.Lootbox: //lootbox setup
                GUILayout.Label("Lootbox Setup", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                ShowLootBoxEditOptions();

                //when any variables have been changed
                if (GUI.changed)
                {
                    for (int i = 0; i < lootboxList.Count; i++)
                    {
                        dataSave.SaveLootboxs(i, lootboxList[i], lootBoxPrice, prizeSound);
                    }
                }
                break;
        }
    }


    // OPTIONS
    //edit options for button objects
    void ShowButtonEditOptions(int _i)
    {
        //Add new button
        buttonList[_i].type = EditorGUILayout.Popup("Button Type", buttonList[_i].type, buttonTypes);
        buttonList[_i].parent = EditorGUILayout.Popup("Menu Parent", buttonList[_i].parent, menuTypes);
        GUILayout.Label(buttonInfo[buttonList[_i].type], EditorStyles.label);
        buttonList[_i].scale = EditorGUILayout.Slider("Scale", buttonList[_i].scale, 0, 4);

        buttonList[_i].sprite = (Sprite)EditorGUILayout.ObjectField("Button Sprite", buttonList[_i].sprite, typeof(Sprite), true);

        buttonList[_i].buttonClickSound = (AudioClip)EditorGUILayout.ObjectField("Click Sound", buttonList[_i].buttonClickSound, typeof(AudioClip), true);

        buttonList[_i].textField = EditorGUILayout.TextField("Button Text", buttonList[_i].textField);

        //additional changes for buttons
        if (EditorGUILayout.BeginFadeGroup(Convert.ToInt32(buttonList[_i].type == 4))) //social media
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
            UpdateButton(_i, buttonList[_i].type, buttonList[_i].scale, buttonList[_i].textField, buttonList[_i].sprite, buttonList[_i].obj, buttonList[_i].parent);
        }

        EditorGUILayout.Space();
    }

    //edit options for text objects
    void ShowTextEditOptions(int _i)
    {
        textList[_i].type = EditorGUILayout.Popup("Text Type", textList[_i].type, textTypes);
        textList[_i].textField = EditorGUILayout.TextField("Text", textList[_i].textField);
        textList[_i].scale = EditorGUILayout.Slider("Font size", textList[_i].scale, 0, 400);
        textList[_i].textFont = (TMP_FontAsset)EditorGUILayout.ObjectField("Button Sprite", textList[_i].textFont, typeof(TMP_FontAsset), true);

        if (EditorGUILayout.BeginFadeGroup(Convert.ToInt32(textList[_i].type == 1))) //currency
        {
            currencyName = EditorGUILayout.TextField("Currency Name", currencyName);
        }

        if (GUILayout.Button("Delete Text " + _i))
        {
            DestroyText(_i);
        }
        if (GUI.changed)//when GUI has been updated
        {
            UpdateText(_i, textList[_i].obj, textList[_i].type, textList[_i].textField, textList[_i].scale, textList[_i].textFont, textList[_i].type);
        }
        EditorGUILayout.EndFadeGroup();
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
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - 100));

        //display lootbox list of items
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

        //check if the menu it is assigned exists
        if(GameObject.FindGameObjectWithTag(menuTypes[_parent] + "Menu") != null)
        {
            _buttonPrefab.transform.SetParent(GameObject.FindGameObjectWithTag(menuTypes[_parent] + "Menu").transform.GetChild(0));//GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(_parent).GetChild(0));
        }
        else
        {
            //set it to the first menu that exists if the one assigned doesn't
            _buttonPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetChild(0));
            string value = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).name;
            value = value.Substring(0, value.Length - 4);

            //check the menus tag name with the existing menu tag types
            for(int i = 0; i < menuTypes.Length; i++)
            {
                if(value == menuTypes[i])
                {
                    buttonList[_i].parent = i;
                }
            }
        }

        buttonList[_i] = new ButtonItem(_buttonPrefab, buttonList[_i].dropDown, _text, _scale, _parent, _type, buttonList[_i].buttonClickSound, _sprite);
        dataSave.SaveButtons(_i, buttonList[_i]);
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
        /*buttonTextField.Add("new text");
        buttonScale.Add(_scale);
        buttonListDropDown.Add(false);
        buttonSelected.Add(_type);
        buttonParent.Add(_type);*/

        buttonList.Add(new ButtonItem(buttonPrefab, false, "new text", _scale, _type, _type, null, defaultButtonSprite));

        return buttonPrefab;
    }

    //destroy a button object
    void DestroyButton(int _i)
    {
        DestroyImmediate(buttonList[_i].obj);
        buttonList.RemoveAt(_i);
    }

    // TEXT
    //add a new text object
    void AddText(int _type, int _menuType, string _text, float _scale, TMP_FontAsset _font, int _parent)
    {
        GameObject textPrefab = Instantiate(Resources.Load<GameObject>("TextPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
        textPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(_menuType).GetChild(0));
        if(_type ==  1) //currency
        {
            textPrefab.GetComponent<TextMeshProUGUI>().text += currencyName;
        }
        textPrefab.GetComponent<TextMeshProUGUI>().text += textTypeAdd[_type];
        textPrefab.GetComponent<TextMeshProUGUI>().text += _text;
        textPrefab.GetComponent<TextMeshProUGUI>().fontSize = _scale;

        textList.Add(new TextItem(_type, _font, _scale, textPrefab, false, "new text", _type));
    }
    void UpdateText(int _i, GameObject _textPrefab, int _type, string _text, float _scale, TMP_FontAsset _font, int _parent)
    {
        _textPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (_text == "") ? buttonTypes[_type] : _text;
        _textPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().font = _font;
        //force update text in inspector
        if (GUI.changed) { EditorUtility.SetDirty(_textPrefab.transform.GetChild(0)); }

        _textPrefab.GetComponent<ButtonScript>().SetType((CanvasManager.ButtonType)_type);
        
        _textPrefab.GetComponent<RectTransform>().localScale = new Vector3(_scale, _scale, 1);

        if (GameObject.FindGameObjectWithTag(menuTypes[_parent] + "Menu") != null)
        {
            _textPrefab.transform.SetParent(GameObject.FindGameObjectWithTag(menuTypes[_parent] + "Menu").transform.GetChild(0));//GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(_parent).GetChild(0));
        }
        else
        {
            _textPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetChild(0));
        }

        textList[_i] = new TextItem(_type, _font, _scale, _textPrefab, textList[_i].dropDown, _text, _parent);
        dataSave.SaveText(_i, textList[_i]);
    }

    //destroy a text object
    void DestroyText(int _i)
    {
        DestroyImmediate(textList[_i].obj);
        textList.RemoveAt(_i);
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