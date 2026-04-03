using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI vidatexto;
    [SerializeField] private TextMeshProUGUI mensajeTexto;

    private void EnsureMensajeTexto()
    {
        if (mensajeTexto != null) return;

        // Intentamos asignarlo desde hijos primero (incluyendo inactivos)
        mensajeTexto = GetComponentInChildren<TextMeshProUGUI>(true);
        if (mensajeTexto != null && mensajeTexto == vidatexto)
        {
            // si encontró el mismo texto de vida, buscar otro
            var all = FindObjectsOfType<TextMeshProUGUI>(true);
            foreach (var t in all)
            {
                if (t != vidatexto)
                {
                    mensajeTexto = t;
                    break;
                }
            }
        }

        if (mensajeTexto == null)
        {
            var all = FindObjectsOfType<TextMeshProUGUI>(true);
            foreach (var t in all)
            {
                var nameLow = t.name.ToLower();
                if (nameLow.Contains("mensaje") || nameLow.Contains("door") || nameLow.Contains("press") || nameLow.Contains("presiona"))
                {
                    mensajeTexto = t;
                    break;
                }
            }
        }

        if (mensajeTexto == null)
        {
            // fallback al primero distinto de vidatexto
            var all = FindObjectsOfType<TextMeshProUGUI>(true);
            foreach (var t in all)
            {
                if (t != vidatexto)
                {
                    mensajeTexto = t;
                    break;
                }
            }
        }

        if (mensajeTexto != null)
        {
            mensajeTexto.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Preserve the whole root GameObject so child UI elements (como el TextMeshProUGUI) no se pierdan al cargar otra escena
            DontDestroyOnLoad(gameObject.transform.root.gameObject);
            Debug.Log($"UIManager creado y marcado como DontDestroyOnLoad en escena: {SceneManager.GetActiveScene().name}");

            // Si la referencia al TextMeshPro no fue asignada en el inspector, intentamos encontrarla en los hijos
            if (vidatexto == null)
            {
                vidatexto = GetComponentInChildren<TextMeshProUGUI>();
                if (vidatexto == null)
                {
                    Debug.LogWarning("UIManager: 'vidatexto' no está asignado y no se encontró un TextMeshProUGUI en los hijos.");
                }
            }
            // Intentamos también asegurar el texto de mensaje de puerta
            EnsureMensajeTexto();
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Se ha intentado crear una instancia adicional de UIManager. Solo debe existir una instancia de UIManager en la escena.");
        }

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Si el TextMeshPro quedó nulo al cargar la escena, intentamos re-asignarlo desde la nueva escena
        if (vidatexto == null)
        {
            var encontrado = FindAnyObjectByType<TextMeshProUGUI>();
            if (encontrado != null)
            {
                vidatexto = encontrado;
                Debug.Log($"UIManager: 'vidatexto' reasignado desde la escena {scene.name}.");
            }
            else
            {
                Debug.LogWarning($"UIManager: no se encontró TextMeshProUGUI en la escena {scene.name} para reasignar 'vidatexto'.");
            }
        }
    }

    public void ActualizarVida(float vida)
    {
        vidatexto.text = vida.ToString("0");
    }

    public void ShowDoorMessage(string texto)
    {
        EnsureMensajeTexto();
        if (mensajeTexto != null)
        {
            mensajeTexto.text = texto;
            mensajeTexto.gameObject.SetActive(true);
        }
    }

    public void HideDoorMessage()
    {
        if (mensajeTexto != null)
        {
            mensajeTexto.gameObject.SetActive(false);
        }
    }


}
