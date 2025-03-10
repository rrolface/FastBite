using UnityEngine;
using System.Collections;
public class obstaculosLogic : MonoBehaviour
{
    public GameObject[] badObstacles;  // Array de obst�culos malos
    public GameObject[] goodObstacles; // Array de obst�culos buenos
    public Transform[] spawnPoints;    // Puntos de spawn en la pista
    public float badObstacleInterval = 4f;  // Intervalo de spawn de obst�culos malos
    public float goodObstacleInterval = 2f; // Intervalo de spawn de obst�culos buenos
    public float obstacleSpeed = 5f;   // Velocidad de movimiento de los obst�culos

    private void Start()
    {
        if ((badObstacles == null || badObstacles.Length == 0) || (goodObstacles == null || goodObstacles.Length == 0))
        {
            Debug.LogError("Faltan obst�culos asignados en el Inspector.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Faltan puntos de spawn asignados en el Inspector.");
            return;
        }

        // Iniciar las corrutinas de spawn con diferentes intervalos
        StartCoroutine(SpawnObstacleRoutine(badObstacles, badObstacleInterval));
        StartCoroutine(SpawnObstacleRoutine(goodObstacles, goodObstacleInterval));
    }

    private IEnumerator SpawnObstacleRoutine(GameObject[] obstacleArray, float spawnInterval)
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Elegir un punto de spawn aleatorio
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Asegurar un delay entre cada spawn en diferentes puntos
            yield return new WaitForSeconds(1f);

            // Elegir un obst�culo aleatorio del grupo correspondiente
            GameObject obstacle = obstacleArray[Random.Range(0, obstacleArray.Length)];

            // Instanciar el obst�culo en el punto de spawn con la rotaci�n deseada
            Quaternion rotation = Quaternion.Euler(-89.98f, 0f, 90.745f);
            GameObject newObstacle = Instantiate(obstacle, spawnPoint.position, rotation);

            // Mover el obst�culo hacia el jugador
            StartCoroutine(MoveObstacle(newObstacle));
        }
    }

    private IEnumerator MoveObstacle(GameObject obstacle)
    {
        while (obstacle != null)
        {
            obstacle.transform.Translate(Vector3.back * obstacleSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
    }
}