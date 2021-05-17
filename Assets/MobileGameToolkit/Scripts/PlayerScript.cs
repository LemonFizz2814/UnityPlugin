using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int lives;
    public int score;
    public int currency;

    public bool isDead;

    public string nameOfCurrency;

    public GameObject objectRenderer;

    private CanvasManager canvasManager;

    private void Start()
    {
        canvasManager = GameObject.FindObjectOfType<Canvas>().GetComponent<CanvasManager>();
        canvasManager.UpdateCurrencyText(nameOfCurrency, currency);
    }
}
