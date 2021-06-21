using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float startHeightMin = -6.0f;
    float startHeightMax = -2.8f;

    float platformSpawnTimeMin = 2.8f;
    float platformSpawnTimeMax = 3.2f;

    float platformLifeTime = 7.0f;

    float amountOfCoinsSpawned = 4;
    float coinSpawnHeight = 5;
    float coinplatformWidth = 3;

    float enemySpawnHeight = 5;

    Vector3 playerSpeed = new Vector3(3, 0, 0);

    bool gameStarted = false;

    GameObject platformObj;
    GameObject mainCamera;
    GameObject playerObj;
    GameObject enemyObj;
    GameObject coinObj;

    DataSave dataSave;

    AudioClip gameBackgroundMusic;
    AudioClip menuBackgroundMusic;

    AudioSource audioSource;

    enum EPLATFORMTYPES
    {
        NORMAL,
        ENEMY,
        COIN,
    }

    EPLATFORMTYPES platformType;

    private void Start()
    {
        dataSave = GameObject.FindGameObjectWithTag("DataSave").GetComponent<DataSave>();

        audioSource = GetComponent<AudioSource>();

        menuBackgroundMusic = dataSave.gameplaySetupObject.menuBackgroundMusic;
        gameBackgroundMusic = dataSave.gameplaySetupObject.gameBackgroundMusic;

        PlaySound(menuBackgroundMusic);
    }

    //when game has started
    public void StartGame()
    {
        gameStarted = true;
        platformObj = Resources.Load<GameObject>("Platform");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerObj = GameObject.FindGameObjectWithTag("Player");

        enemyObj = Resources.Load<GameObject>("EnemyPrefab");
        coinObj = Resources.Load<GameObject>("CoinPrefab");

        PlaySound(gameBackgroundMusic);

        StartCoroutine(PlatformSpawnLoop());
    }

    //when the game scene has ended
    public void GameOver()
    {
        StopAllCoroutines();
        gameStarted = false;
        CalculateHighScore();

        PlaySound(menuBackgroundMusic);
    }

    //play audio
    public void PlaySound(AudioClip _audio)
    {
        if(_audio != null)
        {
            audioSource.clip = _audio;
            audioSource.Play();
        }
    }

    private void FixedUpdate()
    {
        if(gameStarted)
        {
            mainCamera.transform.position += playerSpeed * Time.deltaTime;

            /*for(int i = 0; i < platforms.Count; i++)
            {
                if(platforms[i] == null)
                {
                    platforms.RemoveAt(i);
                }
                else
                {
                    platforms[i].transform.position += platformSpeed * Time.deltaTime;
                }
            }*/
        }
    }

    //check if highscore has been reached
    void CalculateHighScore()
    {
        if (playerObj.GetComponent<PlayerScript>().GetScore() > dataSave.gameDataObject.highscore)
        {
            //save and show highscore
            dataSave.SaveGameData(dataSave.gameDataObject.currency, playerObj.GetComponent<PlayerScript>().GetScore(), dataSave.gameDataObject.lootboxPrizes);
        }
    }

    //loop through platform spawning
    private IEnumerator PlatformSpawnLoop()
    {
        Vector3 spawnLoc = mainCamera.transform.GetChild(0).position;
        spawnLoc = new Vector3(spawnLoc.x, Random.Range(startHeightMin, startHeightMax), spawnLoc.z);

        GameObject _platformObj = Instantiate(platformObj, spawnLoc, Quaternion.identity);
        Destroy(_platformObj, platformLifeTime);

        //make random platform type
        platformType = (EPLATFORMTYPES)Random.Range(0, System.Enum.GetValues(typeof(EPLATFORMTYPES)).Length);

        switch (platformType)
        {
            case EPLATFORMTYPES.NORMAL:
                //put anything else in here for normal if wanted
                break;

            case EPLATFORMTYPES.COIN:
                //spawn coins on platform
                for (int i = 0; i < amountOfCoinsSpawned; i++)
                {
                    GameObject _coinObj = Instantiate(coinObj, new Vector3(spawnLoc.x + (-coinplatformWidth + (i * 2)), spawnLoc.y + coinSpawnHeight, spawnLoc.z), Quaternion.identity);
                    _coinObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = dataSave.gameplaySetupObject.coinSprite;
                    Destroy(_coinObj, platformLifeTime);
                }
                break;

            case EPLATFORMTYPES.ENEMY:
                //spawn enemy on platform
                GameObject _enemyObj = Instantiate(enemyObj, new Vector3(spawnLoc.x, spawnLoc.y + enemySpawnHeight, spawnLoc.z), Quaternion.identity);
                _enemyObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = dataSave.gameplaySetupObject.enemySprite;
                Destroy(_enemyObj, platformLifeTime);
                break;
        }

        yield return new WaitForSeconds(Random.Range(platformSpawnTimeMin, platformSpawnTimeMax));

        StartCoroutine(PlatformSpawnLoop());
    }

    //called when game is quit, save variables before quitting
    public void ApplicationQuit()
    {
        //save variables before quitting
        dataSave.SaveGameData(playerObj.GetComponent<PlayerScript>().currency, playerObj.GetComponent<PlayerScript>().GetScore(), dataSave.gameDataObject.lootboxPrizes);

        Application.Quit();
    }
}
