using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float vidas = 100f;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Se ha intentado crear una instancia adicional de GameManager. Solo debe existir una instancia de GameManager en la escena.");
        }
    }
}
