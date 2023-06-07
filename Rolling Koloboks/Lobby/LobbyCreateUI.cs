using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;

public class LobbyCreateUI : MonoBehaviour
{
    public static LobbyCreateUI Instance { get; private set; }

    [SerializeField] private Button createButton;
    [SerializeField] private Button lobbyNameButton;
    [SerializeField] private Button publicPrivateButton;
    [SerializeField] private Button maxPlayersButton;
    [SerializeField] private Button mapButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI publicPrivateText;
    [SerializeField] private TextMeshProUGUI maxPlayersText;
    [SerializeField] private TextMeshProUGUI mapText;

    private string lobbyName;
    private bool isPrivate;
    private int maxPlayers;
    private LobbyManager.Map map;
    private string password;
    private bool isHolmogorovka = true;

    private void Awake()
    {
        Instance = this;
        Lobby lobby;

        createButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.CreateLobby(
                lobbyName,
                maxPlayers,
                isPrivate,
                map
            );


            Hide();
        });

        lobbyNameButton.onClick.AddListener(() =>
        {
            UI_InputWindow.Show_Static(Translation.Instance.Translate("LobbyName.Title"), lobbyName, "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZàáâãäå¸æçèéêëìíîïðñòóôõö÷øùûüúýþÿÀÁÂÃÄÅ¨ÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÛÜÚÝÞß .,-", 20,
            () =>
            {
                // Cancel
            },
            (string lobbyName) =>
            {
                this.lobbyName = lobbyName;
                UpdateText();
            });
        });

        publicPrivateButton.onClick.AddListener(() =>
        {
            isPrivate = !isPrivate;
            UpdateText();
        });

        maxPlayersButton.onClick.AddListener(() =>
        {
            UI_InputWindow.Show_Static(Translation.Instance.Translate("MaxPlayers.Title"), maxPlayers, "123456789", 1,
            () =>
            {
                // Cancel
            },
            (int maxPlayers) =>
            {
                this.maxPlayers = maxPlayers;
                UpdateText();
            });
        });

        mapButton.onClick.AddListener(() =>
        {
            isHolmogorovka = !isHolmogorovka;

            switch (isHolmogorovka)
            {
                case true:
                    map = LobbyManager.Map.Holmogorovka;
                    break;
                case false:
                    map = LobbyManager.Map.UstRechinsk;
                    break;
            }
            UpdateText();
        });

        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        Hide();
    }

    private void UpdateText()
    {
        lobbyNameText.text = lobbyName;

        if (isPrivate)
            publicPrivateText.text = Translation.Instance.Translate("CreateLobby.Visibility").Split("/")[0];
        else
            publicPrivateText.text = Translation.Instance.Translate("CreateLobby.Visibility").Split("/")[1];

        maxPlayersText.text = maxPlayers.ToString();

        mapText.text = Translation.Instance.Translate(map.ToString());
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        lobbyName = Translation.Instance.Translate("CreateLobby.Name");
        isPrivate = false;
        maxPlayers = 8;
        map = LobbyManager.Map.Holmogorovka;

        UpdateText();
    }
}