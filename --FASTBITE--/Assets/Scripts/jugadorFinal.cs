using UnityEngine;

public class jugadorFinal : MonoBehaviour
{
    public int barrasRecogidas = 0;
    public float distanciaRecorrida = 0;
    public float tiempoJugado = 0;
    public float energia = 5f;
    private float velChocolatina = 0.6f;
    private bool juegoTerminado = false;

    private void Update()
    {
        if (juegoTerminado) return;

        tiempoJugado += Time.deltaTime;
        distanciaRecorrida += Time.deltaTime * 5; // Simulación de avance

        if (energia <= 0)
        {
            TerminarJuego();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("meta"))
        {
            TerminarJuego();
        }

        if (other.CompareTag("Obstaculo"))
        {
            energia -= velChocolatina;

            if (energia <= 0)
            {
                energia = 0;
                TerminarJuego();
            }

            
        }
    }

    private void TerminarJuego()
    {
        juegoTerminado = true;
        GameManager.Instance.FinalizarJuego(barrasRecogidas, distanciaRecorrida, tiempoJugado);
    }
}
