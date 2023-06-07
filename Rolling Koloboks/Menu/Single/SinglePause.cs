using UnityEngine;
using UnityEngine.UI;

public class SinglePause : MonoBehaviour
{
    public static SinglePause Instance { get; private set; }

    [SerializeField] private GameObject _pause;
    [SerializeField] private Button _pauseBtn;
    [SerializeField] private Button _continueBtn;
    [SerializeField] private Button _quitBtn;

    private void Awake()
    {
        Instance = this;

        _pause.SetActive(false);
        _pauseBtn.gameObject.SetActive(false);

        _pauseBtn.onClick.AddListener(PauseButton);
        _continueBtn.onClick.AddListener(ContinueButton);
        _quitBtn.onClick.AddListener(QuitButton);
    }

    public void PauseButtonEnable()
    {
        _pauseBtn.gameObject.SetActive(true);
    }

    private void PauseButton()
    {
        _pause.SetActive(true);
        _pauseBtn.gameObject.SetActive(false);

        Time.timeScale = 0f;
    }

    private void ContinueButton()
    {
        _pause.SetActive(false);
        _pauseBtn.gameObject.SetActive(true);

        Time.timeScale = 1f;
    }

    private void QuitButton()
    {
        Time.timeScale = 1f;

        Loading.Instanse.OpenScene("Menu");

        MusicManager.Instance.PlayMenuMusic();
    }
}
