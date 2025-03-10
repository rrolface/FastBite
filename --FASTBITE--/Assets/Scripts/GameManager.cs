using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool gameStarted = false;

    public TMP_InputField nameInput;
    public GameObject startPanel;
    public GameObject rankingPanel;
    public TMP_Text rankingText; // Se asume que esta es la parte donde se mostrará el ranking
    public GameObject panelJuego;

    private string playerName;
    private float totalTime;
    private int totalChocolates;

    private string filePath; // Ruta del archivo de ranking

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        filePath = Application.dataPath + "/Ranking.txt"; // Ruta del archivo en la carpeta Assets
    }

    public void StartGame()
    {
        playerName = nameInput.text;
        gameStarted = true;
        startPanel.SetActive(false);
        panelJuego.SetActive(true);

        TimerController timerController = FindFirstObjectByType<TimerController>();
        if (timerController != null)
        {
            timerController.StartTimer();
        }
    }

    public void EndGame()
    {
        gameStarted = false;

        PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();
        TimerController timerController = FindFirstObjectByType<TimerController>();

        if (playerManager != null)
            totalChocolates = playerManager.ObtenerPuntaje();

        if (timerController != null)
        {
            timerController.StopTimer();
            totalTime = timerController.ObtenerTiempo();
        }

        // Guardar los datos en el archivo de ranking
        GuardarEnArchivo(playerName, totalChocolates, totalTime);

        // Mostrar el ranking
        MostrarRanking();
        rankingPanel.SetActive(true);
    }

    private void GuardarEnArchivo(string nombre, int chocolates, float tiempo)
    {
        string datos = $"{nombre}-{chocolates}-{tiempo:F2}";
        File.AppendAllText(filePath, datos + "\n"); // Agregar nueva línea al archivo
    }

    private void MostrarRanking()
    {
        if (File.Exists(filePath))
        {
            string[] lineas = File.ReadAllLines(filePath);
            string rankingFinal = "Nombre    |    Chocolates    |    Tiempo\n";
            rankingFinal += "-----------------------------------------------\n";

            foreach (string linea in lineas)
            {
                string[] datos = linea.Split('-');
                if (datos.Length == 3)
                {
                    string nombre = datos[0];
                    string chocolates = datos[1];
                    string tiempo = datos[2];

                    rankingFinal += $"{nombre}     {chocolates}      {tiempo}\n";
                }
            }

            rankingText.text = rankingFinal;
        }
        else
        {
            rankingText.text = "No hay datos aún.";
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
