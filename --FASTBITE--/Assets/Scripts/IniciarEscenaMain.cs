using UnityEngine;
using UnityEngine.SceneManagement; 

public class IniciarEscenaMain : MonoBehaviour
{
    public void cambiarEscena()
    {
        SceneManager.LoadScene("Main"); 
    }

    public void endgame()
    {
        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
        