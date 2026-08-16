using Unity.VisualScripting;
using UnityEngine;

public class LifeSystem: MonoBehaviour
{
    [SerializeField] private float vidas;
    [SerializeField] GameObject explosion;
    [SerializeField] private bool isPlayer;

    public float Vidas 
    {
        get
        {
            return isPlayer ? GameManager.Instance.vidas : vidas;
        }
        set
        {
            if (isPlayer)
            {
                GameManager.Instance.vidas = value;

            }
            else
            { 
                vidas = value; 
            }
                
        }
    }

    public void RecibirDanho(float danhorecibido)
    {
        Vidas -= danhorecibido;
        if (Vidas <= 0)
        {
            if (isPlayer)
            {
                Debug.Log("El jugador ha muerto. Reiniciando el nivel...");
            }
            else
            {
                Destroy(this.gameObject);
                Instantiate(explosion, this.transform.position, Quaternion.identity);
            }
        }

    }

}