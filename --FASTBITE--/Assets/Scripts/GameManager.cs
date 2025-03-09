using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool gameStarted = false;

    public TMP_InputField nameInput;
    public GameObject startPanel;
    public GameObject rankingPanel;
    public TMP_Text rankingText;
    public GameObject panelJuego;

    private string playerName;
    private float totalTime;
    private float totalDistance;
    private int totalChocolates;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartGame()
    {
        playerName = nameInput.text;
        gameStarted = true;
        startPanel.SetActive(false);
        panelJuego.SetActive(true);
    }

    public void EndGame()
    {
        gameStarted = false;

        // Obtener referencias a los scripts en la escena
        PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();
        calculadorDistancia distanceCalculator = FindFirstObjectByType<calculadorDistancia>();
        TimerController timerController = FindFirstObjectByType<TimerController>();

        if (playerManager != null)
            totalChocolates = playerManager.ObtenerPuntaje();

        if (distanceCalculator != null)
            totalDistance = distanceCalculator.ObtenerDistancia();

        if (timerController != null)
            totalTime = timerController.ObtenerTiempo();

        // Mostrar el ranking
        rankingPanel.SetActive(true);
        rankingText.text += $"\n{playerName}: {totalChocolates} chocolates, {totalDistance:F1} m, {totalTime:F1} s";
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateStats(float time, float distance, int chocolates)
    {
        totalTime = time;
        totalDistance = distance;
        totalChocolates = chocolates;
    }
}
