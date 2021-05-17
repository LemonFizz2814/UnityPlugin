using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int lives;
    public int score;
    public int currency;

    public bool isDead;
    public bool gameStarted;

    public string nameOfCurrency;

    public GameObject objectRenderer;

    private CanvasManager canvasManager;

    private void Start()
    {
        canvasManager = GameObject.FindObjectOfType<Canvas>().GetComponent<CanvasManager>();
        canvasManager.UpdateCurrencyText(nameOfCurrency, currency);
    }

    //When play is pressed
    public void StartGame()
    {
        isDead = false;
    }

    //Player taken damage
    public void TakeDamage(int _damage)
    {
        lives -= _damage;
        CheckLivesLeft();
    }

    void CheckLivesLeft()
    {
        if(lives <= 0) //gameover state here
        {
            canvasManager.ShowMenuObject(true, CanvasManager.Menu.GameOver);
            isDead = true;
        }
    }

    private void Update()
    {
        if(isDead)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(collision.gameObject.GetComponent<EnemyScript>().damage);
        }
    }
}
