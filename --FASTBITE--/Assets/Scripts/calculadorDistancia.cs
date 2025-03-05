using UnityEngine;
using TMPro;

public class calculadorDistancia : MonoBehaviour
{
    public TMP_Text distanceText; // Referencia al TextMeshPro para mostrar la distancia
    private Vector3 lastPosition; // �ltima posici�n del jugador
    private float totalDistance;  // Distancia total recorrida

    void Start()
    {
        // Inicializar la �ltima posici�n con la posici�n actual del jugador
        lastPosition = transform.position;
        totalDistance = 0f;

        // Inicializar el texto de la distancia
        if (distanceText != null)
        {
            distanceText.text = "Distancia: 0 m";
        }
    }

    void Update()
    {
        // Calcular la distancia recorrida desde el �ltimo frame
        float distanceThisFrame = Vector3.Distance(transform.position, lastPosition);
        totalDistance += distanceThisFrame;

        // Actualizar la �ltima posici�n
        lastPosition = transform.position;

        // Mostrar la distancia en el texto
        if (distanceText != null)
        {
            distanceText.text = "Distancia: " + totalDistance.ToString("F1") + " m";
        }
    }
}