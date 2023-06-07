using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FinishSingle : MonoBehaviour
{
    public static FinishSingle Instance { get; private set; }

    [SerializeField] private TMP_Text _mapText;
    [SerializeField] private TMP_Text _resultsText;
    [SerializeField] private TMP_Text _counterText;
    [SerializeField] private GameObject _resultWindow;
    [SerializeField] private Sprite _playerSprite;
    [SerializeField] private Sprite _botSprite;
    [SerializeField] private Button _nextButton;

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

    [SerializeField] private int _counter;

    private int _place = 1;
    private int _playerPlace;
    private Stopwatch _stopwatch;
    private List<FinalistSinlge> _finalists = new();

    private void Awake()
    {
        Instance = this;
        _stopwatch = new Stopwatch();

        _nextButton.onClick.AddListener(() =>
        {
            Loading.Instanse.OpenScene("Menu");
        });
    }

    public void SetStartTime() => _stopwatch.Start();

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out KolobokNPC npc))
        {
            var name = npc.GetComponentInChildren<TMP_Text>().text;
            var time = _stopwatch.Elapsed.ToString("mm\\:ss\\.ff");

            if (_place == 1)
                StartCoroutine(EnableEndCounter());

            _resultsText.text += $"{_place}. {name}: {time}\n";
            _place++;

            _finalists.Add(new FinalistSinlge
            {
                Name = name,
                Time = time
            });

            Destroy(npc.gameObject);
        }

        if (collider.TryGetComponent(out KolobokSinglePlayer player))
        {
            SoundManager.Instance.PlayFinishSound();

            var name = Translation.Instance.Translate("You");
            var time = _stopwatch.Elapsed.ToString("mm\\:ss\\.ff");

            _resultsText.text += $"{_place}. {name}: {time}\n";
            _playerPlace = _place;
            _place++;

            _finalists.Add(new FinalistSinlge
            {
                Name = name,
                Time = time
            });

            Destroy(player.gameObject);

            EnableResultWindow();
        }
    }

    private void EnableResultWindow()
    {
        SoundManager.Instance.PlayResultsSound();
        MusicManager.Instance.PlayMenuMusic();

        StopCoroutine(EnableEndCounter());

        _resultWindow.SetActive(true);

        _mapText.text = Translation.Instance.Translate(SceneManager.GetActiveScene().name.Split('S')[0]);

        try
        {
            _nameFirst.text = _finalists[0].Name;
            _timeFirst.text = _finalists[0].Time;
            _logoFirst.sprite = _botSprite;
        }
        catch
        {
            _nameFirst.text = string.Empty;
            _timeFirst.text = string.Empty;
        }

        try
        {
            _nameSecond.text = _finalists[1].Name;
            _timeSecond.text = _finalists[1].Time;
            _logoSecond.sprite = _botSprite;
        }
        catch 
        {
            _nameSecond.text = string.Empty;
            _timeSecond.text = string.Empty;
        }

        try
        {
            _nameThird.text = _finalists[2].Name;
            _timeThird.text = _finalists[2].Time;
            _logoThird.sprite = _botSprite;
        }
        catch
        {
            _nameThird.text = string.Empty;
            _timeThird.text = string.Empty;
        }

        switch (_playerPlace)
        {
            case 1:
                _logoFirst.sprite = _playerSprite;
                break;
            case 2:
                _logoSecond.sprite = _playerSprite;
                break;
            case 3:
                _logoThird.sprite = _playerSprite;
                break;
        }
    }

    private IEnumerator EnableEndCounter()
    {
        int counter = _counter;
        _counterText.text = counter.ToString();

        while (counter != 0)
        {
            yield return new WaitForSeconds(1);
            counter--;

            if (counter != 0)
                _counterText.text = counter.ToString();
            else
            {
                try
                {
                    Destroy(FindObjectOfType<KolobokSinglePlayer>().gameObject);
                }
                catch { }

                EnableResultWindow();
            }
        }
    }

    private struct FinalistSinlge
    {
        public string Name;
        public string Time;
    }
}