using UnityEngine;

public class LobbyAssets : MonoBehaviour
{
    public static LobbyAssets Instance { get; private set; }

    [SerializeField] private Sprite rabbitSprite;
    [SerializeField] private Sprite foxSprite;
    [SerializeField] private Sprite bearSprite;
    [SerializeField] private Sprite wolfSprite;

    private void Awake()
    {
        Instance = this;
    }

    public Sprite GetSprite(LobbyManager.PlayerCharacter playerCharacter)
    {
        switch (playerCharacter)
        {
            default:
            case LobbyManager.PlayerCharacter.Rabbit: return rabbitSprite;
            case LobbyManager.PlayerCharacter.Fox: return foxSprite;
            case LobbyManager.PlayerCharacter.Bear: return bearSprite;
            case LobbyManager.PlayerCharacter.Wolf: return wolfSprite;
        }
    }
}