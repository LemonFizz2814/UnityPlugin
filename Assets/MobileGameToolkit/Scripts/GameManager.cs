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

    List<GameObject> platforms = new List<GameObject>();

    GameObject mainCamera;

    public void StartGame()
    {
        gameStarted = true;
        platformObj = Resources.Load<GameObject>("Platform");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        StartCoroutine(PlatformSpawnLoop());
    }

    public void GameOver()
    {
        StopAllCoroutines();
        gameStarted = false;
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

    private IEnumerator PlatformSpawnLoop()
    {
        Vector3 spawnLoc = mainCamera.transform.GetChild(0).position;
        spawnLoc = new Vector3(spawnLoc.x, Random.Range(startHeightMin, startHeightMax), spawnLoc.z);

        GameObject _platformObj = Instantiate(platformObj, spawnLoc, Quaternion.identity);
        //platforms.Add(_platformObj);
        Destroy(_platformObj, platformLifeTime);

        yield return new WaitForSeconds(Random.Range(platformSpawnTimeMin, platformSpawnTimeMax));

        StartCoroutine(PlatformSpawnLoop());
    }
}
