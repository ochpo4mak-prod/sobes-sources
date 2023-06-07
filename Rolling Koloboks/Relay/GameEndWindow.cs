using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEndWindow : MonoBehaviour
{
    [Header("1st")]
    [SerializeField] private Image _logoFirst;
    [SerializeField] private TMP_Text _nameFirst;
    [SerializeField] private TMP_Text _timeFirst;
    [Header("2st")]
    [SerializeField] private Image _logoSecond;
    [SerializeField] private TMP_Text _nameSecond;
    [SerializeField] private TMP_Text _timeSecond;
    [Header("3st")]
    [SerializeField] private Image _logoThird;
    [SerializeField] private TMP_Text _nameThird;
    [SerializeField] private TMP_Text _timeThird;
    [Header("Other")]
    [SerializeField] private TMP_Text _map;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Sprite _emptySprite;

    private void Awake()
    {
        _nextButton.onClick.AddListener(NextButtonClick);
    }

    public void OnEnable()
    {
        MusicManager.Instance.PlayMenuMusic();
        SoundManager.Instance.PlayResultsSound();

        SetFirstData();
        SetSecondData();
        SetThirdData();

        _map.text = Translation.Instance.Translate(SceneManager.GetActiveScene().name);
    }

    private void SetFirstData()
    {
        try
        {
            SetFinalistData(GameManager.Instance.FinalistDatas[0], _nameFirst, _timeFirst, _logoFirst);
        }
        catch
        {
            SetEmptyData(_nameFirst, _timeFirst, _logoFirst);
        }
    }

    private void SetSecondData()
    {
        try
        {
            SetFinalistData(GameManager.Instance.FinalistDatas[1], _nameSecond, _timeSecond, _logoSecond);
        }
        catch
        {
            SetEmptyData(_nameSecond, _timeSecond, _logoSecond);
        }
    }

    private void SetThirdData()
    {
        try
        {
            SetFinalistData(GameManager.Instance.FinalistDatas[2], _nameThird, _timeThird, _logoThird);
        }
        catch
        {
            SetEmptyData(_nameThird, _timeThird, _logoThird);
        }
    }

    private void SetFinalistData(FinalistData data, TMP_Text name, TMP_Text time, Image logo)
    {
        name.text = data.Name;
        time.text = data.Time;

        var character = Enum.Parse<LobbyManager.PlayerCharacter>(data.Character);
        logo.sprite = LobbyAssets.Instance.GetSprite(character);
    }

    private void SetEmptyData(TMP_Text name, TMP_Text time, Image logo)
    {
        name.text = string.Empty;
        time.text = string.Empty;
        logo.sprite = _emptySprite;
    }

    private void NextButtonClick()
    {
        Loading.Instanse.OpenScene("Menu");
    }
}
