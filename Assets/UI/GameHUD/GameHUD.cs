using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameHUD : MonoBehaviour
{
    [SerializeField] private int matchDurationSeconds = 60;
    [SerializeField] private bool startCountdownOnEnable = true;

    private Label player1ScoreLabel;
    private Label player2ScoreLabel;
    private Label timerLabel;
    private Label player1TeamLabel;
    private Label player2TeamLabel;
    private Label winnerTitleLabel;
    private Label winnerLeftTeamLabel;
    private Label winnerRightTeamLabel;
    private Label winnerLeftScoreLabel;
    private Label winnerRightScoreLabel;
    private Button winnerMainMenuButton;
    private VisualElement player1Card;
    private VisualElement player2Card;
    private VisualElement winnerOverlay;
    private VisualElement winnerCard;

    private int player1Score;
    private int player2Score;
    private float remainingTime;
    private bool isTimerRunning;

    private void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        player1ScoreLabel = root.Q<Label>("player-1-score");
        player2ScoreLabel = root.Q<Label>("player-2-score");
        timerLabel = root.Q<Label>("timer-label");
        player1TeamLabel = root.Q<Label>("player-1-team-label");
        player2TeamLabel = root.Q<Label>("player-2-team-label");
        winnerTitleLabel = root.Q<Label>("winner-title");
        winnerLeftTeamLabel = root.Q<Label>("winner-left-team");
        winnerRightTeamLabel = root.Q<Label>("winner-right-team");
        winnerLeftScoreLabel = root.Q<Label>("winner-left-score");
        winnerRightScoreLabel = root.Q<Label>("winner-right-score");
        winnerMainMenuButton = root.Q<Button>("winner-main-menu-button");
        player1Card = root.Q<VisualElement>("player-1-card");
        player2Card = root.Q<VisualElement>("player-2-card");
        winnerOverlay = root.Q<VisualElement>("winner-overlay");
        winnerCard = root.Q<VisualElement>("winner-card");

        GoalTrigger.OnGoalScored += AddScore;
        winnerMainMenuButton?.RegisterCallback<ClickEvent>(OnWinnerMainMenuClicked);

        ApplyTeamStyles();
        ResetHUD();
    }

    private void OnDisable()
    {
        GoalTrigger.OnGoalScored -= AddScore;
        winnerMainMenuButton?.UnregisterCallback<ClickEvent>(OnWinnerMainMenuClicked);
    }

    private void Update()
    {
        if (!isTimerRunning)
        {
            return;
        }

        remainingTime = Mathf.Max(0f, remainingTime - Time.deltaTime);
        UpdateTimerLabel();

        if (remainingTime <= 0f)
        {
            isTimerRunning = false;
            ShowWinnerCard();
        }
    }

    public void SetScore(int player1, int player2)
    {
        player1Score = Mathf.Max(0, player1);
        player2Score = Mathf.Max(0, player2);
        UpdateScoreLabels();
    }

    public void AddScore(int teamNumber)
    {
        if (!isTimerRunning) return;
        if (teamNumber == 1)
        {
            SetScore(player1Score + 1, player2Score);
        }
        else if (teamNumber == 2)
        {
            SetScore(player1Score, player2Score + 1);
        }
    }

    public void SetRemainingTime(float seconds)
    {
        remainingTime = Mathf.Max(0f, seconds);
        UpdateTimerLabel();
    }

    public void ResetHUD()
    {
        player1Score = 0;
        player2Score = 0;
        remainingTime = matchDurationSeconds;
        isTimerRunning = startCountdownOnEnable;

        UpdateScoreLabels();
        UpdateTimerLabel();
        HideWinnerCard();
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    private void ApplyTeamStyles()
    {
        player1Card?.EnableInClassList("player-red", true);
        player1Card?.EnableInClassList("player-blue", false);
        player2Card?.EnableInClassList("player-red", false);
        player2Card?.EnableInClassList("player-blue", true);

        if (player1TeamLabel != null)
        {
            player1TeamLabel.text = "RED";
        }

        if (player2TeamLabel != null)
        {
            player2TeamLabel.text = "BLUE";
        }

        if (winnerLeftTeamLabel != null)
        {
            winnerLeftTeamLabel.text = "RED";
        }

        if (winnerRightTeamLabel != null)
        {
            winnerRightTeamLabel.text = "BLUE";
        }
    }

    private void UpdateScoreLabels()
    {
        if (player1ScoreLabel != null)
        {
            player1ScoreLabel.text = player1Score.ToString();
        }

        if (player2ScoreLabel != null)
        {
            player2ScoreLabel.text = player2Score.ToString();
        }

        if (winnerLeftScoreLabel != null)
        {
            winnerLeftScoreLabel.text = player1Score.ToString();
        }

        if (winnerRightScoreLabel != null)
        {
            winnerRightScoreLabel.text = player2Score.ToString();
        }
    }

    private void UpdateTimerLabel()
    {
        if (timerLabel == null)
        {
            return;
        }

        int totalSeconds = Mathf.CeilToInt(remainingTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        timerLabel.text = $"{minutes:00}:{seconds:00}";
    }

    private void ShowWinnerCard()
    {
        if (winnerOverlay == null || winnerCard == null)
        {
            return;
        }

        bool player1Wins = player1Score > player2Score;
        bool player2Wins = player2Score > player1Score;
        bool isDraw = !player1Wins && !player2Wins;

        winnerCard.EnableInClassList("player-red", false);
        winnerCard.EnableInClassList("player-blue", false);
        winnerCard.EnableInClassList("draw-state", isDraw);

        if (!isDraw)
        {
            string winningClass = player1Wins
                ? (player1Card != null && player1Card.ClassListContains("player-red") ? "player-red" : "player-blue")
                : (player2Card != null && player2Card.ClassListContains("player-red") ? "player-red" : "player-blue");

            winnerCard.AddToClassList(winningClass);
        }

        if (winnerTitleLabel != null)
        {
            winnerTitleLabel.text = isDraw
                ? "MATCH DRAW"
                : player1Wins
                    ? $"{winnerLeftTeamLabel.text} TEAM WINS"
                    : $"{winnerRightTeamLabel.text} TEAM WINS";
        }
        winnerOverlay.AddToClassList("visible");
    }

    private void HideWinnerCard()
    {
        if (winnerOverlay == null || winnerCard == null)
        {
            return;
        }

        winnerOverlay.RemoveFromClassList("visible");
        winnerCard.RemoveFromClassList("player-red");
        winnerCard.RemoveFromClassList("player-blue");
        winnerCard.RemoveFromClassList("draw-state");
    }

    private void OnWinnerMainMenuClicked(ClickEvent evt)
    {
        SceneManager.LoadScene("Menu");
    }
}
