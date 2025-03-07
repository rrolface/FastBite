using System.Collections.Generic;
using UnityEngine;

public class RankingManager
{
    public static RankingManager Instance; // Singleton para acceder desde otros scripts

    private List<RankingEntry> ranking = new List<RankingEntry>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            
        }
    }

    public void GuardarRanking(int barras, float distancia, float tiempo)
    {
        // Crear una nueva entrada de ranking
        RankingEntry nuevaEntrada = new RankingEntry(barras, distancia, tiempo);

        // Agregar al ranking
        ranking.Add(nuevaEntrada);

        // Ordenar por mayor número de barras recogidas, luego por distancia y finalmente por tiempo
        ranking.Sort((a, b) =>
        {
            int compareBarras = b.barrasRecogidas.CompareTo(a.barrasRecogidas);
            if (compareBarras != 0) return compareBarras;

            int compareDistancia = b.distancia.CompareTo(a.distancia);
            if (compareDistancia != 0) return compareDistancia;

            return a.tiempo.CompareTo(b.tiempo); // Menor tiempo es mejor en caso de empate
        });

        // Opcionalmente, puedes limitar el ranking a los mejores N jugadores
        if (ranking.Count > 5) // Guardar solo el top 5
        {
            ranking.RemoveAt(ranking.Count - 1);
        }
    }

    public void MostrarRanking()
    {
        Debug.Log("=== RANKING ===");
        for (int i = 0; i < ranking.Count; i++)
        {
            Debug.Log((i + 1) + ". Barras: " + ranking[i].barrasRecogidas +
                      " | Distancia: " + ranking[i].distancia.ToString("F1") + "m" +
                      " | Tiempo: " + ranking[i].tiempo.ToString("F1") + "s");
        }
    }
}

[System.Serializable]
public class RankingEntry
{
    public int barrasRecogidas;
    public float distancia;
    public float tiempo;

    public RankingEntry(int barras, float distancia, float tiempo)
    {
        this.barrasRecogidas = barras;
        this.distancia = distancia;
        this.tiempo = tiempo;
    }
}
