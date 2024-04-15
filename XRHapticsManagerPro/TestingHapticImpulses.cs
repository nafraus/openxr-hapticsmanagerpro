using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.XR.Interaction.Toolkit;

public class TestingHapticImpulses : MonoBehaviour
{
    public List<FloatHapticDataSO> floatHapticData = new List<FloatHapticDataSO>();
    public List<CurveHapticDataSO> curveHapticData = new List<CurveHapticDataSO>();

    public GenericHapticData hapticSingle;
    [Button]
    public void SendImpulse()
    {
        foreach(var test in floatHapticData)
        {
            XRHapticsManagerPro.Instance.SendHapticImpulse(test.GenerateImpulse(XRHapticsManagerPro.Instance), XRHapticControllerSpecifier.both);
        }
        foreach(var test in curveHapticData)
        {
            XRHapticsManagerPro.Instance.SendHapticImpulse(test.GenerateImpulse(XRHapticsManagerPro.Instance), XRHapticControllerSpecifier.both);
        }
    }
}
