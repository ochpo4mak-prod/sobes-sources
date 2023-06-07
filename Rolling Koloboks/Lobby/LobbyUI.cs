using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance { get; private set; }

    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI mapText;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;
    [SerializeField] private Button changeRabbitButton;
    [SerializeField] private Button changeFoxButton;
    [SerializeField] private Button changeBearButton;
    [SerializeField] private Button changeWolfButton;
    [SerializeField] private Button leaveLobbyButton;
    [SerializeField] private Button changeMapButton;
    [SerializeField] private Button startGameButton;

    private void Awake()
    {
        Instance = this;

        playerSingleTemplate.gameObject.SetActive(false);

        changeRabbitButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Rabbit);
        });
        changeFoxButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Fox);
        });
        changeBearButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Bear);
        });
        changeWolfButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Wolf);
        });

        leaveLobbyButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.LeaveLobby();
        });

        changeMapButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.ChangeMap();
        });

        startGameButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.StartGame();
        });
    }

    private void Start()
    {
        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyManager.Instance.OnLobbyGameModeChanged += UpdateLobby_Event;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnGameStarted += LobbyManager_OnGameStart;

        Hide();
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e)
    {
        ClearLobby();
        Hide();
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e)
    {
        UpdateLobby();
    }

    private void LobbyManager_OnGameStart(object sender, System.EventArgs e)
    {
        ClearLobby();
        Hide();
    }

    private void UpdateLobby()
    {
        UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby)
    {
        try
        {
            ClearLobby();

            foreach (Player player in lobby.Players)
            {
                Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
                playerSingleTransform.gameObject.SetActive(true);
                LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();

                lobbyPlayerSingleUI.SetKickPlayerButtonVisible(
                    LobbyManager.Instance.IsLobbyHost() &&
                    player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
                );

                lobbyPlayerSingleUI.UpdatePlayer(player);
            }

            changeMapButton.gameObject.SetActive(LobbyManager.Instance.IsLobbyHost());
            startGameButton.gameObject.SetActive(LobbyManager.Instance.IsLobbyHost());

            lobbyNameText.text = lobby.Name;
            playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;

            mapText.text = Translation.Instance.Translate(lobby.Data[LobbyManager.KEY_MAP].Value);

            if (lobby.IsPrivate)
                lobbyCodeText.text = lobby.LobbyCode;
            else
                lobbyCodeText.text = "";

            Show();
        }
        catch { }
    }

    private void ClearLobby()
    {
        try
        {
            foreach (Transform child in container)
            {
                if (child == playerSingleTemplate) continue;
                Destroy(child.gameObject);
            }
        }
        catch { }
    }

    private void Hide()
    {
        try { gameObject.SetActive(false); }
        catch { }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}