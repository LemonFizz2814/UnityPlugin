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
        public List<bool> lootboxPrizes = new List<bool>();
    }
    [Serializable]
    public class MenuSetupSave
    {

    }
    [Serializable]
    public class ButtonSetupSave
    {

    }
    [Serializable]
    public class GameplaySetupSave
    {
        public int lives;
        //string currencyName;
    }
    [Serializable]
    public class LootboxSetupSave
    {
        //int lootBoxPrice;
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
