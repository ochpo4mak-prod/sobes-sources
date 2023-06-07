using TMPro;
using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class WaitingBeforeStartGame : NetworkBehaviour
{
    [SerializeField] private int _secondsCount = 5;
    [SerializeField] private TMP_Text _countdownText;

    private void Start()
    {
        StartCoroutine(WaitingBeforeTimer());
        StartCoroutine(SettingNicknames.Instanse.SetNicknames());
        SetStartGameTimeClientRpc();
    }

    private IEnumerator WaitingBeforeTimer()
    {
        int counter = _secondsCount;
        CountdownTextChangeClientRpc(counter.ToString());
        PlayBeginCounterSoundClientRpc();

        while (counter != 0)
        {
            yield return new WaitForSeconds(1);
            counter--;

            if (counter != 0)
                CountdownTextChangeClientRpc(counter.ToString());
            else
                CountdownTextChangeClientRpc(Translation.Instance.Translate("Game.Roll"));
        }

        EnableKolobokMovingClientRpc();

        yield return new WaitForSeconds(1);
        DisableObjectClientRpc();
    }

    [ClientRpc]
    private void CountdownTextChangeClientRpc(string text)
    {
        _countdownText.text = text;
    }

    [ClientRpc]
    private void EnableKolobokMovingClientRpc()
    {
        MusicManager.Instance.PlayGameMusic();
        NetworkManager.LocalClient.PlayerObject.GetComponent<KolobokRolling>().enabled = true;
    }

    [ClientRpc]
    private void DisableObjectClientRpc() => _countdownText.text = string.Empty;

    [ClientRpc]
    private void SetStartGameTimeClientRpc() => Finish.Instance.SetStartTime();

    [ClientRpc]
    private void PlayBeginCounterSoundClientRpc() => SoundManager.Instance.PlayBeginCounterSound();
}
