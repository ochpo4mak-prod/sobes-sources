using TMPro;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CreateGameUI : MonoBehaviour
{
    public static CreateGameUI Instanse;

    [SerializeField] private Button _close;
    [SerializeField] private Button _mapChange;
    [SerializeField] private Button _create;
    [SerializeField] private Slider _sliderCount;
    [SerializeField] private TMP_Text _mapText;

    private List<string> _maps = new();
    private int _mapIndex = 0;

    public int BotsCount { get; private set; } = 4;

    private void Awake()
    {
        Instanse = this;

        _maps = Enum.GetNames(typeof(LobbyManager.Map)).ToList();

        _close.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        _mapChange.onClick.AddListener(() =>
        {
            if (_mapIndex + 1 < _maps.Count)
                _mapIndex++;
            else
                _mapIndex = 0;

            _mapText.text = Translation.Instance.Translate(_maps[_mapIndex]);
        });

        _create.onClick.AddListener(() =>
        {
            Loading.Instanse.OpenScene(_maps[_mapIndex] + "Single");
        });

        _mapText.text = Translation.Instance.Translate(_maps[_mapIndex]);
    }

    public void OnSliderCountChange(TMP_Text valueText)
    {
        valueText.text = _sliderCount.value.ToString();
        BotsCount = (int)_sliderCount.value;
    }
}
