using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenRules()
    {
        SceneManager.LoadScene("Rules");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);

        mainPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        mainPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }
}