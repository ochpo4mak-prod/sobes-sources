using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody))]
public class Kolobok : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _bounceForce;

    public float Speed => _speed;
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void SetToSpawnPoint()
    {
        var spawnObject = GameObject.Find("SpawnPoints");
        var spawnPoints = spawnObject.GetComponentsInChildren<Transform>();
        transform.position = spawnPoints[OwnerClientId + 1].position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Kolobok _))
        {
            if (collision.relativeVelocity.magnitude > 0)
            {
                var bounceDirection = Vector3.Reflect(collision.relativeVelocity.normalized,
                                                      collision.contacts[0].normal);

                Rigidbody.AddForce(bounceDirection * _bounceForce, ForceMode.Impulse);
            }
        }
    }
}