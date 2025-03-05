using UnityEngine;
using TMPro; // Asegúrate de incluir este namespace para TextMeshPro

public class TimerController : MonoBehaviour
{
    public TMP_Text timerText; // Referencia al TextMeshProUGUI que mostrará el temporizador
    private float startTime; // Tiempo en el que comienza el temporizador
    private bool isRunning; // Indica si el temporizador está en marcha

    void Start()
    {
        // Iniciar el temporizador al comenzar la escena
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            // Calcular el tiempo transcurrido
            float elapsedTime = Time.time - startTime;

            // Convertir el tiempo a minutos y segundos
            string minutes = ((int)elapsedTime / 60).ToString("00");
            string seconds = (elapsedTime % 60).ToString("00");

            // Actualizar el texto del temporizador
            timerText.text = minutes + ":" + seconds;
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        startTime = Time.time;
        timerText.text = "00:00";
    }
}