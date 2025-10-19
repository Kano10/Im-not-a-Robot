using UnityEngine;

public class CaptchaFlowManager : MonoBehaviour
{
    public GameObject captchaTextoPanel;
    public GameObject captchaImagenPanel;

    public GameObject captchaMathPanel;
    public CaptchaMathManager captchaMathManager;


    public CaptchaUI captchaUI; 
    public CaptchaImagenManager captchaImagenManager;
    private int faseActual = 0;

    [Header("M�sica de fondo")]
    public AudioSource musicSource;
    public AudioClip backgroundMusic;


    public void MostrarCaptcha()
    {
        // Iniciar m�sica si a�n no est� sonando
        if (musicSource != null && backgroundMusic != null && !musicSource.isPlaying)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        faseActual = Random.Range(0, 3);

        MostrarPanelSegunFase();
    }



    public void OnCaptchaResuelto()
    {
        GameTime.Instance.AddTime();

        captchaTextoPanel.SetActive(false);
        captchaImagenPanel.SetActive(false);
        captchaMathPanel.SetActive(false);

        faseActual = Random.Range(0, 3);
        MostrarPanelSegunFase();

        Debug.Log($"Siguiente captcha: {(faseActual == 0 ? "Texto" : (faseActual == 1 ? "Imagen" : "Math"))}");
    }


    private void MostrarPanelSegunFase()
    {
        captchaTextoPanel.SetActive(false);
        captchaImagenPanel.SetActive(false);
        captchaMathPanel.SetActive(false);

        if (faseActual == 0)
        {
            captchaTextoPanel.SetActive(true);
            captchaUI.ReiniciarCaptchaTexto();
        }
        else if (faseActual == 1)
        {
            captchaImagenPanel.SetActive(true);
            captchaImagenManager.ReiniciarCaptchaImagen();
        }
        else if (faseActual == 2)
        {
            captchaMathPanel.SetActive(true);
            captchaMathManager.GenerateChallenge();
        }
    }

}

