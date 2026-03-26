using System;
using System.Threading;
using UnityEngine;

public class HM : MonoBehaviour
{
    private const int MaxHapticDevices = 16;

    // Plugin import
    private IntPtr hapticPlugin;

    private Thread hapticThread;
    private volatile bool isHapticThreadRunning; // volatile to ensure visibility across threads
    private readonly object stateLock = new();

    private int hapticDevicesDetected;

    // Haptic workspace
    public float workspace = 1500.0f;

    // Position [m] of each haptic device
    private readonly Vector3[] positions = new Vector3[MaxHapticDevices];
    private readonly Vector3[] appliedForces = new Vector3[MaxHapticDevices];

    void Start()
    {
        // Initialization of Haptic Plugin
        hapticPlugin = HapticPluginImport.CreateHapticDevices();
        hapticDevicesDetected = HapticPluginImport.GetHapticsDetected(hapticPlugin);
        if (hapticDevicesDetected > 0)
            Debug.Log("Haptic Devices Found: " + HapticPluginImport.GetHapticsDetected(hapticPlugin).ToString());
        else
        {
            Debug.Log("Haptic Devices cannot be found");
            Application.Quit();
        }

        // Setting the haptic thread
        isHapticThreadRunning = true;
        hapticThread = new Thread(HapticThread)
        {
            Priority = System.Threading.ThreadPriority.AboveNormal,
            IsBackground = true
        };
        hapticThread.Start();
    }

    void OnDestroy()
    {
        EndHapticThread();
        HapticPluginImport.DeleteHapticDevices(hapticPlugin);
        Debug.Log("Application ended correctly");
    }

    // Thread for haptic device handling
    void HapticThread()
    {
        while (isHapticThreadRunning)
        {
            for (int i = 0; i < hapticDevicesDetected; i++)
            {
                // Get haptic positions and convert them into scene positions
                Vector3 position = workspace * HapticPluginImport.GetHapticsPositions(hapticPlugin, i);
                Vector3 forceToApply;

                lock (stateLock)
                {
                    positions[i] = position;
                    forceToApply = appliedForces[i];
                }

                HapticPluginImport.SetHapticsForce(hapticPlugin, i, forceToApply);
                HapticPluginImport.UpdateHapticDevices(hapticPlugin, i);
            }
        }
    }

    void EndHapticThread()
    {
        isHapticThreadRunning = false;
        if (hapticThread == null)
            return;

        const int timeoutMs = 2000;
        if (!hapticThread.Join(timeoutMs))
            Debug.LogWarning("Haptic thread did not terminate within the timeout.");

        hapticThread = null;
    }

    public Vector3 GetPosition(int numHapDev)
    {
        lock (stateLock)
            return positions[numHapDev];
    }

    public void UpdateCollisionState(int numHapDev, Vector3 totalForce)
    {
        lock (stateLock)
            appliedForces[numHapDev] = totalForce;
    }

    public float GetHapticDeviceInfo(int numHapDev, int parameter)
    {
        // Haptic info variables
        // 0 - m_maxLinearForce -> 8
        // 1 - m_maxAngularTorque
        // 2 - m_maxGripperForce 
        // 3 - m_maxLinearStiffness -> 3000
        // 4 - m_maxAngularStiffness
        // 5 - m_maxGripperLinearStiffness;
        // 6 - m_maxLinearDamping -> 20
        // 7 - m_maxAngularDamping
        // 8 - m_maxGripperAngularDamping

        if (parameter < 0 || parameter > 8)
            parameter = 0;

        return (float)HapticPluginImport.GetHapticsDeviceInfo(hapticPlugin, numHapDev, parameter);
    }

    public Vector3 GetVelocity(int numHapDev)
    {
        return HapticPluginImport.GetHapticsLinearVelocity(hapticPlugin, numHapDev);
    }
}
