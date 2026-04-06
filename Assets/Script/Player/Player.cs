using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Sistema de movimiento")]
    private float velocidadMov = 10;
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private Transform pies;
    [SerializeField] private float distanciaSuelo;
    [SerializeField] private LayerMask queEsSaltable;
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference attack;
    [SerializeField] private InputActionReference jump;
    private Vector2 direccionMov;


    [Header("Sistema de combate")]
    [SerializeField] private Transform puntoAtaque;
    [SerializeField] private float radioAtaque;
    [SerializeField] private float danhoAtaque;
    [SerializeField] private LayerMask queEsDanhable;

    [Header("Sistema de vidas")]
    private float vidasIniciales;

    [Header("KillZone")]
    private GameObject killZone;
    private bool isPlayerInKillZone = false; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        killZone = GameObject.Find("KillZone");

    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        direccionMov = move.action.ReadValue<Vector2>();

        UIManager.Instance.ActualizarVida(GameManager.Instance.vidas);
        vidasIniciales = GameManager.Instance.vidas;


        if (vidasIniciales <= 0)
        {
            isPlayerInKillZone = true; 
        }
        if (isPlayerInKillZone)
        {
            GameManager.Instance.vidas = 100f; // Reinicia las vidas del jugador
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena
            isPlayerInKillZone = false; // Evita recargas múltiples
        }
    }

    private void LanzarAtaque(InputAction.CallbackContext obj)
    {

        anim.SetTrigger("attack");

    }

    //se ejecuta desde evento de animacion
    private void Ataque()
    {
        Collider2D[] collidersTocados = Physics2D.OverlapCircleAll(puntoAtaque.position, radioAtaque, queEsDanhable);
        foreach (Collider2D item in collidersTocados)
        {
            SistemaVidas sistemaVidas = item.gameObject.GetComponent<SistemaVidas>();
            if (sistemaVidas != null)
            {
                sistemaVidas.RecibirDanho(danhoAtaque);
            }
            
        }
    }

    private void Saltar(InputAction.CallbackContext obj)
    {
        transform.SetParent(null);

        if (EstoyEnSuelo())
        {
            rb.AddForce(new Vector2(0f,fuerzaSalto), ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }

    private bool EstoyEnSuelo()
    {
        return Physics2D.Raycast(pies.position, Vector3.down, distanciaSuelo, queEsSaltable); ;
    }

    private void Movimiento()
    {

        rb.linearVelocity = new Vector2(direccionMov.x * velocidadMov, rb.linearVelocity.y);

        if (direccionMov.x != 0)//si hay movimiento
        {
            anim.SetBool("running", true);
            if (direccionMov.x > 0)//movimiento der
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else//movimiento izq
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            anim.SetBool("running", false);
        }
    }

    //al entrar a la zona de muerte, se reinicia la escena
    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.CompareTag("Ground"))
        {
            isPlayerInKillZone = true;
        }
    }

    //se dibuja un gizmo para visualizar el area de ataque en la escena
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoAtaque.position, radioAtaque);
    }

    //sistema de botones de input
    private void OnEnable()
    {
        jump.action.started += Saltar;
        
        attack.action.started += LanzarAtaque;

    }

    private void OnDisable()
    {
        jump.action.started -= Saltar;

        attack.action.started -= LanzarAtaque;

    }

    private void OnCollisionEnter2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(elOtro.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }
}
