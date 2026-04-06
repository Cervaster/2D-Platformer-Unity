using TMPro;
using UnityEngine;

public class SetUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI vida;
    [SerializeField] private TextMeshProUGUI mensaje;
    
    private void Start()
    {
        UIManager.Instance.SetUI( vida , mensaje);
    }
}
