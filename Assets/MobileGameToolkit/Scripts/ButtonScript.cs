using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    CanvasManager canvasManager;
    PlayerScript playerScript;

    public CanvasManager.ButtonType buttonType;

    public string socialMediaURL;
    public int lootBoxPrice;

    private void Start()
    {
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    public void SetType(CanvasManager.ButtonType _type)
    {
        buttonType = _type;
    }

    public void OnButtonClicked()
    {
        switch(buttonType)
        {
            case CanvasManager.ButtonType.Play:
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart scene
                canvasManager.ShowMenuObject(true, CanvasManager.MenuType.GamePlayScreen);
                canvasManager.StartGame();
                break;

            case CanvasManager.ButtonType.Exit:
                Application.Quit(); //exit application
                break;

            case CanvasManager.ButtonType.Shop:
                canvasManager.ShowMenuObject(true, CanvasManager.MenuType.Shop);
                break;

            case CanvasManager.ButtonType.Back:
                canvasManager.ShowMenuObject(true, CanvasManager.MenuType.Start);
                break;

            case CanvasManager.ButtonType.SocialMedia:
                Application.OpenURL(socialMediaURL);
                break;

            case CanvasManager.ButtonType.Settings:
                canvasManager.ShowMenuObject(true, CanvasManager.MenuType.Settings);
                break;

            case CanvasManager.ButtonType.Advert:
                //play advert here
                break;

            case CanvasManager.ButtonType.LootBox:
                if(lootBoxPrice >= playerScript.currency)
                {
                    playerScript.currency -= lootBoxPrice;

                    //buying lootbox
                }
                break;

            default:
                break;
        }
    }
}
