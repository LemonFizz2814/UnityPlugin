using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataSave : MonoBehaviour
{
    public int highscore;
    public int currency;

    public List<bool> lootboxPrizes = new List<bool>();

    public void LoadData()
    {
        for (int i = 0; i < lootboxPrizes.Count; i++)
        {
            lootboxPrizes[i] = (PlayerPrefs.GetInt("lootboxPrizes" + i) == 1) ? true: false;
        }

        highscore = PlayerPrefs.GetInt("highscore");
        currency = PlayerPrefs.GetInt("currency");
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("highscore", highscore);
        PlayerPrefs.SetInt("currency", currency);

        for(int i = 0; i < lootboxPrizes.Count; i++)
        {
            PlayerPrefs.SetInt("lootboxPrizes" + i, Convert.ToInt32(lootboxPrizes[i]));
        }
    }
}
