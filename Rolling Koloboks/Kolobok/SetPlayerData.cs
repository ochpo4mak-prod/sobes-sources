using UnityEngine;

public class SetPlayerData : MonoBehaviour
{
    private void Start()
    {
        Loading.Instanse.DisableLoading();

        var allPlayers = FindObjectsOfType<Kolobok>();

        foreach (var player in allPlayers)
        {
            player.SetToSpawnPoint();
            player.GetComponent<KolobokRolling>().FindCamera();
            player.GetComponent<KolobokTurning>().FindAndSetCinemachine();
            player.GetComponentInChildren<BillboardNickname>().FindCamera();
        }
    }
}
