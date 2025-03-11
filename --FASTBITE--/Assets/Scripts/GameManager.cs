using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool gameStarted = false;

    //Paneles y texto de paneles
    public TMP_InputField nameInput;
    public GameObject startPanel;
    public GameObject rankingPanel;
    public GameObject PanelGameOver;
    public TMP_Text rankingText;
    public GameObject panelJuego;

    // Datos a guardar y Archivo
    private string playerName;
    private float totalTime;
    private int totalChocolates;
    private int tropezones;
    private string filePath;
   
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        filePath = Application.dataPath + "/Ranking.txt";    // Ruta del archivo en la carpeta Assets
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
        // Juego Termino, busca las variables (los objetos que tengan dicho codigo)
        gameStarted = false;

        PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();
        TimerController timerController = FindFirstObjectByType<TimerController>();

        if (playerManager != null)
        {
            totalChocolates = playerManager.ObtenerPuntaje();
            tropezones = playerManager.Tropezones();
        }
            

        if (timerController != null)
        {
            timerController.StopTimer();
            totalTime = timerController.ObtenerTiempo();
        }

        // En caso de que el jugador se quedo sin Energia ó choco con el bus

        if (playerManager.energia <= 0 || playerManager.PerdioPorBus)
        {
            if(playerManager.animator != null)
            {
                playerManager.animator.SetTrigger("Caida");
            }
            if (playerManager.audiosourceDisminuirVelocidad != null)
            {
                playerManager.audiosourceDisminuirVelocidad.PlayOneShot(playerManager.audiosourceDisminuirVelocidad.clip);  // Reproducir sonido
            }
            Debug.Log("Partida no guardada, el jugador perdió.");
            PanelGameOver.SetActive(true);
            return;
        }



        GuardarEnArchivo(playerName, totalTime, totalChocolates, tropezones);
        MostrarRanking();
        rankingPanel.SetActive(true);
    }

    // Guarda los datos en el archivo txt

    private void GuardarEnArchivo(string nombre, float tiempo, int chocolates, int tropezones)
    {
        string datos = $"{nombre}-{tiempo:F2}-{chocolates}-{tropezones}";
        File.AppendAllText(filePath, datos + "\n"); 
    }

    // Extrae los datos del archivo txt y los muestra el Ranking (Solo los 10 primeros)
    // Prioriza Tiempo, luego barras y luego tropiezos

    private void MostrarRanking()
    {
        if (File.Exists(filePath))
        {
            string[] lineas = File.ReadAllLines(filePath);
            List<(string nombre, float tiempo, int chocolates, int tropezones)> rankingList = new List<(string, float, int, int)>();

            foreach (string linea in lineas)
            {
                string[] datos = linea.Split('-');
                if (datos.Length == 4)
                {
                    string nombre = datos[0];
                    float tiempo = float.Parse(datos[1]);
                    int chocolates = int.Parse(datos[2]);
                    int tropezones = int.Parse(datos[3]);

                    rankingList.Add((nombre, tiempo, chocolates, tropezones));
                }
            }

            rankingList = rankingList
            .OrderBy(x => x.tiempo)                              // Menor tiempo primero
            .ThenByDescending(x => x.chocolates)                 // Mayor chocolates después
            .ThenBy(x => x.tropezones)                           // Menos tropiezos al final
            .Take(10)                     
            .ToList();



            // Construcción del ranking para mostrar


            StringBuilder rankingFinal = new StringBuilder();
            rankingFinal.AppendLine("Nombre | Tiempo | Chocolates | Choques");
            rankingFinal.AppendLine("-----------------------------------------------");

            foreach (var jugador in rankingList)
            {
                rankingFinal.AppendLine($"{jugador.nombre}      {jugador.tiempo:F2}     {jugador.chocolates}     {jugador.tropezones}");
            }

            rankingText.text = rankingFinal.ToString();
        }
        else
        {
            rankingText.text = "No hay datos aún.";
        }
    }

    // Funcion llamada por un boton, para reinciar todo

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
