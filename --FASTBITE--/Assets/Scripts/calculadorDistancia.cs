using UnityEngine;
using TMPro;

public class calculadorDistancia : MonoBehaviour
{
    public TMP_Text distanceText;
    private Vector3 lastPosition;
    private float totalDistance;

    void Start()
    {
        ReiniciarDistancia();
    }

    void Update()
    {
        if (!GameManager.Instance.gameStarted) return;

        float distanceThisFrame = Vector3.Distance(transform.position, lastPosition);
        totalDistance += distanceThisFrame;
        lastPosition = transform.position;

        if (distanceText != null)
        {
            distanceText.text = $"Distancia: {totalDistance:F1} m";
        }
    }

    public void ReiniciarDistancia()
    {
        totalDistance = 0f;
        lastPosition = transform.position; // Asegura que la posición inicial sea correcta

        if (distanceText != null)
        {
            distanceText.text = "Distancia: 0 m";
        }
    }

    public float ObtenerDistancia()
    {
        return totalDistance;
    }
}