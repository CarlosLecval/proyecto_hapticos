using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    public VisualElement ui;

    public Button playButton;
    public Button quitButton;

    private void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        playButton = ui.Q<Button>("play-button");
        playButton.clicked += OnPlayButtonClicked;
        quitButton = ui.Q<Button>("quit-button");
        quitButton.clicked += OnQuitButtonClicked;
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    private void OnOptionsButtonClicked()
    {
        // Implement options menu logic here
        Debug.Log("Options button clicked");
    }

    private void OnPlayButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
