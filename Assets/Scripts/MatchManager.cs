using UnityEngine;

public class MatchManager: MonoBehaviour
{
    [SerializeField] private int redTeamScore = 0;
    [SerializeField] private int blueTeamScore = 0;

    void OnEnable()
    {
        GoalTrigger.OnGoalScored += HandleGoalScored;
    }

    void OnDisable()
    {
        GoalTrigger.OnGoalScored -= HandleGoalScored;
    }

    private void HandleGoalScored(int teamNumber)
    {
        if (teamNumber == 1)
        {
            redTeamScore++;
            Debug.Log("Red Team Scored! Score: " + redTeamScore);
        }
        else if (teamNumber == 2)
        {
            blueTeamScore++;
            Debug.Log("Blue Team Scored! Score: " + blueTeamScore);
        }
    }
}