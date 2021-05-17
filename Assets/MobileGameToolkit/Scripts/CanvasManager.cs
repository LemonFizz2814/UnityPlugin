using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public enum ButtonType
    {
        Play,
        Exit,
        Shop,
        Back,
        SocialMedia,
        Settings,
        Advert,
    };
    public enum Menu
    {
        GameOver,
        Start,
        Settings,
        Shop,
        GamePlayScreen,
        ENUM_LENGTH,
    };

    string[] menuTagStrings = { "GameOverMenu", "StartMenu", "SettingsMenu", "ShopMenu", "GamePlayScreen" };

    List<GameObject> menuObjList = new List<GameObject>();

    PlayerScript playerScript;

    private void Start()
    {
        for (int i = 0; i < (int)Menu.ENUM_LENGTH; i++)
        {
            menuObjList.Add(FindObject(menuTagStrings[i])[0]);
            ShowMenuObject(false, (Menu)i);
        }

        ShowMenuObject(true, Menu.Start);

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        if(playerScript == null)
        {
            print("ERROR, player object not found");
        }
    }

    public void ShowMenuObject(bool _active, Menu _menuType)
    {
        UpdateCurrencyText(playerScript.nameOfCurrency, playerScript.currency);
        UpdateLivesText(playerScript.lives);
        UpdateScoreText(playerScript.score);

        if (menuObjList[(int)_menuType] != null) //check if menu exists in menu
        {
            for(int i = 0; i < menuObjList.Count; i++)
            {
                menuObjList[i].SetActive(false);
            }
            menuObjList[(int)_menuType].SetActive(_active);
        }
        else
        {
            print("ERROR, " + _menuType.ToString() + " Menu not found, place the prefab in Canvas to call to it"); //error handling for prefab not found
        }
    }

    public GameObject[] FindObject(string _tag)
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();

        //List<GameObject> listOfChildren = new List<GameObject>();
        GameObject[] listOfChildren = null;

        foreach (Transform child in children)
        {
            if (child.CompareTag(_tag))
            {
                listOfChildren[listOfChildren.Length] = child.gameObject;
                //return child.gameObject;
            }
        }
        //return null;
        return listOfChildren;
    }

    public void UpdateCurrencyText(string _nameOfCurrency, int _currency)
    {
        foreach(GameObject instance in FindObject("CurrencyText"))
        {
            instance.GetComponent<TextMeshProUGUI>().text = _nameOfCurrency + ": " + _currency;
        }
    }

    public void UpdateLivesText(int _lives)
    {
        foreach (GameObject instance in FindObject("LivesText"))
        {
            instance.GetComponent<TextMeshProUGUI>().text = "Lives: " + _lives;
        }
    }
    public void UpdateScoreText(int _score)
    {
        foreach (GameObject instance in FindObject("ScoreText"))
        {
            instance.GetComponent<TextMeshProUGUI>().text = "Score: " + _score;
        }
    }

    public void StartGame()
    {
        playerScript.StartGame();
    }
}
