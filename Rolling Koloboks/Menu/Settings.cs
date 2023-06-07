using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _languageDropdown;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundsSlider;
    [SerializeField] private TMP_Text _musicValueText;
    [SerializeField] private TMP_Text _soundsValueText;

    private void OnEnable()
    {
        _languageDropdown.onValueChanged?.AddListener(LanguageDropdown);
        _musicSlider.onValueChanged?.AddListener(OnMusicSliderValueChange);
        _soundsSlider.onValueChanged?.AddListener(OnSoundsSliderValueChange);
    }

    private void OnDisable()
    {
        _languageDropdown.onValueChanged?.RemoveAllListeners();
        _musicSlider.onValueChanged?.RemoveAllListeners();
        _soundsSlider.onValueChanged?.RemoveAllListeners();
    }

    private void Start()
    {
        if (PlayerPrefs.GetString(Translation.KEY_LANGUAGE) == "RU")
            _languageDropdown.value = 0;
        else
            _languageDropdown.value = 1;

        _musicSlider.value = PlayerPrefs.GetFloat(MusicManager.KEY_MUSIC);
        _soundsSlider.value = PlayerPrefs.GetFloat(SoundManager.KEY_SOUNDS);
    }

    private void LanguageDropdown(int _)
    {
        if (_languageDropdown.value == 0)
            PlayerPrefs.SetString(Translation.KEY_LANGUAGE, "RU");
        else
            PlayerPrefs.SetString(Translation.KEY_LANGUAGE, "EN");

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    private void OnMusicSliderValueChange(float value)
    {
        PlayerPrefs.SetFloat(MusicManager.KEY_MUSIC, value);
        _musicValueText.text = value.ToString();
        MusicManager.Instance.ChangeMusicVolume(value);
    }

    private void OnSoundsSliderValueChange(float value)
    {
        PlayerPrefs.SetFloat(SoundManager.KEY_SOUNDS, value);
        _soundsValueText.text = value.ToString();
    }
}
