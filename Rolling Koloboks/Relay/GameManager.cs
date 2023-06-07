using TMPro;
using System.Linq;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public struct FinalistData
{
    public string Name { get; set; }
    public string Time { get; set; }
    public int Place { get; set; }
    public string Character { get; set; }
}

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int _gameEndTimerValue = 15;
    [SerializeField] private TMP_Text _finalistsListText;

    private int _finalistPlace;
    public List<FinalistData> FinalistDatas { get; private set; } = new();

    private void Awake() => Instance = this;

    [ServerRpc(RequireOwnership = false)]
    public void DespawnPlayerServerRpc(NetworkObjectReference obj)
    {
        obj.TryGet(out NetworkObject networkObject);
        networkObject.Despawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddFinalistServerRpc(string name, string time)
    {
        _finalistPlace++;
        var character = Relay.Instance.PlayersData.First(x => x.Name == name).Character;
        AddFinalistClientRpc(name, time, _finalistPlace, character);

        if (_finalistPlace == Relay.Instance.PlayersCount)
        {
            GameEndCounter.Instance.EndGame();
            return;
        }

        if (_finalistPlace == 1)
            GameEndCounter.Instance.StartGameEndCounter(_gameEndTimerValue);
    }

    [ClientRpc]
    private void AddFinalistClientRpc(string name, string time, int place, string character)
    {
        _finalistsListText.text += $"{place}. {name}: {time}\n";

        FinalistDatas.Add(new FinalistData
        {
            Name = name,
            Time = time,
            Place = place,
            Character = character
        });
    }
}
