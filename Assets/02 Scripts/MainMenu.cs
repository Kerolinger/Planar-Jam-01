using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject m_creditsMenu;

    public void BTN_StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void BTN_ToggleCredits()
    {
        m_creditsMenu.SetActive(!m_creditsMenu.activeSelf);
    }

    public void BTN_QuitGame()
    {
        Application.Quit();
    }
}
