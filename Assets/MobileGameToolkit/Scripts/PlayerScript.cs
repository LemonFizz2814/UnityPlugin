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

    Rigidbody2D rg2D;

    private void Start()
    {
        canvasManager = GameObject.FindObjectOfType<Canvas>().GetComponent<CanvasManager>();
        canvasManager.UpdateCurrencyText(nameOfCurrency, currency);

        rg2D = GetComponent<Rigidbody2D>();
        rg2D.simulated = false;
    }

    //When play is pressed
    public void StartGame()
    {
        rg2D.simulated = true;
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
            canvasManager.ShowMenuObject(true, CanvasManager.MenuType.GameOver);
            isDead = true;
        }
    }

    void IncreaseScore(int _points)
    {
        score += _points;
        canvasManager.UpdateCurrencyText(nameOfCurrency, currency);
    }

    private void Update()
    {
        if(isDead)
        {
            //transform.position = new Vector3(0, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(other.gameObject.GetComponent<EnemyScript>().damage);
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("Coin"))
        {
            IncreaseScore(other.gameObject.GetComponent<CoinScript>().points);
            Destroy(other.gameObject);
        }
    }
}
