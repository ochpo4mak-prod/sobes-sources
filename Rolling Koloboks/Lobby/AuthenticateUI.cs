using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;

public class AuthenticateUI : MonoBehaviour
{
    [SerializeField] private Canvas _lobbyCanvas;
    [SerializeField] private Button _closeButton;

    private void Awake()
    {
        _lobbyCanvas.enabled = false;
        _closeButton.onClick.AddListener(() =>
        {
            Loading.Instanse.OpenScene("Menu");
            AuthenticationService.Instance.SignOut();
        });
    }

    private void Start()
    {
        EditPlayerName.Instance.ChangeNameWithAuth(_lobbyCanvas);
    }
}