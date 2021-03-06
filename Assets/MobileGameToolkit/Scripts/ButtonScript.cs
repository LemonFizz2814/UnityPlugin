using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    CanvasManager canvasManager;
    PlayerScript playerScript;

    public CanvasManager.ButtonType buttonType;

    public AudioClip clickSound;

    DataSave dataSave;

    public string socialMediaURL;
    public int lootBoxPrice;

    private void Start()
    {
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        dataSave = GameObject.FindGameObjectWithTag("DataSave").GetComponent<DataSave>();
    }

    //set button type
    public void SetType(CanvasManager.ButtonType _type)
    {
        buttonType = _type;
    }

    //when button clicked
    public void OnButtonClicked()
    {
        GetComponent<AudioSource>().PlayOneShot(clickSound);

        switch (buttonType)
        {
            case CanvasManager.ButtonType.Play:
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart scene
                canvasManager.ShowMenuObject(true, CanvasManager.MenuType.GamePlay);
                canvasManager.StartGame();
                break;

            case CanvasManager.ButtonType.Exit:
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ApplicationQuit();
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
                if(Advertisement.IsReady())
                {
                    Advertisement.Show();
                }
                break;

            case CanvasManager.ButtonType.LootBox:
                if(lootBoxPrice >= playerScript.currency)
                {
                    playerScript.currency -= lootBoxPrice;

                    //buying lootbox
                    GameObject displayPrizeObj = Instantiate(Resources.Load<GameObject>("DisplayPrize"), new Vector3(0, 0, 0), Quaternion.identity);
                    displayPrizeObj.transform.SetParent(canvasManager.gameObject.transform);

                    //pick random prize to grant
                    int randPrize = Random.Range(0, dataSave.gameDataObject.lootboxPrizes.Count);
                    dataSave.gameDataObject.lootboxPrizes[randPrize] = true;

                    displayPrizeObj.transform.GetChild(1).GetComponent<Image>().sprite = null; //set sprite
                    displayPrizeObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ""; //set text
                }
                break;
            case CanvasManager.ButtonType.ClosePrize:
                Destroy(gameObject.transform.parent.gameObject);
                break;
            default:
                break;
        }
    }
}
