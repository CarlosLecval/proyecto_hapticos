using UnityEngine;

public class TeamAssignmentApplier : MonoBehaviour
{
    [SerializeField] private Material redTeamMaterial;
    [SerializeField] private Material blueTeamMaterial;

    private void Awake()
    {
        ApplyTeamMaterial("HapticP1", TeamSelectionState.Player1Team);
        ApplyTeamMaterial("HapticP2", TeamSelectionState.Player2Team);
    }

    private void ApplyTeamMaterial(string playerRootName, TeamSelectionState.Team team)
    {
        GameObject playerRoot = GameObject.Find(playerRootName);

        if (playerRoot == null)
        {
            Debug.LogWarning("Could not find player root: " + playerRootName);
            return;
        }

        Renderer playerRenderer = playerRoot.GetComponentInChildren<Renderer>(true);

        if (playerRenderer == null)
        {
            Debug.LogWarning("Could not find renderer for player root: " + playerRootName);
            return;
        }

        Material teamMaterial = team == TeamSelectionState.Team.Red ? redTeamMaterial : blueTeamMaterial;

        if (teamMaterial == null)
        {
            Debug.LogWarning("Team material is not assigned for " + team);
            return;
        }

        playerRenderer.material = teamMaterial;
    }
}
