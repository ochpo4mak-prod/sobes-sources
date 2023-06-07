using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Kolobok))]
public class KolobokRolling : NetworkBehaviour
{
    private Camera _camera;
    private Kolobok _kolobok;

    private void Awake()
    {
        _kolobok = GetComponent<Kolobok>();
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;

        if (_camera == null)
            return;

        Move();
    }

    public void FindCamera() => _camera = FindObjectOfType<Camera>();

    private void Move()
    {
        float moveVertical = Input.GetAxis("Vertical");

        moveVertical += 1;

        if (Application.isMobilePlatform && Input.GetMouseButton(0))
            moveVertical -= 1;

        moveVertical = Mathf.Clamp(moveVertical, -1, 1);

        Vector3 cameraForward = _camera.transform.forward;
        Vector3 cameraRight = _camera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        Vector3 movement = cameraForward * moveVertical;

        _kolobok.Rigidbody.AddForce(_kolobok.Speed * Time.deltaTime * movement);
    }
}
