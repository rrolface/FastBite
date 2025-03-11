using UnityEngine;

public class BusSound : MonoBehaviour
{
    public AudioSource busSound; 
    public float intervaloReproduccion = 10f; 
    public float tiempoesperaparasonido = 8f;

    void Start()
    {
        
        Invoke("ComenzarSonido", tiempoesperaparasonido);
    } 

    void ComenzarSonido()
    {
        InvokeRepeating("ReproducirSonido", 0f, intervaloReproduccion);
    }

    void ReproducirSonido()
    {
        
        if (busSound != null)
        {
            busSound.PlayOneShot(busSound.clip); 
        }
    }
}
