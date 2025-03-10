using UnityEngine;
using TMPro; // Aseg�rate de incluir este namespace para TextMeshPro

public class TimerController : MonoBehaviour
{
    public TMP_Text timerText;
    private float startTime;
    private bool isRunning = false; // No inicia autom�ticamente

    void Update()
    {
        if (!isRunning) return; // Solo avanza si el temporizador est� activo

        float elapsedTime = Time.time - startTime;
        string minutes = ((int)elapsedTime / 60).ToString("00");
        string seconds = (elapsedTime % 60).ToString("00.0"); // Incluye d�cimas de segundo

        timerText.text = $"{minutes}:{seconds}";
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

    public void ReiniciarTimer()
    {
        isRunning = false;
        startTime = Time.time;
        timerText.text = "00:00.0";
    }

    public float ObtenerTiempo()
    {
        return Time.time - startTime;
    }

}