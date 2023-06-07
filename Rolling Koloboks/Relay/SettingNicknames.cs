using TMPro;
using UnityEngine;
using System.Linq;
using Unity.Netcode;
using System.Collections;

public class SettingNicknames : NetworkBehaviour
{
    public static SettingNicknames Instanse { get; private set; }

    private void Awake()
    {
        Instanse = this;
    }

    public IEnumerator SetNicknames()
    {
        yield return new WaitForSeconds(0.1f);

        foreach (var data in Relay.Instance.PlayersData)
            SetNicknamesClientRpc(data.Id, data.Name, data.Character);
    }

    [ClientRpc]
    private void SetNicknamesClientRpc(ulong id, string name, string character)
    {
        var players = FindObjectsOfType<Kolobok>();
        var player = players.First(x => x.GetComponent<NetworkObject>().OwnerClientId == id);
        var nicknameText = player.GetComponentInChildren<TMP_Text>();
        nicknameText.text = name;

        var playerCharacter = System.Enum.Parse<LobbyManager.PlayerCharacter>(character);

        switch (playerCharacter)
        {
            case LobbyManager.PlayerCharacter.Rabbit:
                nicknameText.color = Color.white;
                break;
            case LobbyManager.PlayerCharacter.Fox:
                nicknameText.color = new Color(1f, 0.65f, 0f);
                break;
            case LobbyManager.PlayerCharacter.Bear:
                nicknameText.color = new Color(0.59f, 0.29f, 0f);
                break;
            case LobbyManager.PlayerCharacter.Wolf:
                nicknameText.color = Color.gray;
                break;
        }
    }
}
