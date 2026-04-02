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
    private float inputH;
    private Animator anim;

    [Header("Sistema de movimiento")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private Transform pies;
    [SerializeField] private float distanciaSuelo;
    [SerializeField] private LayerMask queEsSaltable;
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference attack;
    [SerializeField] private InputActionReference jump;
    private Vector2 moveDirection;


    [Header("Sistema de combate")]
    [SerializeField] private Transform puntoAtaque;
    [SerializeField] private float radioAtaque;
    [SerializeField] private float danhoAtaque;
    [SerializeField] private LayerMask queEsDanhable;

    [Header("Sistema de vidas")]
    [SerializeField] private TextMeshProUGUI vidas;
    private SistemaVidas sistemaVidas;
    private float vidasIniciales;

    [Header("KillZone")]
    [SerializeField] private GameObject killZone;
    private bool isPlayerInKillZone = false; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sistemaVidas = GetComponent<SistemaVidas>(); // Asignar el componente SistemaVidas
        
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        moveDirection = move.action.ReadValue<Vector2>();

        vidas.text = sistemaVidas.Vidas.ToString("0");
        vidasIniciales = sistemaVidas.Vidas;


        if (vidasIniciales <= 20)
        {
            isPlayerInKillZone = true; 
        }
        if (isPlayerInKillZone)
        {
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
            sistemaVidas.RecibirDanho(danhoAtaque);
        }
    }

    private void Jumping(InputAction.CallbackContext obj)
    {
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

        rb.linearVelocity = new Vector2(moveDirection.x * velocidadMovimiento, rb.linearVelocity.y);

        if (moveDirection.x != 0)//hay movimiento
        {
            anim.SetBool("running", true);
            if (moveDirection.x > 0)//movimiento der
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

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.CompareTag("Ground"))
        {
            isPlayerInKillZone = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoAtaque.position, radioAtaque);
    }

    private void OnEnable()
    {
        jump.action.started += Jumping;
        
        attack.action.started += LanzarAtaque;

    }

    private void OnDisable()
    {
        jump.action.started -= Jumping;

        attack.action.started -= LanzarAtaque;

    }
}
