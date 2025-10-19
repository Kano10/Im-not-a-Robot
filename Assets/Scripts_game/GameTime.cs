using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameTime : MonoBehaviour
{
    [Header("Tiempo de juego")]
    public float startTime = 10f;
    public float bonusTime = 8f;
    public GameObject gameOverPanel;

    [Header("Texto UI")]
    public TMP_Text timeText;

    [Header("Audio")]
    public GameObject audioManager; // referencia al GameObject con el AudioSource

    [Header("Game Over UI")]
    public TMP_Text scoreText;
    public TMP_Text finalScore;
    public Button backToMenuButton;

    private int captchasResueltos = 0;


    private float timeRemaining;
    private bool gameActive = true; //con esto espero que la cuenta inicial termine para cambiar a algún panel de juego

    public static GameTime Instance { get; private set; }

    public CaptchaFlowManager flowManager; 



    void Awake()
    {
        if (Instance == null) Instance = this;
        timeRemaining = startTime;
        gameActive = false; // para que no empiece solo
        UpdateTimeDisplay(timeRemaining);//display del tiempo
    }

    void Update()//encargado de actualizar el tiempo durante el ciclo
    {
        if (!gameActive) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            audioManager.SetActive(false);
            GameOver();
        }

        UpdateTimeDisplay(timeRemaining);
    }

    public void AddTime()//Si se acierta algun captcha, se suma tiempo
    {
        if (!gameActive) return;

        timeRemaining += bonusTime;
        captchasResueltos++;
        UpdateTimeDisplay(timeRemaining);
    }
    private void GameOver()
    {
        gameActive = false;
        UpdateTimeDisplay(0f);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        timeText.gameObject.SetActive(false);

        // Detener música
        if (audioManager != null)
        {
            audioManager.SetActive(false);
        }

        // Desactivar captchas
        if (flowManager != null)
        {
            flowManager.captchaTextoPanel.SetActive(false);
            flowManager.captchaImagenPanel.SetActive(false);
            flowManager.captchaMathPanel.SetActive(false);
        }

        // Mostrar puntaje
        if (finalScore != null)
        {
            finalScore.text = $"{captchasResueltos}";
        }

        // Activar botón para volver
        if (backToMenuButton != null)
        {
            backToMenuButton.gameObject.SetActive(true);
            backToMenuButton.onClick.RemoveAllListeners();
            backToMenuButton.onClick.AddListener(() => VolverAlMenu());
        }
    }
    private void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Cambia al índice de tu menú
    }

    private void UpdateTimeDisplay(float time)
    {
        time = Mathf.Max(0f, time);//evita tiempo negativo
        timeText.text = $"Time: {time:F1}s";
    }

    public void StartTimer()//se llama en CountdownManager
    {
        gameActive = true;
    }
}
