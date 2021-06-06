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

    public float jumpForce;

    public string nameOfCurrency;

    public GameObject objectRenderer;

    private CanvasManager canvasManager;
    private GameManager gameManager;

    Rigidbody2D rg2D;

    private void Start()
    {
        canvasManager = FindObjectOfType<Canvas>().GetComponent<CanvasManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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

    //pause game when player dies
    public void GameOver()
    {
        canvasManager.ShowMenuObject(true, CanvasManager.MenuType.GameOver);
        gameManager.GameOver();
        rg2D.simulated = false;
        isDead = true;
    }

    void CheckLivesLeft()
    {
        if(lives <= 0) //gameover state here
        {
            print("gameover");
            GameOver();
        }
    }

    void IncreaseScore(int _points)
    {
        score += _points;
        canvasManager.UpdateCurrencyText(nameOfCurrency, currency);
    }

    private void Update()
    {
        if(!isDead)
        {
            //controls
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
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

    void Jump()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * jumpForce);
    }
}
