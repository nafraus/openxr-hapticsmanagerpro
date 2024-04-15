using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.XR.Interaction.Toolkit;

public class TestingHapticImpulses : MonoBehaviour
{
    public List<FloatHapticDataSO> floatHapticData = new List<FloatHapticDataSO>();
    public List<CurveHapticDataSO> curveHapticData = new List<CurveHapticDataSO>();
    [Button]
    public void SendImpulse()
    {
        foreach(var test in floatHapticData)
        {
            VRHapticsManager.Instance.SendHapticImpulse(test.GenerateImpulse(VRHapticsManager.Instance), XRHapticControllerSpecifier.both);
        }
        foreach(var test in curveHapticData)
        {
            VRHapticsManager.Instance.SendHapticImpulse(test.GenerateImpulse(VRHapticsManager.Instance), XRHapticControllerSpecifier.both);
        }
    }
}
