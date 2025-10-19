using UnityEngine;
using UnityEngine.UI;

public class CaptchaButtonController : MonoBehaviour
{
    public Sprite newSprite;
    public Button botonParaActivar;  // El otro botón

    private Image imageComponent;
    private Button buttonComponent;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        buttonComponent = GetComponent<Button>();

        if (buttonComponent != null)
            buttonComponent.onClick.AddListener(OnButtonPressed);

        // Asegúrate de que empiece desactivado (opcional, puedes controlarlo desde Inspector también)
        if (botonParaActivar != null)
            botonParaActivar.interactable = false;
    }

    void OnButtonPressed()
    {
        if (newSprite != null)
            imageComponent.sprite = newSprite;

        buttonComponent.interactable = false;

        // Activar el otro botón
        if (botonParaActivar != null)
            botonParaActivar.interactable = true;
    }
}
