using UnityEngine;
using Cinemachine;
using System.Collections;

public class DeathSingle : MonoBehaviour
{
    [SerializeField] private Transform _checkPoints;

    private CinemachineFreeLook _cinemachine;

    private void Awake()
    {
        _cinemachine = FindObjectOfType<CinemachineFreeLook>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out KolobokNPC npc))
        {
            var checkPoint = GetClosestCheckPont(_checkPoints.GetComponentsInChildren<Transform>(), npc.transform);
            npc.transform.position = checkPoint.position;
        }

        if (collision.gameObject.TryGetComponent(out KolobokSinglePlayer player))
        {
            StartCoroutine(RotateCinemachine(player));
        }
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

    private IEnumerator RotateCinemachine(KolobokSinglePlayer player)
    {
        try
        {
            SoundManager.Instance.PlayDeathSound();

            player.IsMoving = false;
            Loading.Instanse.EnableLoading();
            Loading.Instanse.DisableLoading();

            var checkPoint = GetClosestCheckPont(_checkPoints.GetComponentsInChildren<Transform>(), player.transform);
            player.transform.position = checkPoint.position;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }
        catch { }

        yield return new WaitForSeconds(0.3f);
        _cinemachine.m_XAxis.Value -= Camera.main.transform.eulerAngles.y;
        yield return new WaitForSeconds(0.3f);

        try
        {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            player.IsMoving = true;
        }
        catch { }
    }
}