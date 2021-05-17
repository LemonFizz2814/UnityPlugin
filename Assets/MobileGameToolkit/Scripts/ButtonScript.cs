using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    CanvasManager canvasManager;

    public CanvasManager.ButtonType buttonType;

    public string socialMediaURL;

    private void Start()
    {
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
    }

    public void OnButtonClicked()
    {
        switch(buttonType)
        {
            case CanvasManager.ButtonType.Play:
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart scene
                canvasManager.ShowMenuObject(true, CanvasManager.Menu.GamePlayScreen);
                canvasManager.StartGame();
                break;

            case CanvasManager.ButtonType.Exit:
                Application.Quit(); //exit application
                break;

            case CanvasManager.ButtonType.Shop:
                canvasManager.ShowMenuObject(true, CanvasManager.Menu.Shop);
                break;

            case CanvasManager.ButtonType.Back:
                canvasManager.ShowMenuObject(true, CanvasManager.Menu.Start);
                break;

            case CanvasManager.ButtonType.SocialMedia:
                Application.OpenURL(socialMediaURL);
                break;

            case CanvasManager.ButtonType.Settings:
                canvasManager.ShowMenuObject(true, CanvasManager.Menu.Settings);
                break;

            case CanvasManager.ButtonType.Advert:
                //play advert here
                break;

            default:
                break;
        }
    }
}
