using Cinemachine;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CinemachineFreeLook))]
public class SpectatorMode : MonoBehaviour
{
    public static SpectatorMode Instance { get; private set; }

    [SerializeField] private Button _nextButton;
    private CinemachineFreeLook _cinemachine;
    private int _currentPlayerIndex;

    private List<Kolobok> OtherPlayers { get; set; } = new();

    private void Awake()
    {
        Instance = this;
        _cinemachine = GetComponent<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        _nextButton.onClick.AddListener(NextPlayer);
    }

    private void OnDisable()
    {
        _nextButton.onClick.RemoveAllListeners();
    }

    public void StartEnableSpectatorMode() => StartCoroutine(EnableSpectatorMode());

    public void StartDisableNextButton() => StartCoroutine(DisableNextButton());

    private IEnumerator EnableSpectatorMode()
    {
        yield return new WaitForSeconds(1f);

        OtherPlayers = FindObjectsOfType<Kolobok>().ToList();

        if (OtherPlayers.Count > 0)
        {
            _nextButton.gameObject.SetActive(true);
            
            _currentPlayerIndex = Random.Range(0, OtherPlayers.Count);
            FollowCurrentPlayer();
        }
    }

    private IEnumerator DisableNextButton()
    {
        yield return new WaitForSeconds(1.01f);
        _nextButton.gameObject.SetActive(false);
    }

    private void NextPlayer()
    {
        if (_currentPlayerIndex + 1 < OtherPlayers.Count)
            _currentPlayerIndex++;
        else
            _currentPlayerIndex = 0;

        FollowCurrentPlayer();
    }

    private void FollowCurrentPlayer()
    {
        try
        {
            _cinemachine.Follow = OtherPlayers[_currentPlayerIndex].transform;
            _cinemachine.LookAt = OtherPlayers[_currentPlayerIndex].transform;
        }
        catch
        {
            NextPlayer();
        }
    }
}
