using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float velocidadPlatform;
    private Vector3 destinoActual;
    private int indiceActual = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destinoActual = points[indiceActual].position;
        StartCoroutine(Patrulla());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Patrulla()
    {
        while (true)
        {
            while (transform.position != destinoActual)
            {
                transform.position = Vector3.MoveTowards(transform.position, destinoActual, velocidadPlatform * Time.deltaTime);
                yield return null;
            }
            DefinirNuevoDestino();
        }
    }
    private void DefinirNuevoDestino()
    {
        indiceActual++;
        if (indiceActual >=points.Length)
        {
            indiceActual = 0;
        }
        destinoActual = points[indiceActual].position;

    }
}
