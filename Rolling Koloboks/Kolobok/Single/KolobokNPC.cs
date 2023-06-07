using UnityEngine;

public class KolobokNPC : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _bounceForce;
    public bool IsMoving = false;

    private Rigidbody _rigidbody;

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        if (IsMoving)
            Move();
    }

    private void Move()
    {
        Vector3 movement = Vector3.forward * 1;

        _rigidbody.AddForce(_speed * Time.deltaTime * movement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out KolobokNPC _) ||
            collision.gameObject.TryGetComponent(out KolobokSinglePlayer _))
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
