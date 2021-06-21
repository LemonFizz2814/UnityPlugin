using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int lives;
    public int score;
    public int highscore;
    public int currency;

    public bool isDead;
    public bool gameStarted;

    public float jumpForce;

    string nameOfCurrency;

    public GameObject objectRenderer;

    private CanvasManager canvasManager;
    private DataSave dataSave;
    private GameManager gameManager;

    private AudioClip jumpSound;
    private AudioClip hurtSound;
    private AudioClip collectSound;
    private AudioSource audioSource;

    Rigidbody2D rg2D;

    private void Start()
    {
        canvasManager = FindObjectOfType<Canvas>().GetComponent<CanvasManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        dataSave = GameObject.FindGameObjectWithTag("DataSave").GetComponent<DataSave>();

        audioSource = gameObject.GetComponent<AudioSource>();

        //setup variables with saved variables
        lives = dataSave.gameplaySetupObject.lives;
        jumpForce = dataSave.gameplaySetupObject.playerJump;
        nameOfCurrency = dataSave.gameplaySetupObject.currencyName;
        jumpSound = dataSave.gameplaySetupObject.playerJumpSound;
        hurtSound = dataSave.gameplaySetupObject.playerHurtSound;
        collectSound = dataSave.gameplaySetupObject.coinCollectSound;
        currency = dataSave.gameDataObject.currency;
        highscore = dataSave.gameDataObject.highscore;

        canvasManager.UpdateCurrencyText(nameOfCurrency, currency);
        canvasManager.UpdateLivesText(lives);

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
        audioSource.clip = hurtSound;
        audioSource.Play();

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
        
        if (score > highscore)
        {
            //set highscore
            highscore = score;
        }

        //save variables
        dataSave.SaveGameData(currency, GetScore(), dataSave.gameDataObject.lootboxPrizes);
    }

    void CheckLivesLeft()
    {
        canvasManager.UpdateLivesText(lives);

        if(lives <= 0) //gameover state here
        {
            print("gameover");
            GameOver();
        }
    }

    public void UpdateVariables(string _nameOfCurrency, int _lives, Sprite _sprite)
    {
        nameOfCurrency = _nameOfCurrency;
        lives = _lives;
        gameObject.transform.GetComponent<SpriteRenderer>().sprite = _sprite;

        //canvasManager.UpdateCurrencyText(nameOfCurrency, currency);
        //canvasManager.UpdateLivesText(lives);
    }

    void IncreaseScore(int _points)
    {
        audioSource.clip = collectSound;
        audioSource.Play();

        score += _points;
        canvasManager.UpdateCurrencyText(nameOfCurrency, currency);
    }

    private void Update()
    {
        if(!isDead)
        {
            //keyboard testing controls
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            //mobile controls
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Jump();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(dataSave.gameplaySetupObject.enemyDamage);
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("Coin"))
        {
            IncreaseScore(dataSave.gameplaySetupObject.coinPoints);
            Destroy(other.gameObject);
        }
    }

    void Jump()
    {
        audioSource.clip = jumpSound;
        audioSource.Play();
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * jumpForce);
    }

    public int GetScore()
    {
        return score;
    }

    public string GetNameOfCurrency()
    {
        return nameOfCurrency;
    }
}
