using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class HapticPluginImport : MonoBehaviour {

    // Constructor
    [DllImport("libGodot_Haptic_Plugin")]
    public static extern IntPtr CreateHapticDevices();

    // Destructor
    [DllImport("libGodot_Haptic_Plugin")]
    public static extern void DeleteHapticDevices(IntPtr hapticPlugin);

    // get haptic device information
    [DllImport("libGodot_Haptic_Plugin")]
    public static extern int GetHapticsDetected(IntPtr hapticPlugin);

    // get haptic device information
    [DllImport("libGodot_Haptic_Plugin")]
    public static extern double GetHapticsDeviceInfo(IntPtr hapticPlugin, int numHapDev, int var);

    [DllImport("libGodot_Haptic_Plugin")]
    public static extern Vector3 GetHapticsPositions(IntPtr hapticPlugin, int numHapDev);

    [DllImport("libGodot_Haptic_Plugin")]
    public static extern Quaternion GetHapticsOrientations(IntPtr hapticPlugin, int numHapDev);

    [DllImport("libGodot_Haptic_Plugin")]
    public static extern double GetHapticsGripperAngle(IntPtr hapticPlugin, int numHapDev);

    [DllImport("libGodot_Haptic_Plugin")]
    public static extern Vector3 GetHapticsLinearVelocity(IntPtr hapticPlugin, int numHapDev);

    [DllImport("libGodot_Haptic_Plugin")]
    public static extern Vector3 GetHapticsAngularVelocity(IntPtr hapticPlugin, int numHapDev);

    [DllImport("libGodot_Haptic_Plugin")]
    public static extern double GetHapticsGripperAngularVelocity(IntPtr hapticPlugin, int numHapDev);

    [DllImport("libGodot_Haptic_Plugin")]
    public static extern bool GetHapticsButtons(IntPtr hapticPlugin, int numHapDev, int button);

    // set haptic device forcefeedback
    [DllImport("libGodot_Haptic_Plugin")]
    public static extern void SetHapticsForce(IntPtr hapticPlugin, int numHapDev, Vector3 sentForce);

    [DllImport("libGodot_Haptic_Plugin")]
    public static extern void SetHapticsTorque(IntPtr hapticPlugin, int numHapDev, Vector3 sentTorque);

    [DllImport("libGodot_Haptic_Plugin")]
    public static extern void SetHapticsGripperForce(IntPtr hapticPlugin, int numHapDev, double sentGripperForce);

    // main haptics rendering loop
    [DllImport("libGodot_Haptic_Plugin")]
    public static extern void UpdateHapticDevices(IntPtr hapticPlugin, int numHapDev);
}
