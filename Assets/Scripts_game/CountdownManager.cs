using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownManager : MonoBehaviour
{
    [Header("UI de conteo")]
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI infoText;
    public float startCount = 3f;
    public GameObject timer;


    [Header("Referencias de flujo")]
    public CaptchaFlowManager flowManager;


    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip countdownBeep;
    public AudioClip startSound;


    void Start()
    {
        Time.timeScale = 1f; // esto reanuda el tiempo por si quedó en 0, cuando se hacia un segundo ciclo de juego, este nunca comenzaba
        StartCoroutine(CountdownCoroutine());
    }


    IEnumerator CountdownCoroutine() // Se encarga de elegir un panel captcha al azar de los disponibles
    {
        float count = startCount;
        while (count > 0)
        {
            countdownText.text = Mathf.Ceil(count).ToString();

            // reproducir beep para 3, 2, 1
            if (audioSource != null && countdownBeep != null)
                audioSource.PlayOneShot(countdownBeep);

            yield return new WaitForSeconds(1f);
            count -= 1f;
        }

        countdownText.text = "Start!";

        // reproducir sonido distinto para "Start"
        if (audioSource != null && startSound != null)
            audioSource.PlayOneShot(startSound);

        // esperar exactamente lo que dure el sonido de inicio
        yield return new WaitForSeconds(startSound != null ? startSound.length : 1f);

        // Ocultar textos de conteo
        countdownText.gameObject.SetActive(false);
        infoText.gameObject.SetActive(false);
        timer.gameObject.SetActive(true);

        GameTime.Instance.StartTimer();


        // Notificar al controlador de flujo que inicie el captcha
        flowManager.MostrarCaptcha();
    }


}
