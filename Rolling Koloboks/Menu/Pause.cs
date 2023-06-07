using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.Services.Authentication;

public class Pause : NetworkBehaviour
{
    public static Pause Instance;

    [SerializeField] private GameObject _pause;
    [SerializeField] private Button _pauseBtn;
    [SerializeField] private Button _continueBtn;
    [SerializeField] private Button _quitBtn;

    private void Awake()
    {
        Instance = this;

        _pause.SetActive(false);

        _pauseBtn.onClick.AddListener(PauseButton);
        _continueBtn.onClick.AddListener(ContinueButton);
        _quitBtn.onClick.AddListener(QuitButton);

        NetworkManager.OnClientDisconnectCallback += QuitAction;
    }

    private void PauseButton()
    {
        _pause.SetActive(true);
        _pauseBtn.gameObject.SetActive(false);
    }

    public void PauseButtonDisabler()
    {
        _pause.SetActive(false);
        _pauseBtn.gameObject.SetActive(false);
    }

    private void ContinueButton()
    {
        _pause.SetActive(false);
        _pauseBtn.gameObject.SetActive(true);
    }

    private void QuitButton()
    {
        if (IsHost)
            HostQuit();
        else
            Quit();
    }

    private void QuitAction(ulong id)
    {
        if (id == 0 && !GameEndCounter.Instance.IsEnd)
        {
            AuthenticationService.Instance.SignOut();
            NetworkManager.Shutdown();
            Destroy(NetworkManager.gameObject);

            Loading.Instanse.OpenScene("Menu");

            MusicManager.Instance.PlayMenuMusic();
        }
        else
            QuitServerRpc();
    }

    private void Quit()
    {
        AuthenticationService.Instance.SignOut();
        NetworkManager.Shutdown();
        Destroy(NetworkManager.gameObject);

        Loading.Instanse.OpenScene("Menu");

        MusicManager.Instance.PlayMenuMusic();
    }

    [ServerRpc(RequireOwnership = false)]
    private void QuitServerRpc()
    {
        Relay.Instance.PlayersCount--;
    }

    private void HostQuit()
    {
        SignOutClientRpc();
    }

    [ClientRpc]
    private void SignOutClientRpc()
    {
        AuthenticationService.Instance.SignOut();
        NetworkManager.Shutdown();
        Destroy(NetworkManager.gameObject);

        Loading.Instanse.OpenScene("Menu");

        MusicManager.Instance.PlayMenuMusic();
    }
}
