using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public List<GameObject> buttonList = new List<GameObject>();
    }
    [Serializable]
    public class GameplaySetupSave
    {
        public int lives;
        public int playerJump;
        public int cointPoints;
        public int enemyDamage;
        public int enemySpawnFrequency;
        public int coinSpawnFrequency;
        //string currencyName;
    }
    [Serializable]
    public class LootboxSetupSave
    {
        //int lootBoxPrice;
    }
    [Serializable]
    public class AudioSave
    {
        AudioClip menuBackgroundMusic;
        AudioClip gameBackgroundMusic;
        AudioClip buttonClickSound;
        AudioClip prizeSound;
        AudioClip playerJumpSound;
        AudioClip coinCollectSound;
        AudioClip playerHurtSound;
    }

    //object classes
    public GameDataSave gameDataObject = new GameDataSave();
    public GameplaySetupSave gameplaySetupObject = new GameplaySetupSave();

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

        if(jsonGameData == null)
        {
            Debug.Log("jsonGameData Is null");
            SaveGameData(0, 0, null);
        }
        if (jsonGamePlaySetup == null)
        {
            Debug.Log("jsonGamePlaySetup Is null");
            SaveGamePlaySetup(3);
        }

        gameDataObject = JsonUtility.FromJson<GameDataSave>(jsonGameData);
        gameplaySetupObject = JsonUtility.FromJson<GameplaySetupSave>(jsonGamePlaySetup);
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
    public void SaveGamePlaySetup(int _lives)
    {
        gameplaySetupObject.lives = _lives;

        jsonGamePlaySetup = JsonUtility.ToJson(gameplaySetupObject);
    }
}
