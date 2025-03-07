using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputManager inputManager;
    private Vector3 targetPosition;
    public float lanesDistance = 7f;  // Distancia entre carriles
    public float velocidad = 5f;      // Velocidad hacia adelante
    public float lateralSpeed = 10f;  // Velocidad de movimiento lateral

    public float chocolatina = 1f;
    private float velChocolatina = 0.6f;

    private int barrasRecogidas = 0;  // Contador de barras recogidas
    public float energia = 5f;  // Energía del jugador
    public TMP_Text chocolateText;


    // Límites de los carriles
    public float leftLimit = 7.89f;    // Límite izquierdo
    public float rightLimit = 21.89f;  // Límite derecho

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        targetPosition = transform.position; // Inicializa la posición objetivo

        if(chocolateText != null)
        {
            chocolateText.text = "Barras Recogidas 0";
        }

        
    }

    void Update()
    {
        // Movimiento hacia adelante (siempre activo)
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

        // Selección de carril
        SelectTargetPosition();

        // Movimiento lateral suave hacia el carril objetivo
        MoveToTargetPosition();

        if (chocolateText != null)
        {
            chocolateText.text = "Barras Recogidas: " + barrasRecogidas.ToString();
        }

        Debug.Log("energia: " + energia);
        Debug.Log("Chocolatina: " + chocolatina);

    }

    private void SelectTargetPosition()
    {
        float horizontalMovement = inputManager.HorizontalMovement.ReadValue<float>();
        float x = transform.position.x;

        // Movimiento a la derecha (solo si no supera el límite derecho)
        if (horizontalMovement == -1 && x < rightLimit)
        {
            targetPosition.x = Mathf.Min(x + lanesDistance, rightLimit); // Asegura que no se pase del límite derecho
        }
        // Movimiento a la izquierda (solo si no supera el límite izquierdo)
        else if (horizontalMovement == 1 && x > leftLimit)
        {
            targetPosition.x = Mathf.Max(x - lanesDistance, leftLimit); // Asegura que no se pase del límite izquierdo
        }
    }

    private void MoveToTargetPosition()
    {
        // Mueve suavemente hacia la posición objetivo en el eje X
        Vector3 newPosition = new Vector3(
            Mathf.MoveTowards(transform.position.x, targetPosition.x, lateralSpeed * Time.deltaTime),
            transform.position.y,
            transform.position.z
        );

        transform.position = newPosition;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BarraChocolate"))
        {
            barrasRecogidas++;
            energia += velChocolatina;
            velocidad += velChocolatina;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Obstaculo"))
        {
            energia -= velChocolatina;
            velocidad -= velChocolatina;
            if (energia <= 0)
            {
                GameOver();
            }
        }
    }
    void GameOver()
    {
        float distanciaFinal = transform.position.z;
        float tiempoFinal = Time.timeSinceLevelLoad;

        RankingManager.Instance.GuardarRanking(barrasRecogidas, distanciaFinal, tiempoFinal);
        RankingManager.Instance.MostrarRanking();

        Debug.Log("Juego Terminado. Barras Recogidas: " + barrasRecogidas);

    }
}