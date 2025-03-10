using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private bool isMoving = false;
    private InputManager inputManager;
    private Vector3 targetPosition;
    public float lanesDistance = 7f;  // Distancia entre carriles
    public float velocidad = 5f;      // Velocidad hacia adelante
    public float lateralSpeed = 10f;  // Velocidad de movimiento lateral
    public GameObject PanelJuego;

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
        if (!GameManager.Instance.gameStarted) return;

        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

        SelectTargetPosition();
        MoveToTargetPosition();

        if (chocolateText != null)
            chocolateText.text = $"FastBites: {barrasRecogidas}";

        if (energiaSlider != null)
            energiaSlider.value = energia;

        if (energia <= 0)
            GameManager.Instance.EndGame();

        Debug.Log("Energía: " + energia);
        Debug.Log("Chocolatina: " + barrasRecogidas);
    }

    private void SelectTargetPosition()
    {
        if (isMoving) return; // Evita cambios hasta completar el movimiento

        float horizontalMovement = inputManager.HorizontalMovement.ReadValue<float>();
        float x = transform.position.x;

        if (horizontalMovement == -1 && x < rightLimit)
        {
            targetPosition.x = Mathf.Min(x + lanesDistance, rightLimit);
            isMoving = true; // Evita nuevos movimientos hasta que llegue
        }
        else if (horizontalMovement == 1 && x > leftLimit)
        {
            targetPosition.x = Mathf.Max(x - lanesDistance, leftLimit);
            isMoving = true;
        }
    }

    private void MoveToTargetPosition()
    {
        Vector3 newPosition = new Vector3(
        Mathf.MoveTowards(transform.position.x, targetPosition.x, lateralSpeed * Time.deltaTime),
        transform.position.y,
        transform.position.z
        );

        transform.position = newPosition;

        // Si ya llegó al objetivo, permite nuevos movimientos
        if (transform.position.x == targetPosition.x)
        {
            isMoving = false;
        }
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


            if (animator != null)
            {
                animator.SetTrigger("Trip");
            }

            if (audiosourceDisminuirVelocidad != null)
            {
                audiosourceDisminuirVelocidad.PlayOneShot(audiosourceDisminuirVelocidad.clip);
            }
        }
        if (other.CompareTag("meta"))
        {
            GameManager.Instance.EndGame();
            PanelJuego.SetActive(false);
            
        }
    }

    public void ReiniciarPosicion()
    {
        transform.position = targetPosition;
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int ObtenerPuntaje()
    {
        return barrasRecogidas;
    }
}