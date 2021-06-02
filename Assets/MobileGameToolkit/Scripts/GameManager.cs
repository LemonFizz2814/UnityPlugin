using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float startPos = 6;
    float startHeightMin = -5.5f;
    float startHeightMax = -3.2f;

    float platformLifeTime = 6.0f;

    float platformSpawnTime = 4.0f;

    Vector3 platformSpeed = new Vector3(3, 0, 0);

    bool gameStarted = false;

    GameObject platformObj;

    List<GameObject> platforms = new List<GameObject>();

    public void StartGame()
    {
        StartCoroutine(PlatformSpawnLoop());
        gameStarted = true;
        platformObj = Resources.Load<GameObject>("Platform");
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
            for(int i = 0; i < platforms.Count; i++)
            {
                if(platforms[i] == null)
                {
                    platforms.RemoveAt(i);
                }
                else
                {
                    platforms[i].transform.position -= platformSpeed * Time.deltaTime;
                }
            }
        }
    }

    private IEnumerator PlatformSpawnLoop()
    {
        yield return new WaitForSeconds(platformSpawnTime);

        GameObject _platformObj = Instantiate(platformObj, new Vector3(startPos, Random.Range(startHeightMin, startHeightMax), 0), Quaternion.identity);
        platforms.Add(_platformObj);
        Destroy(_platformObj, platformLifeTime);

        StartCoroutine(PlatformSpawnLoop());
    }
}
