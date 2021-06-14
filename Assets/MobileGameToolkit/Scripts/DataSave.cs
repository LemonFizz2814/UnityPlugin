using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class DataSave : MonoBehaviour
{
    [Serializable]
    public class GameDataSave
    {
        public int highscore;
        public int currency;
        public Sprite selectedSprite;
        public List<bool> lootboxPrizes = new List<bool>();
    }
    [Serializable]
    public class MenuSetupSave
    {
        public List<GameObject> menuList = new List<GameObject>();
    }
    [Serializable]
    public class ButtonSetupSave
    {
        public List<SetupEditor.ButtonItem> buttonItems = new List<SetupEditor.ButtonItem>();
    }
    [Serializable]
    public class TextSetupSave
    {
        public List<SetupEditor.TextItem> textItems = new List<SetupEditor.TextItem>();
    }
    [Serializable]
    public class GameplaySetupSave
    {
        public int lives;
        public int playerJump;
        public int coinPoints;
        public int enemyDamage;
        public int enemySpawnFrequency;
        public int coinSpawnFrequency;
        public string currencyName;
        public AudioClip menuBackgroundMusic;
        public AudioClip gameBackgroundMusic;
        public AudioClip coinCollectSound;
        public AudioClip playerJumpSound;
        public AudioClip playerHurtSound;
    }
    [Serializable]
    public class LootboxSetupSave
    {
        public List<SetupEditor.LootBoxItem> lootBoxItems = new List<SetupEditor.LootBoxItem>();
    }

    //object classes
    public GameDataSave gameDataObject = new GameDataSave();
    public GameplaySetupSave gameplaySetupObject = new GameplaySetupSave();
    public MenuSetupSave menuSetupObject = new MenuSetupSave();
    public ButtonSetupSave buttonSetupObject = new ButtonSetupSave();
    public LootboxSetupSave lootboxSetupObject = new LootboxSetupSave();

    //json strings
    string jsonGameData;
    string jsonGamePlaySetup;

    private void Start()
    {
        LoadData();
    }

    //load data of json to objects
    public void LoadData()
    {
        Debug.Log("Load Data");

        /*if (JsonUtility.ToJson(gameDataObject) == null)
        {
            Debug.Log("gameDataObject Is null");
            SaveGameData(0, 0, null);
        }
        if (JsonUtility.ToJson(gameplaySetupObject) == null)
        {
            Debug.Log("jsonGamePlaySetup Is null");
            SaveGamePlaySetup(3);
        }*/
        if (!File.Exists(Application.persistentDataPath + "/saveload.json"))
        {
            Debug.Log("Doesn't exist");
            jsonGameData = JsonUtility.ToJson(gameDataObject);
            File.WriteAllText(Application.persistentDataPath + "/saveload.json", jsonGameData, Encoding.UTF8);
        }
        gameplaySetupObject = JsonUtility.FromJson<GameplaySetupSave>(File.ReadAllText(Application.persistentDataPath + "/saveload.json"));

        jsonGameData = JsonUtility.ToJson(gameDataObject);
        //jsonGamePlaySetup = JsonUtility.ToJson(gameplaySetupObject);

        gameDataObject = JsonUtility.FromJson<GameDataSave>(jsonGameData);
        //gameplaySetupObject = JsonUtility.FromJson<GameplaySetupSave>(jsonGamePlaySetup);
    }

    //save data for game data
    public void SaveGameData(int _currency, int _highscore, List<bool> _lootboxPrizes)
    {
        gameDataObject.currency = _currency;
        gameDataObject.highscore = _highscore;
        gameDataObject.lootboxPrizes = _lootboxPrizes;

        jsonGameData = JsonUtility.ToJson(gameDataObject);
    }

    //save game play setup data
    public void SaveGamePlaySetup(int _lives, int _playerJump, int _coinPoints, int _enemyDamage, string _currencyName,
        AudioClip _menuBackgroundMusic, AudioClip _gameBackgroundMusic, AudioClip _coinCollectSound, AudioClip _playerJumpSound, AudioClip _playerHurtSound)
    {
        gameplaySetupObject.lives = _lives;
        gameplaySetupObject.playerJump = _playerJump;
        gameplaySetupObject.coinPoints = _coinPoints;
        gameplaySetupObject.enemyDamage = _enemyDamage;
        gameplaySetupObject.currencyName = _currencyName;
        gameplaySetupObject.menuBackgroundMusic = _menuBackgroundMusic;
        gameplaySetupObject.gameBackgroundMusic = _gameBackgroundMusic;
        gameplaySetupObject.coinCollectSound = _coinCollectSound;
        gameplaySetupObject.playerJumpSound = _playerJumpSound;
        gameplaySetupObject.playerHurtSound = _playerHurtSound;

    //jsonGamePlaySetup = JsonUtility.ToJson(gameplaySetupObject);
    //gameplaySetupObject = JsonUtility.FromJson<GameplaySetupSave>(jsonGamePlaySetup);
    Debug.Log("gameplaySetupObject.lives " + gameplaySetupObject.lives);

        string jsonData = JsonUtility.ToJson(gameplaySetupObject, true);
        File.WriteAllText(Application.persistentDataPath + "/saveload.json", jsonData);
    }

    //save menu setup data
    public void SaveMenuSetup()
    {

    }

    //save button data
    public void SaveButtons(int _i, SetupEditor.ButtonItem _buttonItem)
    {
        if (_i > buttonSetupObject.buttonItems.Count)
        {
            buttonSetupObject.buttonItems.Add(_buttonItem);
        }
        buttonSetupObject.buttonItems[_i] = _buttonItem;
    }

    //save lootbox data
    public void SaveLootboxs(int _i, SetupEditor.LootBoxItem _lootboxItem)
    {
        lootboxSetupObject.lootBoxItems[_i] = _lootboxItem;
    }

    //save text
    public void SaveText(int _i, SetupEditor.TextItem _textItem)
    {
        //lootboxSetupObject.lootBoxItems[_i] = _textItem;
    }
}
