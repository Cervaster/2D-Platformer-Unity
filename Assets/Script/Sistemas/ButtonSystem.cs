using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSystem : MonoBehaviour
{
    public void Entrar()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Salir()
    {
        SceneManager.LoadScene("Menu");
    }

}
