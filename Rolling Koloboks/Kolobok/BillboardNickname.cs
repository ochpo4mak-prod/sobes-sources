using UnityEngine;
using Unity.Netcode;

public class BillboardNickname : NetworkBehaviour
{
    private Transform _player;
    private Transform _cameraTransform;

    private void Start()
    {
        if (!IsOwner)
            return;

        NetworkManager.OnClientConnectedCallback += Find_OnClientConnectedCallback;

        AddDataServerRpc(OwnerClientId,
                         LobbyManager.Instance.PlayerName,
                         LobbyManager.Instance.Character);
    }

    private void Find_OnClientConnectedCallback(ulong obj)
    {
        NetworkManager.ConnectedClients[obj].PlayerObject.GetComponentInChildren<BillboardNickname>().FindCamera();
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddDataServerRpc(ulong id, string name, string character)
    {
        Relay.Instance.PlayersData.Add(new PlayerData
        {
            Id = id,
            Name = name,
            Character = character
        });
    }

    public void FindCamera()
    {
        _player = transform.parent;
        _cameraTransform = FindObjectOfType<Camera>().transform;
    }

    private void LateUpdate()
    {
        if (_cameraTransform != null)
        {
            var playerPos = _player.position;
            playerPos.y += 0.8f;
            transform.position = playerPos;

            transform.LookAt(transform.position + _cameraTransform.rotation * Vector3.forward,
                             _cameraTransform.rotation * Vector3.up);
        }
    }
}
