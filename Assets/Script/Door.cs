using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private bool isPlayerInDoor = false;
    [SerializeField] private InputActionReference interac;


    // Update is called once per frame
 
    private void Interaction(InputAction.CallbackContext obj)
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            if (isPlayerInDoor == true)
            {
                SceneManager.LoadScene("Level2");
            }
        }

        if (SceneManager.GetActiveScene().name == "Level2")
        {
            if (isPlayerInDoor == true)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
    public void Entrar()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single); 
    }

    public void Salir()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.CompareTag("PlayerHitBox"))
        {
            isPlayerInDoor = true;
            UIManager.Instance.ShowDoorMessage("Presiona E para entrar al siguiente nivel");
        }
        else
        {
            isPlayerInDoor = false;
            UIManager.Instance.HideDoorMessage();
        }
    }

    private void OnEnable()
    {
        interac.action.started += Interaction;
    }

    private void OnDisable()
    {
        interac.action.started -= Interaction;
    }
}
