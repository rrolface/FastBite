using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputManager inputManager;
    private Vector3 targetPosition;
    public float lanesDistance = 7f;  // Distancia entre carriles
    public float velocidad = 5f;      // Velocidad hacia adelante
    public float lateralSpeed = 10f;  // Velocidad de movimiento lateral

    // L�mites de los carriles
    public float leftLimit = 7.89f;    // L�mite izquierdo
    public float rightLimit = 21.89f;  // L�mite derecho

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        targetPosition = transform.position; // Inicializa la posici�n objetivo
    }

    void Update()
    {
        // Movimiento hacia adelante (siempre activo)
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

        // Selecci�n de carril
        SelectTargetPosition();

        // Movimiento lateral suave hacia el carril objetivo
        MoveToTargetPosition();
    }

    private void SelectTargetPosition()
    {
        float horizontalMovement = inputManager.HorizontalMovement.ReadValue<float>();
        float x = transform.position.x;

        // Movimiento a la derecha (solo si no supera el l�mite derecho)
        if (horizontalMovement == 1 && x < rightLimit)
        {
            targetPosition.x = Mathf.Min(x + lanesDistance, rightLimit); // Asegura que no se pase del l�mite derecho
        }
        // Movimiento a la izquierda (solo si no supera el l�mite izquierdo)
        else if (horizontalMovement == -1 && x > leftLimit)
        {
            targetPosition.x = Mathf.Max(x - lanesDistance, leftLimit); // Asegura que no se pase del l�mite izquierdo
        }
    }

    private void MoveToTargetPosition()
    {
        // Mueve suavemente hacia la posici�n objetivo en el eje X
        Vector3 newPosition = new Vector3(
            Mathf.MoveTowards(transform.position.x, targetPosition.x, lateralSpeed * Time.deltaTime),
            transform.position.y,
            transform.position.z
        );

        transform.position = newPosition;
    }
}