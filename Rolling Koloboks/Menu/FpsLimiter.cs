using UnityEngine;

public class FpsLimiter : MonoBehaviour
{
    private void Start()
    {
        if (Application.isMobilePlatform)
            Application.targetFrameRate = 60;
        else
            Application.targetFrameRate = 144;
    }
}
