using UnityEngine;

public class TeamAssignmentApplier : MonoBehaviour
{
    private const string RedHapticObjectName = "HapticRed";
    private const string BlueHapticObjectName = "HapticBlue";

    [SerializeField] private Material redTeamMaterial;
    [SerializeField] private Material blueTeamMaterial;

    private void Awake()
    {
        ApplyTeamSetup(RedHapticObjectName, redTeamMaterial, TeamSelectionState.RedDeviceIndex);
        ApplyTeamSetup(BlueHapticObjectName, blueTeamMaterial, TeamSelectionState.BlueDeviceIndex);
    }

    private void ApplyTeamSetup(string rootObjectName, Material teamMaterial, int deviceIndex)
    {
        GameObject teamRoot = GameObject.Find(rootObjectName);

        if (teamRoot == null)
        {
            Debug.LogWarning("Could not find team root: " + rootObjectName);
            return;
        }

        ApplyDeviceAssignment(teamRoot, deviceIndex);
    }

    private void ApplyDeviceAssignment(GameObject teamRoot, int deviceIndex)
    {
        IHIP ihip = teamRoot.GetComponentInChildren<IHIP>(true);
        if (ihip != null)
        {
            ihip.numHapDev = deviceIndex;
        }

        HapticInteractionPoint interactionPoint = teamRoot.GetComponentInChildren<HapticInteractionPoint>(true);
        if (interactionPoint != null)
        {
            interactionPoint.hapticDevice = deviceIndex;
        }

        if (ihip == null && interactionPoint == null)
        {
            Debug.LogWarning("Could not find a haptic control component under " + teamRoot.name);
        }
    }
}
