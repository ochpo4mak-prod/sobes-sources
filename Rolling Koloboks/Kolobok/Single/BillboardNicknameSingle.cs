using UnityEngine;

public class BillboardNicknameSingle : MonoBehaviour
{
    private Transform _player;
    private Transform _cameraTransform;

    public void Awake()
    {
        _player = transform.parent;
        _cameraTransform = FindObjectOfType<Camera>().transform;
    }

    private void LateUpdate()
    {
        if (_cameraTransform != null)
        {
            var playerPos = _player.position;
            playerPos.y += 0.8f;
            transform.position = playerPos;

            transform.LookAt(transform.position + _cameraTransform.rotation * Vector3.forward,
                             _cameraTransform.rotation * Vector3.up);
        }
    }
}
