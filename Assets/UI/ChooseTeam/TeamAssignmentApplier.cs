using UnityEngine;

public class TeamAssignmentApplier : MonoBehaviour
{
    private const string RedHapticObjectName = "HapticRed";
    private const string BlueHapticObjectName = "HapticBlue";

    private void Awake()
    {
        ApplyTeamSetup(RedHapticObjectName, TeamSelectionState.RedDeviceIndex);
        ApplyTeamSetup(BlueHapticObjectName, TeamSelectionState.BlueDeviceIndex);
    }

    private void ApplyTeamSetup(string rootObjectName, int deviceIndex)
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
            ihip.numHapDev = deviceIndex;

        if (ihip == null)
            Debug.LogWarning("Could not find a haptic control component under " + teamRoot.name);
    }
}
