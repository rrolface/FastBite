using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private bool isMoving = false; // Variable para evitar mas de un movimiento
    private InputManager inputManager;
    private Vector3 targetPosition;

    public float lanesDistance = 7f;  // Distancia entre carriles
    public float velocidad = 5f;
    public float lateralSpeed = 10f;


    public GameObject PanelJuego;

    // Variables de Mencanicas del juego

    private float velChocolatina = 0.5f; 
    private int barrasRecogidas = 0; 
    private int tropezones = 0;
    public TMP_Text chocolateText;

    // Variables Mecanica de barra de enrgia
    public float energia = 0.5f; 
    private float maxEnergiaActual = 1f; 
    public Slider energiaSlider;

    // Límites de los carriles
    public float leftLimit = 7.89f;   
    public float rightLimit = 21.89f;  

    // Audio
    public AudioSource audiosourceDisminuirVelocidad;
    public AudioSource audiosourceChocolatina;
    public AudioSource audioBus;

    // Animator
    public Animator animator;


    //Variable para activar panel de perdida en el GameManager
    public bool PerdioPorBus = false;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        targetPosition = transform.position; // Inicializa la posición objetivo


        animator = GetComponent<Animator>();

        if (chocolateText != null)
        {
            chocolateText.text = "Barras Recogidas 0";
        }
        energia = 0.5f; // Comienza con energía en el medio


        // Inicializa la barra de energía
        if (energiaSlider != null)
        {
            energiaSlider.minValue = 0;
            energiaSlider.maxValue = maxEnergiaActual; // Valor inicial máximo
            energiaSlider.value = energia; // Comienza en 0.5
        }


        
        if (audiosourceChocolatina == null)
        {
            audiosourceChocolatina = GetComponent<AudioSource>();
        }
        if (audiosourceDisminuirVelocidad == null && GetComponents<AudioSource>().Length > 1)
        {
            audiosourceDisminuirVelocidad = GetComponents<AudioSource>()[1];
        }

        if (PerdioPorBus)
        {
           
            audioBus.PlayOneShot(audioBus.clip);
        }
    }

    void Update()
    {
        if (!GameManager.Instance.gameStarted) return;

        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

        SelectTargetPosition();
        MoveToTargetPosition();


        // Textos en el panel de Juego

        if (chocolateText != null)
            chocolateText.text = $"FastBites: {barrasRecogidas}";

        if (energiaSlider != null)
            energiaSlider.value = energia;

        if (energia <= 0)
            GameManager.Instance.EndGame();


        //Visualizar en Consolar

        Debug.Log("Energía: " + energia);
        Debug.Log("Chocolatina: " + barrasRecogidas);
        Debug.Log("Velocidad: " + velocidad);
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
            isMoving = true; // Evita nuevos movimientos hasta que llegue
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
        // Que sucede si agarra barra de chocolate
        if (other.CompareTag("BarraChocolate"))
        {
            barrasRecogidas++;
            energia += velChocolatina; 
            velocidad += velChocolatina; 

            // Limitar el aumento de energía al máximo
            if (energia > maxEnergiaActual)
            {
                energia = maxEnergiaActual; 
            }

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


        // Que sucede al chocar con un obstaculo
        if (other.CompareTag("Obstaculo"))
        {
            
            energia -= 0.2f;             // Barra de energia decrece
            velocidad -= 0.2f; 
            tropezones++;

            
            if (energia < 0)
            {
                energia = 0; 
            }

            if (animator != null)
            {
                animator.SetTrigger("Trip");
            }

            if (audiosourceDisminuirVelocidad != null)
            {
                audiosourceDisminuirVelocidad.PlayOneShot(audiosourceDisminuirVelocidad.clip);
            }
        }

        // Que sucede si llega a Meta
        if (other.CompareTag("meta"))
        {
            GameManager.Instance.EndGame();
            PanelJuego.SetActive(false);
        }

        // Que sucede si choca con un bus
        if (other.CompareTag("Bus"))
        {
            if(animator != null)
            {
                animator.SetTrigger("Caida");
            }
            if (audiosourceDisminuirVelocidad != null)
            {
                audiosourceDisminuirVelocidad.PlayOneShot(audiosourceDisminuirVelocidad.clip);
            }
            PerdioPorBus = true;
            GameManager.Instance.EndGame();
        }
    }

    
    // puntake total para que el gameManager llame

    public int ObtenerPuntaje()
    {
        return barrasRecogidas;
    }

    // Tropezones para que el gameManager llame

    public int Tropezones()
    {
        return tropezones;
    }
}
