using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameHUD : MonoBehaviour
{
    [SerializeField] private int matchDurationSeconds = 180;
    [SerializeField] private bool startCountdownOnEnable = true;

    private Label player1ScoreLabel;
    private Label player2ScoreLabel;
    private Label timerLabel;
    private Label player1TeamLabel;
    private Label player2TeamLabel;
    private VisualElement player1Card;
    private VisualElement player2Card;

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
        player1Card = root.Q<VisualElement>("player-1-card");
        player2Card = root.Q<VisualElement>("player-2-card");

        ApplyTeamStyles();
        ResetHUD();
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
        }
    }

    public void SetScore(int player1, int player2)
    {
        player1Score = Mathf.Max(0, player1);
        player2Score = Mathf.Max(0, player2);
        UpdateScoreLabels();
    }

    public void AddScorePlayer1(int amount = 1)
    {
        SetScore(player1Score + amount, player2Score);
    }

    public void AddScorePlayer2(int amount = 1)
    {
        SetScore(player1Score, player2Score + amount);
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
        bool isPlayer1Red = TeamSelectionState.Player1Team == TeamSelectionState.Team.Red;

        player1Card?.EnableInClassList("player-red", isPlayer1Red);
        player1Card?.EnableInClassList("player-blue", !isPlayer1Red);
        player2Card?.EnableInClassList("player-red", !isPlayer1Red);
        player2Card?.EnableInClassList("player-blue", isPlayer1Red);

        if (player1TeamLabel != null)
        {
            player1TeamLabel.text = isPlayer1Red ? "RED" : "BLUE";
        }

        if (player2TeamLabel != null)
        {
            player2TeamLabel.text = isPlayer1Red ? "BLUE" : "RED";
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
}
