using UnityEngine;

public class BusSound : MonoBehaviour
{
    public AudioSource busSound; // El AudioSource donde está el sonido del bus
    public float intervaloReproduccion = 10f; // Intervalo de tiempo en segundos entre cada reproducción
    public float tiempoesperaparasonido = 5f;

    void Start()
    {
        // Inicia la corrutina para reproducir el sonido cada cierto tiempo
        Invoke("ComenzarSonido", tiempoesperaparasonido);
    } 

    void ComenzarSonido()
    {
        InvokeRepeating("ReproducirSonido", 0f, intervaloReproduccion);
    }

    void ReproducirSonido()
    {
        // Reproducir el sonido del pito del bus cada vez que se llama a esta función
        if (busSound != null)
        {
            busSound.PlayOneShot(busSound.clip); // Reproduce el sonido solo una vez
        }
    }
}
