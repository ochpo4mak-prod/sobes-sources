using UnityEngine;
using Unity.Entities;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _UIPauseMenu;
    // [SerializeField] private GameObject _UIInventory;

    private bool isPause = false;
    public static bool inInventory = false;
    public static bool firstOpenInv = false;
    private World _world;
    private Button _UIResumeBtn;
    private Button _UIQuitBtn;

    void Start()
    {
        _world = World.DefaultGameObjectInjectionWorld;

        _UIPauseMenu.SetActive(false);
        // _UIInventory.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            _world.QuitUpdate = isPause;
            _UIPauseMenu.SetActive(isPause);
            // _UIInventory.SetActive(false);
            inInventory = false;

            if (isPause)
            {
                var root = _UIPauseMenu.GetComponent<UIDocument>().rootVisualElement;
                _UIResumeBtn = root.Q<Button>("resume-btn");
                _UIQuitBtn = root.Q<Button>("quit-btn");

                _UIResumeBtn.clicked += ResumePauseMenu;
                _UIQuitBtn.clicked += QuitPauseMenu;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isPause)
            {
                inInventory = !inInventory;
                // _UIInventory.SetActive(inInventory);
            }
        }
    }

    void ResumePauseMenu()
    {
        isPause = false;
        _world.QuitUpdate = false;
        _UIPauseMenu.SetActive(false);
    }

    void QuitPauseMenu()
    {
        Application.Quit();
        Debug.Log("CloseApp");
    }
}