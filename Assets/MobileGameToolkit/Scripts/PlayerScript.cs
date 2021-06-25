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
    float jumpWait;
    float jumpTime = 0.7f;

    string nameOfCurrency;

    public GameObject objectRenderer;

    private CanvasManager canvasManager;
    private DataSave dataSave;
    private GameManager gameManager;

    private AudioClip jumpSound;
    private AudioClip hurtSound;
    private AudioClip collectSound;
    private AudioSource audioSource;

    private Vector3 startingPos = new Vector3(-2.0f, -0.5f, 10.0f);

    public GameObject background;

    Rigidbody2D rg2D;

    private void Start()
    {
        canvasManager = FindObjectOfType<Canvas>().GetComponent<CanvasManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        dataSave = GameObject.FindGameObjectWithTag("DataSave").GetComponent<DataSave>();

        audioSource = gameObject.GetComponent<AudioSource>();

        //background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(dataSave.gameplaySetupObject.backgroundSprite);
        //GetComponent<SpriteRenderer>().sprite = dataSave.gameplaySetupObject.playerSprite;

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

        //activate player
        rg2D = GetComponent<Rigidbody2D>();
        rg2D.simulated = false;
    }

    private void Update()
    {
        jumpWait -= Time.deltaTime;

        if (!isDead)
        {
            //keyboard testing controls
            if (Input.GetKeyDown(KeyCode.Space))
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
        //player hits enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(dataSave.gameplaySetupObject.enemyDamage);
            Destroy(other.gameObject);
        }
        //player hit coins
        if (other.gameObject.CompareTag("Coin"))
        {
            IncreaseCurrency(dataSave.gameplaySetupObject.coinPoints);
            Destroy(other.gameObject);
        }
        //player goes out of bounds
        if (other.gameObject.CompareTag("Boundary"))
        {
            TakeDamage(lives);
        }
    }

    //When play is pressed
    public void StartGame()
    {
        StartCoroutine(ScoreIncrease());

        transform.localPosition = startingPos;
        rg2D.simulated = true;
        isDead = false;

        //reset variables
        lives = dataSave.gameplaySetupObject.lives;        
        currency = dataSave.gameDataObject.currency;
        highscore = dataSave.gameDataObject.highscore;
        score = 0;

        //update canvas
        canvasManager.UpdateCurrencyText(nameOfCurrency, currency);
        canvasManager.UpdateLivesText(lives);
        canvasManager.UpdateScoreText(score);
    }

    //Player taken damage
    public void TakeDamage(int _damage)
    {
        audioSource.clip = hurtSound;
        audioSource.Play();

        lives -= _damage;
        canvasManager.UpdateLivesText(lives);

        CheckLivesLeft();
    }

    //pause game when player dies
    public void GameOver()
    {
        //update canvas
        canvasManager.ShowMenuObject(true, CanvasManager.MenuType.GameOver);
        gameManager.GameOver();
        canvasManager.UpdateCurrencyText(nameOfCurrency, currency);
        canvasManager.UpdateScoreText(score);

        //freeze player object
        rg2D.simulated = false;
        isDead = true;
        
        if (score > highscore)
        {
            //set highscore
            highscore = score;
        }

        //save variables
        dataSave.SaveGameData(currency, highscore, dataSave.gameDataObject.lootboxPrizes);
    }

    //check if player has died
    void CheckLivesLeft()
    {
        if(lives <= 0) //gameover state here
        {
            print("gameover");
            GameOver();
        }
    }

    //increase currency variable
    void IncreaseCurrency(int _points)
    {
        //play sound
        audioSource.clip = collectSound;
        audioSource.Play();

        currency += _points;
        canvasManager.UpdateCurrencyText(nameOfCurrency, currency);
    }

    //increase score every second
    private IEnumerator ScoreIncrease()
    {
        score += 1;
        canvasManager.UpdateScoreText(score);

        yield return new WaitForSeconds(1);

        if (!isDead)
        {
            StartCoroutine(ScoreIncrease());
        }
    }

    //make player jump
    void Jump()
    {
        if (jumpWait < 0)
        {
            jumpWait = jumpTime;
            audioSource.clip = jumpSound;
            audioSource.Play();
            gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * jumpForce);
        }
    }

    //return of the player
    public int GetScore()
    {
        return score;
    }

    //return name of currency
    public string GetNameOfCurrency()
    {
        return nameOfCurrency;
    }
}
