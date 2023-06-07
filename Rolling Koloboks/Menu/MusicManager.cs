using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public const string KEY_MUSIC = "MUSIC";

    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;

    private AudioSource _audioSource;

    private void Awake()
    {
        var objs = FindObjectsOfType<MusicManager>();
        if (objs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _audioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey(KEY_MUSIC))
            PlayerPrefs.SetFloat(KEY_MUSIC, 100);
    }

    private void Start()
    {
        PlayMenuMusic();
    }

    public void ChangeMusicVolume(float value)
    {
        _audioSource.volume = value / 100;
    }

    public void PlayMenuMusic()
    {
        StartCoroutine(SmoothPlayCoroutine(_menuMusic));
    }

    public void PlayGameMusic()
    {
        _audioSource.clip = _gameMusic;
        _audioSource.Play();
    }

    private IEnumerator SmoothPlayCoroutine(AudioClip clip)
    {
        _audioSource.volume = 0;
        _audioSource.clip = clip;
        _audioSource.Play();

        while (_audioSource.volume < PlayerPrefs.GetFloat(KEY_MUSIC) / 100)
        {
            _audioSource.volume += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
