using UnityEngine;

public class obstaculosLogic : MonoBehaviour
{
    public GameObject[] obstacles; // Array de objetos que pueden aparecer
    public Transform[] spawnPoints; // Puntos de spawn en la pista
    public float spawnInterval = 2f; // Intervalo de tiempo entre spawns
    public float obstacleSpeed = 5f; // Velocidad a la que se mueven los obst�culos

    private void Start()
    {
        // Verificar que los arrays est�n asignados
        if (obstacles == null || obstacles.Length == 0)
        {
            Debug.LogError("No hay obst�culos asignados en el Inspector.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No hay puntos de spawn asignados en el Inspector.");
            return;
        }

        //Debug.Log("Configuraci�n correcta. Iniciando generaci�n de obst�culos...");

        // Iniciar la corrutina para spawnear obst�culos
        StartCoroutine(SpawnObstacles());
    }

    private System.Collections.IEnumerator SpawnObstacles()
    {
        while (true)
        {
            // Esperar el intervalo de tiempo antes de spawnear el siguiente obst�culo
            yield return new WaitForSeconds(spawnInterval);

            // Elegir un obst�culo aleatorio del array
            GameObject obstacle = obstacles[Random.Range(0, obstacles.Length)];
            //Debug.Log("Obst�culo seleccionado: " + obstacle.name);

            // Elegir un punto de spawn aleatorio
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            //Debug.Log("Punto de spawn seleccionado: " + spawnPoint.position);

            // Instanciar el obst�culo en el punto de spawn
            GameObject newObstacle = Instantiate(obstacle, spawnPoint.position, Quaternion.Euler(0,180,0));
            //Debug.Log("Obst�culo generado: " + newObstacle.name + " en " + spawnPoint.position);

            // Mover el obst�culo hacia el jugador
            StartCoroutine(MoveObstacle(newObstacle));
        }
    }

    private System.Collections.IEnumerator MoveObstacle(GameObject obstacle)
    {
        Debug.Log("Iniciando movimiento del obst�culo: " + obstacle.name);

        while (obstacle != null)
        {
            // Mover el obst�culo hacia adelante (en la direcci�n negativa del eje Z)
            obstacle.transform.Translate(Vector3.forward * obstacleSpeed * Time.deltaTime);
            //Debug.Log("Obst�culo " + obstacle.name + " en posici�n: " + obstacle.transform.position);

            yield return null;
        }
    }
}