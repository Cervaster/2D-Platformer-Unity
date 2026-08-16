using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private bool isPlayerInDoor;
    [SerializeField] private InputActionReference interac;


 
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

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        
        if (elOtro.CompareTag("PlayerHitBox"))
        {
            isPlayerInDoor = true;
            UIManager.Instance.ShowDoorMessage("Presiona E para entrar al siguiente nivel");
            Debug.Log("El jugador ha entrado en la zona de la puerta.");
        }
        
    }
    private void OnTriggerExit2D(Collider2D elOtro)
    {
        if (elOtro.CompareTag("PlayerHitBox"))
        {
            isPlayerInDoor = false;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.HideDoorMessage();
            }      
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
