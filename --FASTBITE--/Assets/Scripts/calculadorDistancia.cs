using UnityEngine;
using TMPro;

public class calculadorDistancia : MonoBehaviour
{
    public TMP_Text distanceText; // Referencia al TextMeshPro para mostrar la distancia
    private Vector3 lastPosition; // Última posición del jugador
    private float totalDistance;  // Distancia total recorrida

    void Start()
    {
        // Inicializar la última posición con la posición actual del jugador
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
        // Calcular la distancia recorrida desde el último frame
        float distanceThisFrame = Vector3.Distance(transform.position, lastPosition);
        totalDistance += distanceThisFrame;

        // Actualizar la última posición
        lastPosition = transform.position;

        // Mostrar la distancia en el texto
        if (distanceText != null)
        {
            distanceText.text = "Distancia: " + totalDistance.ToString("F1") + " m";
        }
    }
}