using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public const string KEY_SOUNDS = "SOUNDS";

    [SerializeField] private AudioClip _tapSound;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _finishSound;
    [SerializeField] private AudioClip _resultsSound;
    [SerializeField] private AudioClip _beginCounterSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        var objs = FindObjectsOfType<SoundManager>();
        if (objs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _audioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey(KEY_SOUNDS))
            PlayerPrefs.SetFloat(KEY_SOUNDS, 100);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PlayTapSound();
    }

    public void PlayTapSound() => PlaySound(_tapSound);
    public void PlayDeathSound() => PlaySound(_deathSound);
    public void PlayFinishSound() => PlaySound(_finishSound);
    public void PlayResultsSound() => PlaySound(_resultsSound);
    public void PlayBeginCounterSound() => PlaySound(_beginCounterSound);

    private void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip, PlayerPrefs.GetFloat(KEY_SOUNDS) / 100);
    }
}
