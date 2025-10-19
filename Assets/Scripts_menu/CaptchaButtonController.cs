using UnityEngine;
using UnityEngine.UI;

public class CaptchaButtonController : MonoBehaviour
{
    public Sprite newSprite;
    public Button botonParaActivar;  // El otro bot�n

    private Image imageComponent;
    private Button buttonComponent;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        buttonComponent = GetComponent<Button>();

        if (buttonComponent != null)
            buttonComponent.onClick.AddListener(OnButtonPressed);

        // Aseg�rate de que empiece desactivado (opcional, puedes controlarlo desde Inspector tambi�n)
        if (botonParaActivar != null)
            botonParaActivar.interactable = false;
    }

    void OnButtonPressed()
    {
        if (newSprite != null)
            imageComponent.sprite = newSprite;

        buttonComponent.interactable = false;

        // Activar el otro bot�n
        if (botonParaActivar != null)
            botonParaActivar.interactable = true;
    }
}
