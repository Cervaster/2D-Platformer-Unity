using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private TextMeshProUGUI vidatexto;
    private TextMeshProUGUI mensajeTexto;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }   

    }

    public void ShowDoorMessage(string message)
    {
        if (mensajeTexto == null) return; 

        mensajeTexto.text = message;
        mensajeTexto.gameObject.SetActive(true);

    }

    public void HideDoorMessage()
    {
        if (mensajeTexto == null) return;

        mensajeTexto.gameObject.SetActive(false);
    }


    public void ActualizarVida(float vida)
    {
        if (vidatexto == null) return;
        vidatexto.text = "Vida: " + vida.ToString("F0");
    }

    public void SetUI(TextMeshProUGUI vida, TextMeshProUGUI mensaje)
    {
        vidatexto = vida;
        mensajeTexto = mensaje;

        mensajeTexto.gameObject.SetActive(false);
    }
}
