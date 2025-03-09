using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool juegoActivo = true;

    private UIManager uiManager;
    private RankingManager rankingManager;
    private PlayerManager player;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Sustituimos FindObjectOfType por FindFirstObjectByType
        uiManager = FindFirstObjectByType<UIManager>();
        rankingManager = FindFirstObjectByType<RankingManager>();
        player = FindFirstObjectByType<PlayerManager>();

        IniciarJuego();
    }

    public void IniciarJuego()
    {
        juegoActivo = true;
    }

    public void TerminarJuego()
    {
        if (!juegoActivo) return;

        juegoActivo = false;
        uiManager?.MostrarGameOver();

        if (player != null)
            rankingManager?.GuardarPuntaje(player.ObtenerPuntaje());
    }
}
