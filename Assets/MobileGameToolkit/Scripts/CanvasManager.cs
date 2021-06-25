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
        LootBox,
        ClosePrize,
    };
    public enum MenuType
    {
        Start,
        GameOver,
        Settings,
        Shop,
        GamePlay,
    };
    public enum TextType
    {
        Info,
        Currency,
        Score,
        Lives,
        Highscore,
    };

    string[] menuTagStrings = { "StartMenu", "GameOverMenu", "SettingsMenu", "ShopMenu", "GamePlayMenu" };

    List<GameObject> menuObjList = new List<GameObject>();

    PlayerScript playerScript;
    GameManager gameManager;

    DataSave dataSave;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        dataSave = GameObject.FindGameObjectWithTag("DataSave").GetComponent<DataSave>();

        if (playerScript == null)
        {
            print("ERROR, player object not found");
        }

        //add all menu objects into list
        for (int i = 0; i < System.Enum.GetValues(typeof(MenuType)).Length; i++)
        {
            //menuObjList.Add(FindObject(menuTagStrings[i])[0]);
            if (FindObject(menuTagStrings[i])[0] != null)
            {
                menuObjList.Add(FindObject(menuTagStrings[i])[0]);
            }
            //FindObject(menuTagStrings[i])[0].gameObject.SetActive(false);
            ShowMenuObject(false, (MenuType)i);
        }

        ShowMenuObject(true, MenuType.Start);
    }

    //display menu objects
    public void ShowMenuObject(bool _active, MenuType _menuType)
    {
        //update all text types
        UpdateCurrencyText(playerScript.GetNameOfCurrency(), playerScript.currency);
        UpdateLivesText(playerScript.lives);
        UpdateScoreText(playerScript.score);

        //check if menu exists in menu
        if (menuObjList[(int)_menuType] != null)
        {
            for(int i = 0; i < menuObjList.Count; i++)
            {
                menuObjList[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            menuObjList[(int)_menuType].transform.GetChild(0).gameObject.SetActive(_active);
        }
        else //error check
        {
            print("ERROR, " + _menuType.ToString() + " Menu not found, place the prefab in Canvas to call to it"); //error handling for prefab not found
        }
    }

    //Find object in scene with tag
    public List<GameObject> FindObject(string _tag)
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();

        List<GameObject> listOfChildren = new List<GameObject>();
        //GameObject[] listOfChildren;

        foreach (Transform child in children)
        {
            if (child.CompareTag(_tag))
            {
                //listOfChildren[listOfChildren.Length] = child.gameObject;
                listOfChildren.Add(child.gameObject);
                //return child.gameObject;
            }
        }
        return listOfChildren;
    }

    //update all the currency text
    public void UpdateCurrencyText(string _nameOfCurrency, int _currency)
    {
        foreach(GameObject instance in GameObject.FindGameObjectsWithTag("CurrencyText"))
        {
            instance.GetComponent<TextMeshProUGUI>().text = _nameOfCurrency + ": " + _currency;
        }
    }

    //update all live text
    public void UpdateLivesText(int _lives)
    {
        foreach (GameObject instance in GameObject.FindGameObjectsWithTag("LivesText"))
        {
            instance.GetComponent<TextMeshProUGUI>().text = "Lives: " + _lives;
        }
    }

    //update all score text
    public void UpdateScoreText(int _score)
    {
        foreach (GameObject instance in GameObject.FindGameObjectsWithTag("ScoreText"))
        {
            instance.GetComponent<TextMeshProUGUI>().text = "Score: " + _score;
        }
    }

    //call when game begins
    public void StartGame()
    {
        gameManager.StartGame();
        playerScript.StartGame();
        //Instantiate(Resources.Load<GameObject>("GameManager"), new Vector3(0, 0, 0), Quaternion.identity);
    }
}
