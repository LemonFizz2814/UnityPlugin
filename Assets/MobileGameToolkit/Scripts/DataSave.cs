using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class DataSave : MonoBehaviour
{
    public class GameDataSave
    {
        public int highscore;
        public int currency;
        public Sprite selectedSprite;
        public List<bool> lootboxPrizes = new List<bool>();
    }
    public class ButtonSetupSave
    {
        public List<SetupEditor.ButtonItem> buttonItems = new List<SetupEditor.ButtonItem>();
    }
    public class TextSetupSave
    {
        public List<SetupEditor.TextItem> textItems = new List<SetupEditor.TextItem>();
    }
    public class GameplaySetupSave
    {
        public int lives;
        public int coinPoints;
        public int enemyDamage;
        public int enemySpawnFrequency;
        public int coinSpawnFrequency;
        public float playerJump;
        public string currencyName;
        public AudioClip menuBackgroundMusic;
        public AudioClip gameBackgroundMusic;
        public AudioClip coinCollectSound;
        public AudioClip playerJumpSound;
        public AudioClip playerHurtSound;
        public Sprite enemySprite;
        public Sprite coinSprite;
        public string backgroundSprite;
        public Sprite playerSprite;
    }
    public class LootboxSetupSave
    {
        public int lootBoxPrice;
        public AudioClip prizeSound; 
        public List<SetupEditor.LootBoxItem> lootBoxItems = new List<SetupEditor.LootBoxItem>();
    }
    public class MenuSave
    {
        public bool[] startEnabled;
    }

    //object classes
    public GameDataSave gameDataObject = new GameDataSave();
    public GameplaySetupSave gameplaySetupObject = new GameplaySetupSave();
    public ButtonSetupSave buttonSetupObject = new ButtonSetupSave();
    public LootboxSetupSave lootboxSetupObject = new LootboxSetupSave();
    public MenuSave menuSetupObject = new MenuSave();
    public TextSetupSave textSetupObject = new TextSetupSave();

    //json strings
    string jsonGameData;
    string jsonGamePlaySetup;
    string jsonButtonSetup;
    string jsonLootBoxSetup;
    string jsonMenuSetup;
    string jsonTextSetup;

    private void Awake()
    {
        LoadData();
    }

    //load data of json to objects
    public void LoadData()
    {
        Debug.Log("Load Data");

        //make json files if they do not exist
        if (!File.Exists(Application.persistentDataPath + "/gameData.json"))
        {
            //Debug.Log("Doesn't exist");
            jsonGameData = JsonUtility.ToJson(gameDataObject);
            File.WriteAllText(Application.persistentDataPath + "/gameData.json", jsonGameData, Encoding.UTF8);
        }
        if (!File.Exists(Application.persistentDataPath + "/gameplay.json"))
        {
            //Debug.Log("Doesn't exist");
            jsonGamePlaySetup = JsonUtility.ToJson(gameplaySetupObject);
            File.WriteAllText(Application.persistentDataPath + "/gameplay.json", jsonGamePlaySetup, Encoding.UTF8);
        }
        if (!File.Exists(Application.persistentDataPath + "/button.json"))
        {
            //Debug.Log("Doesn't exist");
            jsonButtonSetup = JsonUtility.ToJson(buttonSetupObject);
            File.WriteAllText(Application.persistentDataPath + "/button.json", jsonButtonSetup, Encoding.UTF8);
        }
        if (!File.Exists(Application.persistentDataPath + "/lootbox.json"))
        {
            //Debug.Log("Doesn't exist");
            jsonLootBoxSetup = JsonUtility.ToJson(lootboxSetupObject);
            File.WriteAllText(Application.persistentDataPath + "/lootbox.json", jsonLootBoxSetup, Encoding.UTF8);
        }
        if (!File.Exists(Application.persistentDataPath + "/menu.json"))
        {
            //Debug.Log("Doesn't exist");
            jsonMenuSetup = JsonUtility.ToJson(menuSetupObject);
            File.WriteAllText(Application.persistentDataPath + "/menu.json", jsonMenuSetup, Encoding.UTF8);
        }
        if (!File.Exists(Application.persistentDataPath + "/text.json"))
        {
            //Debug.Log("Doesn't exist");
            jsonTextSetup = JsonUtility.ToJson(textSetupObject);
            File.WriteAllText(Application.persistentDataPath + "/text.json", jsonTextSetup, Encoding.UTF8);
        }

        //read json and set objects to json
        gameDataObject = JsonUtility.FromJson<GameDataSave>(File.ReadAllText(Application.persistentDataPath + "/gameData.json"));
        gameplaySetupObject = JsonUtility.FromJson<GameplaySetupSave>(File.ReadAllText(Application.persistentDataPath + "/gameplay.json"));
        buttonSetupObject = JsonUtility.FromJson<ButtonSetupSave>(File.ReadAllText(Application.persistentDataPath + "/button.json"));
        lootboxSetupObject = JsonUtility.FromJson<LootboxSetupSave>(File.ReadAllText(Application.persistentDataPath + "/lootbox.json"));
        menuSetupObject = JsonUtility.FromJson<MenuSave>(File.ReadAllText(Application.persistentDataPath + "/menu.json"));
        textSetupObject = JsonUtility.FromJson<TextSetupSave>(File.ReadAllText(Application.persistentDataPath + "/text.json"));

        //set the json string files to the objects
        jsonGameData = JsonUtility.ToJson(gameDataObject);
        jsonGamePlaySetup = JsonUtility.ToJson(gameplaySetupObject);
        jsonButtonSetup = JsonUtility.ToJson(buttonSetupObject);
        jsonLootBoxSetup = JsonUtility.ToJson(lootboxSetupObject);
        jsonMenuSetup = JsonUtility.ToJson(menuSetupObject);
        jsonTextSetup = JsonUtility.ToJson(textSetupObject);

        //gameDataObject = JsonUtility.FromJson<GameDataSave>(jsonGameData);
        //gameplaySetupObject = JsonUtility.FromJson<GameplaySetupSave>(jsonGamePlaySetup);
    }

    //save data for game data
    public void SaveGameData(int _currency, int _highscore, List<bool> _lootboxPrizes)
    {
        gameDataObject.currency = _currency;
        gameDataObject.highscore = _highscore;
        gameDataObject.lootboxPrizes = _lootboxPrizes;

        File.WriteAllText(Application.persistentDataPath + "/gameData.json", JsonUtility.ToJson(gameDataObject, true));
    }

    //save game play setup data
    public void SaveGamePlaySetup(int _lives, int _coinPoints, int _enemyDamage, int _enemySpawnFrequency, int _coinSpawnFrequency, float _playerJump, string _currencyName,
        AudioClip _menuBackgroundMusic, AudioClip _gameBackgroundMusic, AudioClip _coinCollectSound, AudioClip _playerJumpSound, AudioClip _playerHurtSound, Sprite _enemySprite, Sprite _coinSprite, string _backgroundSprite, Sprite _playerSprite)
    {
        gameplaySetupObject.lives = _lives;
        gameplaySetupObject.coinPoints = _coinPoints;
        gameplaySetupObject.enemyDamage = _enemyDamage;
        gameplaySetupObject.enemySpawnFrequency = _enemySpawnFrequency;
        gameplaySetupObject.coinSpawnFrequency = _coinSpawnFrequency;
        gameplaySetupObject.playerJump = _playerJump;
        gameplaySetupObject.currencyName = _currencyName;
        gameplaySetupObject.menuBackgroundMusic = _menuBackgroundMusic;
        gameplaySetupObject.gameBackgroundMusic = _gameBackgroundMusic;
        gameplaySetupObject.coinCollectSound = _coinCollectSound;
        gameplaySetupObject.playerJumpSound = _playerJumpSound;
        gameplaySetupObject.playerHurtSound = _playerHurtSound;
        gameplaySetupObject.enemySprite = _enemySprite;
        gameplaySetupObject.coinSprite = _coinSprite;
        gameplaySetupObject.backgroundSprite = _backgroundSprite;
        gameplaySetupObject.playerSprite = _playerSprite;

        //jsonGamePlaySetup = JsonUtility.ToJson(gameplaySetupObject);
        //gameplaySetupObject = JsonUtility.FromJson<GameplaySetupSave>(jsonGamePlaySetup);

        File.WriteAllText(Application.persistentDataPath + "/gameplay.json", JsonUtility.ToJson(gameplaySetupObject, true));
    }

    //save menu setup data
    public void SaveMenuSetup(bool[] _list)
    {
        menuSetupObject.startEnabled = _list;

        File.WriteAllText(Application.persistentDataPath + "/menu.json", JsonUtility.ToJson(menuSetupObject, true));
    }

    //save button data
    public void SaveButtons(int _i, SetupEditor.ButtonItem _buttonItem)
    {
        if (_i > buttonSetupObject.buttonItems.Count)
        {
            buttonSetupObject.buttonItems.Add(_buttonItem);
        }
        buttonSetupObject.buttonItems[_i] = _buttonItem;

        File.WriteAllText(Application.persistentDataPath + "/button.json", JsonUtility.ToJson(buttonSetupObject, true));
    }

    //save lootbox data
    public void SaveLootboxs(int _i, SetupEditor.LootBoxItem _lootboxItem, int _lootBoxPrice, AudioClip _prizeSound)
    {
        lootboxSetupObject.lootBoxItems[_i] = _lootboxItem;
        lootboxSetupObject.lootBoxPrice = _lootBoxPrice;
        lootboxSetupObject.prizeSound = _prizeSound;

        File.WriteAllText(Application.persistentDataPath + "/lootbox.json", JsonUtility.ToJson(lootboxSetupObject, true));
    }

    //save text data
    public void SaveText(int _i, SetupEditor.TextItem _textItem)
    {
        textSetupObject.textItems[_i] = _textItem;

        File.WriteAllText(Application.persistentDataPath + "/text.json", JsonUtility.ToJson(textSetupObject, true));
    }
}
