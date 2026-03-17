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

    // Haptic workspace
    public float workspace = 100.0f;
    // Number of haptic devices
    private int hapticDevicesDetected;

    // Position [m] of each haptic device
    private readonly Vector3[] positions = new Vector3[MaxHapticDevices];
    private readonly bool[] deviceIsColliding = new bool[MaxHapticDevices];
    private readonly Vector3[] desiredPositions = new Vector3[MaxHapticDevices];
    private readonly float[] springConstants = new float[MaxHapticDevices];
    private readonly float[] linearDampings = new float[MaxHapticDevices];

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

    void Update()
    {
        // Exit application
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
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
                // get haptic positions and convert them into scene positions
                Vector3 position = workspace * HapticPluginImport.GetHapticsPositions(hapticPlugin, i);
                bool isColliding;
                Vector3 desiredPosition;
                float springConstant;
                float linearDamping;

                lock (stateLock)
                {
                    positions[i] = position;
                    isColliding = deviceIsColliding[i];
                    desiredPosition = desiredPositions[i];
                    springConstant = springConstants[i];
                    linearDamping = linearDampings[i];
                }

                /* if (isColliding)
                    SetForceByDesiredPosition(i, desiredPosition, springConstant, linearDamping);
                else
                    ClearForces(i); */

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

    public void UpdateCollisionState(int numHapDev, bool isColliding, Vector3 contactPosition, float springConstant, float linearDamping)
    {
        lock (stateLock)
        {
            deviceIsColliding[numHapDev] = isColliding;
            desiredPositions[numHapDev] = contactPosition;
            springConstants[numHapDev] = springConstant;
            linearDampings[numHapDev] = linearDamping;
        }
    }

    public float GetHapticDeviceInfo(int numHapDev, int parameter)
    {
        // Haptic info variables
        // 0 - m_maxLinearForce
        // 1 - m_maxAngularTorque
        // 2 - m_maxGripperForce 
        // 3 - m_maxLinearStiffness
        // 4 - m_maxAngularStiffness
        // 5 - m_maxGripperLinearStiffness;
        // 6 - m_maxLinearDamping
        // 7 - m_maxAngularDamping
        // 8 - m_maxGripperAngularDamping

        if (parameter < 0 || parameter > 8)
            parameter = 0;

        return (float)HapticPluginImport.GetHapticsDeviceInfo(hapticPlugin, numHapDev, parameter);
    }

    private void SetForceByDesiredPosition(int numHapDev, Vector3 desiredPosition, float springConstant, float linearDamping)
    {
        // Vector3 forceSpring = stiffness * positionError;
        // Vector3 forceDamping = linearDamping * velocityError;

        Vector3 totalForce = Vector3.zero;// forceSpring + forceDamping;
        HapticPluginImport.SetHapticsForce(hapticPlugin, numHapDev, totalForce);
        // compute linear spring force
        /* Vector3 direction = desiredPosition - position[hapDevNum];
        Vector3 forceField = stiffness * direction;

        // compute linear damping force
        Vector3 linearVelocity = HapticPluginImport.GetHapticsLinearVelocity(hapticPlugin, hapDevNum);
        Vector3 forceDamping = -linearDamping * linearVelocity;

        // send the combined linear force to the haptic device
        Vector3 totalForce = forceField + forceDamping;
        HapticPluginImport.SetHapticsForce(hapticPlugin, hapDevNum, totalForce);*/
    }

    private void ClearForces(int numHapDev)
    {
        HapticPluginImport.SetHapticsForce(hapticPlugin, numHapDev, Vector3.zero);
    }
}
