using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class CaptchaMathManager : MonoBehaviour
{
    public TMP_Text operationText;
    public TMP_InputField inputField;
    public Button checkButton;
    public TMP_Text feedbackText;

    public CaptchaFlowManager flowManager;


    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip submitAudio;
    public AudioClip errorAudio;

    private int correctAnswer;

    void Start()
    {
        GenerateChallenge();
        checkButton.onClick.AddListener(VerificarCaptcha);
    }

    public void GenerateChallenge()
    {
        int a = Random.Range(0, 21);
        int b = Random.Range(0, 21);

        int big = Mathf.Max(a, b);
        int small = Mathf.Min(a, b);

        bool isAddition = Random.value < 0.5f;

        if (isAddition)
        {
            correctAnswer = big + small;
            operationText.text = $"{big} + {small}";
        }
        else
        {
            correctAnswer = big - small;
            operationText.text = $"{big} - {small}";
        }

        inputField.text = "";
        feedbackText.text = "";
        inputField.Select();
        inputField.ActivateInputField();

    }

    void VerificarCaptcha()
    {
        feedbackText.text = "";  // Limpiar el feedback al inicio

        if (int.TryParse(inputField.text, out int userAnswer))
        {
            if (userAnswer == correctAnswer)
            {
                audioSource.PlayOneShot(submitAudio);
                // Notificar al FlowManager que se resolvió el captcha
                flowManager.OnCaptchaResuelto();

                // Desactivar este panel
                gameObject.SetActive(false);
            }
            else
            {
                feedbackText.text = " Respuesta incorrecta. Intenta de nuevo.";
                feedbackText.color = Color.red;
                audioSource.PlayOneShot(errorAudio);
            }
        }
    }

}
