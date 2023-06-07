using UnityEngine;
using Unity.Netcode;
using Unity.Services.Core;
using Unity.Services.Relay;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies.Models;
using Unity.Networking.Transport.Relay;
using System.Collections;

public struct PlayerData
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string Character { get; set; }
}

public class Relay : MonoBehaviour
{
    public static Relay Instance { get; private set; }
    public int PlayersCount { get; set; }
    public List<PlayerData> PlayersData { get; private set; } = new();

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async Task<string> CreateRelay(Lobby lobby)
    {
        try
        {
            PlayersCount = lobby.Players.Count;

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(lobby.MaxPlayers);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);

            RelayServerData relayServerData = new(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            StartCoroutine(OpenGameScene(lobby));
            
            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            Loading.Instanse.EnableLoading();
            Debug.Log("Joining Relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    private IEnumerator OpenGameScene(Lobby lobby)
    {
        yield return new WaitForSeconds(10);
        NetworkManager.Singleton.SceneManager.LoadScene(lobby.Data[LobbyManager.KEY_MAP].Value, 
                                                        LoadSceneMode.Single);
    }
}