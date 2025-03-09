using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public TMP_Text rankingText;
    private List<int> scores = new List<int>();
    private const int maxRankings = 5;

    void Start()
    {
        CargarPuntajes();
        MostrarRanking();
    }

    public void GuardarPuntaje(int nuevoPuntaje)
    {
        scores.Add(nuevoPuntaje);
        scores.Sort((a, b) => b.CompareTo(a)); // Ordena de mayor a menor

        if (scores.Count > maxRankings)
            scores.RemoveAt(scores.Count - 1); // Mantiene solo los 5 mejores

        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt($"Ranking_{i}", scores[i]);
        }

        PlayerPrefs.Save();
        MostrarRanking();
    }

    private void CargarPuntajes()
    {
        scores.Clear();
        for (int i = 0; i < maxRankings; i++)
        {
            if (PlayerPrefs.HasKey($"Ranking_{i}"))
                scores.Add(PlayerPrefs.GetInt($"Ranking_{i}"));
        }
    }

    private void MostrarRanking()
    {
        if (!rankingText) return;

        rankingText.text = "?? **Ranking** ??\n";
        for (int i = 0; i < scores.Count; i++)
        {
            rankingText.text += $"{i + 1}. {scores[i]} pts\n";
        }
    }
}
