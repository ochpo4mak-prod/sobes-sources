using Cinemachine;
using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class Death : MonoBehaviour
{
    [SerializeField] private Transform _checkPoints;
    private Transform[] _checkPointsList;
    private CinemachineFreeLook _cinemachine;

    private void Start()
    {
        _checkPointsList = _checkPoints.GetComponentsInChildren<Transform>();
        _cinemachine = FindObjectOfType<CinemachineFreeLook>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out KolobokRolling kolobok))
            StartCoroutine(RotateCinemachine(kolobok));
    }

    private IEnumerator RotateCinemachine(KolobokRolling kolobok)
    {
        try
        {
            SoundManager.Instance.PlayDeathSound();
            
            kolobok.enabled = false;
            Loading.Instanse.EnableLoading();
            Loading.Instanse.DisableLoading();

            kolobok.transform.position = GetClosestCheckPont(_checkPointsList, kolobok.transform).position;
            kolobok.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }
        catch { }

        yield return new WaitForSeconds(0.3f);
        _cinemachine.m_XAxis.Value += -Camera.main.transform.eulerAngles.y;
        yield return new WaitForSeconds(0.3f);

        try
        {
            kolobok.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            kolobok.enabled = true;
        }
        catch { }
    }

    private Transform GetClosestCheckPont(Transform[] checkPonts, Transform player)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = player.position;

        for (int i = 0; i < checkPonts.Length; i++)
        {
            Vector3 directionToTarget = checkPonts[i].position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = checkPonts[i];
            }
        }

        return bestTarget;
    }
}
