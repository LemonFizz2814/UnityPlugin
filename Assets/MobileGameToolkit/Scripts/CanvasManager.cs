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
    };
    public enum Menu
    {
        GameOver,
        Start,
        Settings,
        Shop,
        ENUM_LENGTH,
    };

    string[] menuTagStrings = { "GameOverMenu", "StartMenu", "SettingsMenu", "ShopMenu" };

    List<GameObject> menuObjList = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < (int)Menu.ENUM_LENGTH; i++)
        {
            menuObjList.Add(FindObject(menuTagStrings[i]));
            ShowMenuObject(false, (Menu)i);
        }

        ShowMenuObject(true, Menu.Start);
    }

    public void ShowMenuObject(bool _active, Menu _menuType)
    {
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
            print("Error, " + _menuType.ToString() + " Menu not found, place the prefab in Canvas to call to it"); //error handling for prefab not found
        }
    }

    public GameObject FindObject(string _tag)
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.CompareTag(_tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public void UpdateCurrencyText(string _nameOfCurrency, int _currency)
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.CompareTag("CurrencyText"))
            {
                child.GetComponent<TextMeshProUGUI>().text = _nameOfCurrency + ": " + _currency;
            }
        }
    }
}
