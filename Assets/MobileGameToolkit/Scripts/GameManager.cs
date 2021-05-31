using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool gameStarted = false;

    public void StartGame()
    {
        gameStarted = true;
    }
    public void GameOver()
    {
        gameStarted = false;
    }

    private void Update()
    {
        if(gameStarted)
        {

        }
    }

    private IEnumerator PlatformSpawnLoop()
    {
        yield return new WaitForSeconds(3);
    }
}
