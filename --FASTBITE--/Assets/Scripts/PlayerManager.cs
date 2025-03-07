using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public float energia = 0f;  // Energía del jugador (comienza en 0)
    private float maxEnergiaActual = 0f; // Energía máxima actual
    public TMP_Text chocolateText;

    public Slider energiaSlider;

    // Límites de los carriles
    public float leftLimit = 7.89f;    // Límite izquierdo
    public float rightLimit = 21.89f;  // Límite derecho

    // Audio
    public AudioSource audiosourceDisminuirVelocidad;
    public AudioSource audiosourceChocolatina;

    // Animator
    private Animator animator;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        targetPosition = transform.position; // Inicializa la posición objetivo

        // Obtén el componente Animator
        animator = GetComponent<Animator>();

        if (chocolateText != null)
        {
            chocolateText.text = "Barras Recogidas 0";
        }

        // Inicializa la barra de energía
        if (energiaSlider != null)
        {
            energiaSlider.minValue = 0;
            energiaSlider.maxValue = 1; // Valor inicial máximo (puede aumentar)
            energiaSlider.value = energia; // Comienza en 0
        }

        // Asegúrate de que los AudioSources estén asignados
        if (audiosourceChocolatina == null)
        {
            audiosourceChocolatina = GetComponent<AudioSource>();
        }
        if (audiosourceDisminuirVelocidad == null && GetComponents<AudioSource>().Length > 1)
        {
            audiosourceDisminuirVelocidad = GetComponents<AudioSource>()[1];
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

        // Actualiza el texto de las barras recogidas
        if (chocolateText != null)
        {
            chocolateText.text = "Barras Recogidas: " + barrasRecogidas.ToString();
        }

        // Actualiza la barra de energía
        if (energiaSlider != null)
        {
            energiaSlider.value = energia;
        }

        // Verifica si la energía llegó a 0
        if (energia <= 0)
        {
            GameOver();
        }

        Debug.Log("Energía: " + energia);
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

            // Actualiza la energía máxima si es necesario
            if (energia > maxEnergiaActual)
            {
                maxEnergiaActual = energia;
                if (energiaSlider != null)
                {
                    energiaSlider.maxValue = maxEnergiaActual;
                }
            }

            if (audiosourceChocolatina != null)
            {
                audiosourceChocolatina.PlayOneShot(audiosourceChocolatina.clip);
            }
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Obstaculo"))
        {
            energia -= velChocolatina;
            velocidad -= velChocolatina;

            // Asegúrate de que la energía no sea menor que 0
            if (energia < 0)
            {
                energia = 0;
            }

            // Reproduce la animación de tropezar
            if (animator != null)
            {
                animator.SetTrigger("Trip");
            }

            if (energia <= 0)
            {
                Debug.Log("PERDISTE EL JUEGOOOOOOOOOO");
                GameOver();
            }

            if (audiosourceDisminuirVelocidad != null)
            {
                audiosourceDisminuirVelocidad.PlayOneShot(audiosourceDisminuirVelocidad.clip);
            }
        }
    }

    void GameOver()
    {
        Debug.Log("Game over llamado");

        float distanciaFinal = transform.position.z;
        float tiempoFinal = Time.timeSinceLevelLoad;

        Debug.Log("Barras recogidas: " + barrasRecogidas);
        Debug.Log("Distancia final: " + distanciaFinal);
        Debug.Log("Tiempo final: " + tiempoFinal);

        if (RankingManager.Instance != null)
        {
            RankingManager.Instance.GuardarRanking(barrasRecogidas, distanciaFinal, tiempoFinal);
            RankingManager.Instance.MostrarRanking();
        }
        else
        {
            Debug.Log("RankingManager.Instance es nulo");
        }

        ReiniciarJuego();
    }

    void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}