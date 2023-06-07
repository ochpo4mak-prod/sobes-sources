using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EditPlayerName : MonoBehaviour
{
    public static EditPlayerName Instance { get; private set; }

    public event EventHandler OnNameChanged;

    [SerializeField] private TextMeshProUGUI playerNameText;

    private string playerName = "Nickname";
    private const string PLAYER_NAME = "PLAYER_NAME";

    private void Awake()
    {
        Instance = this;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            UI_InputWindow.Show_Static(Translation.Instance.Translate("PlayerName.Title"), playerName, "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZàáâãäå¸æçèéêëìíîïðñòóôõö÷øùûüúýþÿÀÁÂÃÄÅ¨ÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÛÜÚÝÞß .,-", 20,
            () =>
            {
                // Cancel
            },
            (string newName) =>
            {
                ChangeName(newName);
            });
        });

        playerNameText.text = playerName;
    }

    private void Start()
    {
        OnNameChanged += EditPlayerName_OnNameChanged;
    }

    public void ChangeNameWithAuth(Canvas lobbyCanvas)
    {
        if (PlayerPrefs.HasKey(PLAYER_NAME))
        {
            ChangeName(PlayerPrefs.GetString(PLAYER_NAME));
            LobbyManager.Instance.Authenticate(GetPlayerName());
            lobbyCanvas.enabled = true;
        }
        else
        {
            UI_InputWindow.Show_Static(Translation.Instance.Translate("PlayerName.Title"), playerName, "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZàáâãäå¸æçèéêëìíîïðñòóôõö÷øùûüúýþÿÀÁÂÃÄÅ¨ÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÛÜÚÝÞß .,-", 20,
            () =>
            {
                // Cancel
            },
            (string newName) =>
            {
                ChangeName(newName);
                LobbyManager.Instance.Authenticate(GetPlayerName());
                lobbyCanvas.enabled = true;
            });
        }
    }

    private void ChangeName(string newName)
    {
        playerName = newName;
        playerNameText.text = playerName;
        PlayerPrefs.SetString(PLAYER_NAME, playerName);

        OnNameChanged?.Invoke(this, EventArgs.Empty);
    }

    private void EditPlayerName_OnNameChanged(object sender, EventArgs e)
    {
        LobbyManager.Instance.UpdatePlayerName(GetPlayerName());
    }

    public string GetPlayerName()
    {
        return playerName;
    }
}