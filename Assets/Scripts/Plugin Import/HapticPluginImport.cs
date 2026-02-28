using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class HapticPluginImport : MonoBehaviour {

#if UNITY_STANDALONE_WIN
    private const string HapticDLL = "C++ DLL for Unity";
#elif UNITY_STANDALONE_OSX
    private const string HapticDLL = "libGodot_Haptic_Plugin";
#else
    private const string HapticDLL = "libGodot_Haptic_Plugin"; // Default to macOS
#endif

    // Constructor
    [DllImport(HapticDLL)]
    public static extern IntPtr CreateHapticDevices();

    // Destructor
    [DllImport(HapticDLL)]
    public static extern void DeleteHapticDevices(IntPtr hapticPlugin);

    // get haptic device information
    [DllImport(HapticDLL)]
    public static extern int GetHapticsDetected(IntPtr hapticPlugin);

    // get haptic device information
    [DllImport(HapticDLL)]
    public static extern double GetHapticsDeviceInfo(IntPtr hapticPlugin, int numHapDev, int var);

    [DllImport(HapticDLL)]
    public static extern Vector3 GetHapticsPositions(IntPtr hapticPlugin, int numHapDev);

    [DllImport(HapticDLL)]
    public static extern Quaternion GetHapticsOrientations(IntPtr hapticPlugin, int numHapDev);

    [DllImport(HapticDLL)]
    public static extern double GetHapticsGripperAngle(IntPtr hapticPlugin, int numHapDev);

    [DllImport(HapticDLL)]
    public static extern Vector3 GetHapticsLinearVelocity(IntPtr hapticPlugin, int numHapDev);

    [DllImport(HapticDLL)]
    public static extern Vector3 GetHapticsAngularVelocity(IntPtr hapticPlugin, int numHapDev);

    [DllImport(HapticDLL)]
    public static extern double GetHapticsGripperAngularVelocity(IntPtr hapticPlugin, int numHapDev);

    [DllImport(HapticDLL)]
    public static extern bool GetHapticsButtons(IntPtr hapticPlugin, int numHapDev, int button);

    // set haptic device forcefeedback
    [DllImport(HapticDLL)]
    public static extern void SetHapticsForce(IntPtr hapticPlugin, int numHapDev, Vector3 sentForce);

    [DllImport(HapticDLL)]
    public static extern void SetHapticsTorque(IntPtr hapticPlugin, int numHapDev, Vector3 sentTorque);

    [DllImport(HapticDLL)]
    public static extern void SetHapticsGripperForce(IntPtr hapticPlugin, int numHapDev, double sentGripperForce);

    // main haptics rendering loop
    [DllImport(HapticDLL)]
    public static extern void UpdateHapticDevices(IntPtr hapticPlugin, int numHapDev);
}
