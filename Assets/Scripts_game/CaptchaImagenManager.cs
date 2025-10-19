using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CaptchaImagenManager : MonoBehaviour
{
    [System.Serializable]
    public class CategoriaImagenes
    {
        public string nombreCategoria;
        public Sprite[] imagenes; // 9 sprites por categoría
    }

    [Header("Configuración")]
    public List<CategoriaImagenes> categorias;
    public TMP_Text textoInstruccion;
    public List<Image> botonesGrilla; // los 9 botones (Image) en la grilla

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip selectAudio;
    public AudioClip deselectAudio;
    public AudioClip submitAudio;
    public AudioClip errorAudio;

    private string categoriaObjetivo;
    private HashSet<int> indicesCorrectos = new HashSet<int>();
    private HashSet<int> seleccionJugador = new HashSet<int>();

    public GameObject errorTexto; 

    public CaptchaFlowManager flowManager; 

    void Start()
    {
        GenerarNuevoCaptcha();
    }

    public void GenerarNuevoCaptcha()
    {
        // Elegir una categoría al azar como objetivo
        int categoriaIndex = Random.Range(0, categorias.Count);
        categoriaObjetivo = categorias[categoriaIndex].nombreCategoria;
        textoInstruccion.text = "Select all the: " + categoriaObjetivo;

        indicesCorrectos.Clear();
        seleccionJugador.Clear();

        // Mezclar sprites de ambas categorías
        List<(Sprite sprite, string categoria)> spritesMezclados = new List<(Sprite, string)>();

        foreach (var categoria in categorias)
        {
            foreach (var sprite in categoria.imagenes)
            {
                spritesMezclados.Add((sprite, categoria.nombreCategoria));
            }
        }

        // Mezclar y tomar 9 imágenes
        spritesMezclados.Shuffle(); // usa extensión abajo
        var seleccionadas = spritesMezclados.GetRange(0, botonesGrilla.Count);

        for (int i = 0; i < botonesGrilla.Count; i++)
        {
            botonesGrilla[i].sprite = seleccionadas[i].sprite;

            // Guardar cuáles son las correctas
            if (seleccionadas[i].categoria == categoriaObjetivo)
            {
                indicesCorrectos.Add(i);
            }

            // Resetear selección, evita que salgan marcados los botones seleccionadas en una iteración anterior
            botonesGrilla[i].color = Color.white;

            int indexCapturado = i; // evitar bug de closure
            botonesGrilla[i].GetComponent<Button>().onClick.RemoveAllListeners();
            botonesGrilla[i].GetComponent<Button>().onClick.AddListener(() => AlPresionarImagen(indexCapturado));
        }
    }

    public void AlPresionarImagen(int index) // encargado de "seleccionar" o "deseleccionar" una opción
    {
        if (seleccionJugador.Contains(index))
        {
            seleccionJugador.Remove(index);
            botonesGrilla[index].color = Color.white; // Deseleccionar
            audioSource.PlayOneShot(deselectAudio);
        }
        else
        {
            seleccionJugador.Add(index);
            botonesGrilla[index].color = new Color(0.5f, 0.5f, 0.5f, 1f); // Oscurecer
            audioSource.PlayOneShot(selectAudio);
        }
    }

    public void VerificarCaptcha() // encargado de ver si las opciones seleccionadas son correcatas
    {
        bool todoCorrecto = true;

        // Verifica si hay alguna imagen mal seleccionada
        foreach (int index in seleccionJugador)
        {
            if (!indicesCorrectos.Contains(index))
            {
                todoCorrecto = false;
                break;
            }
        }

        // También verifica si el jugador dejó alguna correcta sin seleccionar
        if (todoCorrecto && seleccionJugador.Count != indicesCorrectos.Count)
        {
            todoCorrecto = false;        }

        if (todoCorrecto)
        {
            audioSource.PlayOneShot(submitAudio);
            errorTexto.SetActive(false);
            flowManager.OnCaptchaResuelto();
        }
        else
        {
            audioSource.PlayOneShot(errorAudio);
            errorTexto.SetActive(true);

            // Deseleccionar todo visualmente y lógicamente
            foreach (int i in seleccionJugador)
            {
                botonesGrilla[i].color = Color.white;
            }
            seleccionJugador.Clear();
        }
    }


    public void ReiniciarCaptchaImagen()
    {
        GenerarNuevoCaptcha();
    }


}
public static class ExtensionMetodos // para el shuffle
{
    public static void Shuffle<T>(this IList<T> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lista[i], lista[j]) = (lista[j], lista[i]);
        }
    }
}
