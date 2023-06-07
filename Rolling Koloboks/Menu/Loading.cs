using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public static Loading Instanse { get; private set; }

    private Animator _animator;
    private const string LOADING_TRIGGER = "Off";

    private void Awake()
    {
        var objs = FindObjectsOfType<Loading>();
        if (objs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        Instanse = this;
        _animator = GetComponentInChildren<Animator>();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        DisableLoading();
    }

    public void EnableLoading() => gameObject.SetActive(true);

    public void DisableLoading()
    {
        StartCoroutine(DisableLoadingCoroutine());
    }

    public void OpenScene(string sceneName)
    {
        gameObject.SetActive(true);
        StartCoroutine(OpenSceneCoroutine(sceneName));
    }

    private IEnumerator OpenSceneCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
        DisableLoading();
    }

    private IEnumerator DisableLoadingCoroutine()
    {
        _animator.SetTrigger(LOADING_TRIGGER);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
