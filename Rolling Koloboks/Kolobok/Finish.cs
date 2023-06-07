using TMPro;
using UnityEngine;
using Unity.Netcode;
using System.Diagnostics;

public class Finish : MonoBehaviour
{
    public static Finish Instance { get; private set; }

    private Stopwatch _stopwatch;

    private void Awake()
    {
        Instance = this;
        _stopwatch = new Stopwatch();
    }

    public void SetStartTime() => _stopwatch.Start();

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out NetworkObject player) && player.IsOwner)
        {
            var name = player.GetComponentInChildren<TMP_Text>().text;
            var time = _stopwatch.Elapsed.ToString("mm\\:ss\\.ff");

            SoundManager.Instance.PlayFinishSound();
            GameManager.Instance.DespawnPlayerServerRpc(player);
            GameManager.Instance.AddFinalistServerRpc(name, time);
            SpectatorMode.Instance.StartEnableSpectatorMode();
        }
    }
}
