using UnityEngine;
using Cinemachine;
using Unity.Netcode;
using System.Collections;

[RequireComponent(typeof(Kolobok))]
public class KolobokTurning : NetworkBehaviour
{
    private Kolobok _kolobok;
    private CinemachineFreeLook _cinemachine;

    private void Awake()
    {
        _kolobok = GetComponent<Kolobok>();
    }

    private void FixedUpdate()
    {
        if (_cinemachine == null)
            return;

        if (!IsOwner)
            return;

        CinemachineRotate();
    }

    public void FindAndSetCinemachine()
    {
        _cinemachine = FindObjectOfType<CinemachineFreeLook>();

        if (!IsOwner)
            return;

        _cinemachine.Follow = transform;
        _cinemachine.LookAt = transform;

        StartCoroutine(RotateCinemachine());
    }

    private IEnumerator RotateCinemachine()
    {
        yield return new WaitForSeconds(0.05f);

        _cinemachine.m_XAxis.Value += -Camera.main.transform.eulerAngles.y;
    }

    private void CinemachineRotate()
    {
        float turnHorizontal;

        if (Application.isMobilePlatform)
            turnHorizontal = Input.acceleration.x * 2;
        else
            turnHorizontal = Input.GetAxis("Horizontal");

        _cinemachine.m_XAxis.Value += _kolobok.Speed / 10 * turnHorizontal * Time.deltaTime;
    }
}
