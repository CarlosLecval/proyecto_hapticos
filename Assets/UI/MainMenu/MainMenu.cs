using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset chooseTeamAsset;

    private UIDocument document;
    private VisualTreeAsset mainMenuAsset;
    private VisualElement ui;

    private Button playButton;
    private Button quitButton;
    private Button redTeamButton;
    private Button blueTeamButton;
    private Button continueButton;
    private Button backButton;
    private Label redTeamPlayerTag;
    private Label blueTeamPlayerTag;
    private IVisualElementScheduledItem chooseTeamAnimationReset;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        mainMenuAsset = document.visualTreeAsset;
        ui = document.rootVisualElement;
    }

    private void OnEnable()
    {
        ShowMainMenu();
    }

    private void OnDisable()
    {
        UnregisterCallbacks();
    }

    private void ShowMainMenu()
    {
        chooseTeamAnimationReset?.Pause();
        chooseTeamAnimationReset = null;
        LoadScreen(mainMenuAsset);

        playButton = ui.Q<Button>("play-button");
        quitButton = ui.Q<Button>("quit-button");

        if (playButton != null)
        {
            playButton.clicked += OnPlayButtonClicked;
        }

        if (quitButton != null)
        {
            quitButton.clicked += OnQuitButtonClicked;
        }
    }

    private void ShowChooseTeam()
    {
        if (chooseTeamAsset == null)
        {
            Debug.LogError("Choose team UI asset is not assigned.");
            return;
        }

        LoadScreen(chooseTeamAsset);

        redTeamButton = ui.Q<Button>("red-team-button");
        blueTeamButton = ui.Q<Button>("blue-team-button");
        continueButton = ui.Q<Button>("continue-button");
        backButton = ui.Q<Button>("back-button");
        redTeamPlayerTag = ui.Q<Label>("red-team-player-tag");
        blueTeamPlayerTag = ui.Q<Label>("blue-team-player-tag");

        if (redTeamButton != null)
        {
            redTeamButton.clicked += OnRedTeamClicked;
        }

        if (blueTeamButton != null)
        {
            blueTeamButton.clicked += OnBlueTeamClicked;
        }

        if (continueButton != null)
        {
            continueButton.clicked += OnContinueButtonClicked;
        }

        if (backButton != null)
        {
            backButton.clicked += OnBackButtonClicked;
        }

        UpdateChooseTeamSelection();
    }

    private void LoadScreen(VisualTreeAsset screenAsset)
    {
        UnregisterCallbacks();
        ui.Clear();
        screenAsset.CloneTree(ui);
    }

    private void UnregisterCallbacks()
    {
        if (playButton != null)
        {
            playButton.clicked -= OnPlayButtonClicked;
        }

        if (quitButton != null)
        {
            quitButton.clicked -= OnQuitButtonClicked;
        }

        if (redTeamButton != null)
        {
            redTeamButton.clicked -= OnRedTeamClicked;
        }

        if (blueTeamButton != null)
        {
            blueTeamButton.clicked -= OnBlueTeamClicked;
        }

        if (backButton != null)
        {
            backButton.clicked -= OnBackButtonClicked;
        }

        if (continueButton != null)
        {
            continueButton.clicked -= OnContinueButtonClicked;
        }

        playButton = null;
        quitButton = null;
        redTeamButton = null;
        blueTeamButton = null;
        continueButton = null;
        backButton = null;
        redTeamPlayerTag = null;
        blueTeamPlayerTag = null;
        chooseTeamAnimationReset?.Pause();
        chooseTeamAnimationReset = null;
    }

    private void OnPlayButtonClicked()
    {
        ShowChooseTeam();
    }

    private void OnRedTeamClicked()
    {
        TriggerChooseTeamAnimation();
        TeamSelectionState.AssignTeams(TeamSelectionState.Team.Red);
        UpdateChooseTeamSelection();
    }

    private void OnBlueTeamClicked()
    {
        TriggerChooseTeamAnimation();
        TeamSelectionState.AssignTeams(TeamSelectionState.Team.Blue);
        UpdateChooseTeamSelection();
    }

    private void OnContinueButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnBackButtonClicked()
    {
        ShowMainMenu();
    }

    private void UpdateChooseTeamSelection()
    {
        bool isRedSelected = TeamSelectionState.Player1Team == TeamSelectionState.Team.Red;

        redTeamButton?.EnableInClassList("selected", isRedSelected);
        blueTeamButton?.EnableInClassList("selected", !isRedSelected);

        if (redTeamPlayerTag != null)
        {
            redTeamPlayerTag.text = isRedSelected ? "Player 1" : "Player 2";
        }

        if (blueTeamPlayerTag != null)
        {
            blueTeamPlayerTag.text = isRedSelected ? "Player 2" : "Player 1";
        }
    }

    private void TriggerChooseTeamAnimation()
    {
        redTeamPlayerTag?.AddToClassList("animate-swap");
        blueTeamPlayerTag?.AddToClassList("animate-swap");

        chooseTeamAnimationReset?.Pause();
        chooseTeamAnimationReset = ui.schedule.Execute(RemoveChooseTeamAnimationClasses).StartingIn(220);
    }

    private void RemoveChooseTeamAnimationClasses()
    {
        redTeamPlayerTag?.RemoveFromClassList("animate-swap");
        blueTeamPlayerTag?.RemoveFromClassList("animate-swap");
        chooseTeamAnimationReset = null;
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
