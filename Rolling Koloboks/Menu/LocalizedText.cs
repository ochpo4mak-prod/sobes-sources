using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string _localizationKey;

    private void OnEnable()
    {
        GetComponent<TMP_Text>().text = Translation.Instance.Translate(_localizationKey);
    }
}