using TMPro;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CheckDirection : MonoBehaviour
{
    [SerializeField] private TMP_Text _field;
    [SerializeField] private float _maxAngle = 240;
    [SerializeField] private float _minAngle = 120;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (_camera.transform.eulerAngles.y < _maxAngle &&
            _camera.transform.eulerAngles.y > _minAngle)
            _field.text = Translation.Instance.Translate("Game.Direction");
        else
            _field.text = string.Empty;
    }
}
