using UnityEngine;
using UnityEngine.UI;

public class CaptchaUI : MonoBehaviour
{
    [Header("UI References:")]
    [SerializeField] private Image uiCodeImage;
    [SerializeField] private InputField uiCodeInput;
    [SerializeField] private Text uiErrorsText;
    [SerializeField] private Button uiSubmitButton;


    [Header("Captcha Generator:")]
    [SerializeField] private CaptchaGenerator captchaGenerator;


    [Header("Sonidos")]
    public AudioSource audioSource2;
    public AudioClip submitAudio;
    public AudioClip errorAudio;

    private Captcha currentCaptcha; // la instancia del "struct" captcha


    [SerializeField] private CaptchaFlowManager flowManager;

    private void Start()
    {
        // validación inicial
        if (uiCodeImage == null || uiCodeInput == null || uiErrorsText == null || uiSubmitButton == null || captchaGenerator == null)
        {
            Debug.LogError(" Uno o más campos del inspector no están asignados en CaptchaUI.");
            return;
        }

        // Generar primer captcha
        GenerateCaptcha();

        // Asignar eventos
        uiSubmitButton.onClick.AddListener(Submit);
    }

    private void GenerateCaptcha()
    {
        currentCaptcha = captchaGenerator.Generate();

        if (currentCaptcha == null || currentCaptcha.Image == null)
        {
            Debug.LogError(" El captcha generado es nulo o no tiene imagen.");
            return;
        }

        // Actualizar la UI
        uiCodeImage.sprite = currentCaptcha.Image;
        uiErrorsText.gameObject.SetActive(false);
        uiCodeInput.text = "";  // Limpiar campo de texto

        // Activar teclado móvil automáticamente
        uiCodeInput.Select();
        uiCodeInput.ActivateInputField();
    }


    private void Submit()
    {
        if (currentCaptcha == null || captchaGenerator == null)
            return;

        string enteredCode = uiCodeInput.text;

        if (captchaGenerator.IsCodeValid(enteredCode, currentCaptcha))
        {
            uiErrorsText.gameObject.SetActive(false);

            //  Aquí notificamos al sistema que se resolvió correctamente
            audioSource2.PlayOneShot(submitAudio);
            flowManager.OnCaptchaResuelto();
        }
        else
        {
            audioSource2.PlayOneShot(errorAudio);
            uiErrorsText.gameObject.SetActive(true);
        }
    }

    public void ReiniciarCaptchaTexto()
    {
        GenerateCaptcha();
    }





}
