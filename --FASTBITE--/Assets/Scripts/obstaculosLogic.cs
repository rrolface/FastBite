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

            // Elegir un punto de spawn aleatorio
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Definir la rotación deseada
            Quaternion rotation = Quaternion.Euler(-89.98f, 0f, 90.745f);

            // Instanciar el obstáculo en el punto de spawn con la rotación correcta
            GameObject newObstacle = Instantiate(obstacle, spawnPoint.position, rotation);

            // Mover el obstáculo hacia el jugador
            StartCoroutine(MoveObstacle(newObstacle));
        }
    }

    private System.Collections.IEnumerator MoveObstacle(GameObject obstacle)
    {
        while (obstacle != null)
        {
            // Mover el obstáculo en la dirección negativa del eje Z global
            obstacle.transform.Translate(Vector3.back * obstacleSpeed * Time.deltaTime, Space.World);

            yield return null;
        }
    }
}