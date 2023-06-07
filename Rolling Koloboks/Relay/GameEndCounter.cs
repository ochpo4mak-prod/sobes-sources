using TMPro;
using UnityEngine;
using Unity.Netcode;
using System.Collections;
using Unity.Services.Authentication;

public class GameEndCounter : NetworkBehaviour
{
    public static GameEndCounter Instance;

    [SerializeField] private TMP_Text _counterText;
    [SerializeField] private GameEndWindow _gameEndWindow;

    public bool IsEnd { get; private set; } = false;

    private void Awake() => Instance = this;

    public void StartGameEndCounter(int gameEndTimerValue)
    {
        StartCoroutine(EndCounter(gameEndTimerValue));
    }

    private IEnumerator EndCounter(int counter)
    {
        CountdownTextChangeClientRpc(counter.ToString());

        while (counter != 0)
        {
            yield return new WaitForSeconds(1);
            counter--;

            if (counter != 0)
                CountdownTextChangeClientRpc(counter.ToString());
            else
                EndGame();
        }
    }

    public void EndGame()
    {
        if (IsEnd)
            return;

        Pause.Instance.PauseButtonDisabler();
        EndChangeClientRpc();
        CountdownTextChangeClientRpc(Translation.Instance.Translate("Game.End"));
        EnableGameEndWindowClientRpc();
        SignOutClientRpc();
    }

    [ClientRpc]
    private void EndChangeClientRpc()
    {
        IsEnd = true;
    }

    [ClientRpc]
    private void CountdownTextChangeClientRpc(string text)
    {
        _counterText.text = text;
    }

    [ClientRpc]
    private void EnableGameEndWindowClientRpc()
    {
        SpectatorMode.Instance.StartDisableNextButton();
        _gameEndWindow.gameObject.SetActive(true);
    }

    [ClientRpc]
    private void SignOutClientRpc()
    {
        AuthenticationService.Instance.SignOut();
        NetworkManager.Shutdown();
        Destroy(NetworkManager.gameObject);
    }
}
