using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _createGameUI;

    public void SingleplayerButton()
    {
        _createGameUI.SetActive(true);
    }

    public void MultiplayerButton()
    {
        Loading.Instanse.OpenScene("Lobby");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SettingsButton()
    {
        _menu.SetActive(false);
        _settings.SetActive(true);
    }

    public void BackButton()
    {
        _settings.SetActive(false);
        _menu.SetActive(true);
    }
}
