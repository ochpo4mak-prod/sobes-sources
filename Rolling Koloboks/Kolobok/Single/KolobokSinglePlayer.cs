using UnityEngine;
using Cinemachine;
using System.Collections;

public class KolobokSinglePlayer : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _bounceForce;
    public bool IsMoving = false;

    private Rigidbody _rigidbody;
    private CinemachineFreeLook _cinemachine;
    private Camera _camera;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _cinemachine = FindObjectOfType<CinemachineFreeLook>();
        _camera = FindObjectOfType<Camera>();

        _cinemachine.Follow = transform;
        _cinemachine.LookAt = transform; 
        
        StartCoroutine(RotateCinemachine());
    }

    private IEnumerator RotateCinemachine()
    {
        yield return new WaitForSeconds(0.05f);

        _cinemachine.m_XAxis.Value += -Camera.main.transform.eulerAngles.y;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        _cinemachine.m_XAxis.Value += 180;
    //        StartCoroutine(SetLookAtNull());
    //    }

    //    if (Input.GetKeyUp(KeyCode.C))
    //    {
    //        _cinemachine.LookAt = transform;
    //        _cinemachine.m_XAxis.Value -= 180;
    //    }
    //}

    //private IEnumerator SetLookAtNull()
    //{
    //    yield return new WaitForSeconds(0.05f);
    //    _cinemachine.LookAt = null;
    //}

    private void FixedUpdate()
    {
        if (IsMoving)
            Move();

        CinemachineRotate();
    }

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

        //if (Input.GetKey(KeyCode.C))
        //    movement = -cameraForward * moveVertical;

        _rigidbody.AddForce(_speed * Time.deltaTime * movement);
    }

    private void CinemachineRotate()
    {
        float turnHorizontal;

        if (Application.isMobilePlatform)
            turnHorizontal = Input.acceleration.x;
        else
            turnHorizontal = Input.GetAxis("Horizontal");

        _cinemachine.m_XAxis.Value += _speed / 10 * turnHorizontal * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out KolobokNPC _))
        {
            if (collision.relativeVelocity.magnitude > 0)
            {
                var bounceDirection = Vector3.Reflect(collision.relativeVelocity.normalized,
                                                      collision.contacts[0].normal);

                _rigidbody.AddForce(bounceDirection * _bounceForce, ForceMode.Impulse);
            }
        }
    }
}
