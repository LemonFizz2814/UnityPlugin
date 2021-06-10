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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlaySound(menuBackgroundMusic);
    }

    public void StartGame()
    {
        gameStarted = true;
        platformObj = Resources.Load<GameObject>("Platform");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerObj = GameObject.FindGameObjectWithTag("Player");

        dataSave = GameObject.FindGameObjectWithTag("DataSave").GetComponent<DataSave>();

        enemyObj = Resources.Load<GameObject>("EnemyPrefab");
        coinObj = Resources.Load<GameObject>("CoinPrefab");

        PlaySound(gameBackgroundMusic);

        StartCoroutine(PlatformSpawnLoop());
    }

    public void GameOver()
    {
        StopAllCoroutines();
        gameStarted = false;
        CalculateHighScore();

        PlaySound(menuBackgroundMusic);
    }

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

    void CalculateHighScore()
    {
        if (playerObj.GetComponent<PlayerScript>().GetScore() > dataSave.gameDataObject.highscore)
        {
            //save and show highscore
            dataSave.SaveGameData(dataSave.gameDataObject.currency, playerObj.GetComponent<PlayerScript>().GetScore(), dataSave.gameDataObject.lootboxPrizes);
        }
    }

    private IEnumerator PlatformSpawnLoop()
    {
        Vector3 spawnLoc = mainCamera.transform.GetChild(0).position;
        spawnLoc = new Vector3(spawnLoc.x, Random.Range(startHeightMin, startHeightMax), spawnLoc.z);

        GameObject _platformObj = Instantiate(platformObj, spawnLoc, Quaternion.identity);
        GameObject _enemyObj = Instantiate(enemyObj, new Vector3(spawnLoc.x, spawnLoc.y + 5, spawnLoc.z), Quaternion.identity);
        //platforms.Add(_platformObj);
        Destroy(_platformObj, platformLifeTime);

        yield return new WaitForSeconds(Random.Range(platformSpawnTimeMin, platformSpawnTimeMax));

        StartCoroutine(PlatformSpawnLoop());
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }
}
