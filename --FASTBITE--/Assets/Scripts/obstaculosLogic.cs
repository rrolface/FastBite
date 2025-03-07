using UnityEngine;

public class obstaculosLogic : MonoBehaviour
{
    public GameObject[] obstacles; // Array de objetos que pueden aparecer
    public Transform[] spawnPoints; // Puntos de spawn en la pista
    public float spawnInterval = 2f; // Intervalo de tiempo entre spawns
    public float obstacleSpeed = 5f; // Velocidad a la que se mueven los obstáculos

    private void Start()
    {
        // Verificar que los arrays estén asignados
        if (obstacles == null || obstacles.Length == 0)
        {
            Debug.LogError("No hay obstáculos asignados en el Inspector.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No hay puntos de spawn asignados en el Inspector.");
            return;
        }

        //Debug.Log("Configuración correcta. Iniciando generación de obstáculos...");

        // Iniciar la corrutina para spawnear obstáculos
        StartCoroutine(SpawnObstacles());
    }

    private System.Collections.IEnumerator SpawnObstacles()
    {
        while (true)
        {
            // Esperar el intervalo de tiempo antes de spawnear el siguiente obstáculo
            yield return new WaitForSeconds(spawnInterval);

            // Elegir un obstáculo aleatorio del array
            GameObject obstacle = obstacles[Random.Range(0, obstacles.Length)];
            //Debug.Log("Obstáculo seleccionado: " + obstacle.name);

            // Elegir un punto de spawn aleatorio
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            //Debug.Log("Punto de spawn seleccionado: " + spawnPoint.position);

            // Instanciar el obstáculo en el punto de spawn
            GameObject newObstacle = Instantiate(obstacle, spawnPoint.position, Quaternion.Euler(0,180,0));
            //Debug.Log("Obstáculo generado: " + newObstacle.name + " en " + spawnPoint.position);

            // Mover el obstáculo hacia el jugador
            StartCoroutine(MoveObstacle(newObstacle));
        }
    }

    private System.Collections.IEnumerator MoveObstacle(GameObject obstacle)
    {
        Debug.Log("Iniciando movimiento del obstáculo: " + obstacle.name);

        while (obstacle != null)
        {
            // Mover el obstáculo hacia adelante (en la dirección negativa del eje Z)
            obstacle.transform.Translate(Vector3.forward * obstacleSpeed * Time.deltaTime);
            //Debug.Log("Obstáculo " + obstacle.name + " en posición: " + obstacle.transform.position);

            yield return null;
        }
    }
}