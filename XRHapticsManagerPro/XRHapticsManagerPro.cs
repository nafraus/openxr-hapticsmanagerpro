using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class XRHapticsManagerPro : MonoBehaviour
{
    public static XRHapticsManagerPro Instance;
    private float timeStep { get => Time.fixedDeltaTime; }

    [SerializeField] private XRDirectInteractor leftInteractor;
    [SerializeField] private XRDirectInteractor rightInteractor;

    public Action<float> AddTimeStepAction;

    private Dictionary<XRDirectInteractor, List<HapticImpulse>> ActiveHapticImpulses;

    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;

        ActiveHapticImpulses = new Dictionary<XRDirectInteractor, List<HapticImpulse>>(); 
        ActiveHapticImpulses.Add(leftInteractor, new List<HapticImpulse>());
        ActiveHapticImpulses.Add(rightInteractor, new List<HapticImpulse>());
    }
   
    private void FixedUpdate()
    {
        ProcessImpulses();
        
        //All haptic impulses are subscribed to this action, and update their time
        AddTimeStepAction?.Invoke(timeStep);
    }

    private void ProcessImpulses()
    {
        foreach (var pair in ActiveHapticImpulses)
        {
            XRDirectInteractor currentInteractor = pair.Key;

            float impulseAdditives = 0;
            float impulseMultipliers = 1;

            bool doBreak = false;

            foreach (HapticImpulse data in pair.Value)
            {
                switch (data.ImpulseType)
                {
                    case XRHapticsApplyType.Additive:
                        impulseAdditives += data.Value;
                        break;
                    case XRHapticsApplyType.Multiplier:
                        impulseMultipliers *= data.Value;
                        break;
                    case XRHapticsApplyType.Override:
                        impulseAdditives = data.Value;
                        impulseMultipliers = 1;
                        doBreak = true;
                        break;
                }

                if (doBreak) break;
            }

            currentInteractor.SendHapticImpulse(Mathf.Clamp01(impulseAdditives * impulseMultipliers), timeStep);
        }
    }

    public void RemoveHaptic(HapticImpulse data)
    {
        foreach(var pair in ActiveHapticImpulses)
        {
            List<HapticImpulse> impulses = pair.Value;
            if (impulses.Contains(data))
            {
                //Garbage collecter wil destroy the class
                pair.Value.Remove(data);
            }
        }

        AddTimeStepAction -= data.AddTime;
    }

    #region Sending Haptic Impulses from Haptic Data objects. Add new functions for each type
    public void SendHapticImpulse(HapticData data, XRDirectInteractor interactor)
    {
        ActiveHapticImpulses[interactor].Add(data.GenerateImpulse(this));
    }
    #region Adapters
    public void SendHapticImpulse(HapticData data, XRBaseInteractable interactable)
    {
        List<IXRSelectInteractor> interactors = interactable.interactorsSelecting;

        foreach (IXRSelectInteractor interactor in interactors)
        {
            SendHapticImpulse(data, interactor.transform.GetComponent<XRDirectInteractor>());
        }
    }

    public void SendHapticImpulse(HapticData data, XRBaseController controller)
    {
        SendHapticImpulse(data, controller.GetComponent<XRDirectInteractor>());
    }

    public void SendHapticImpulse(HapticData data, XRHapticControllerSpecifier type)
    {
        switch (type)
        {
            case XRHapticControllerSpecifier.left:
                SendHapticImpulse(data, leftInteractor);
                break;
            case XRHapticControllerSpecifier.right:
                SendHapticImpulse(data, rightInteractor);
                break;
            case XRHapticControllerSpecifier.both:
                SendHapticImpulse(data, leftInteractor);
                SendHapticImpulse(data, rightInteractor);
                break;
        }
    }
    #endregion
    #endregion
}

public enum XRHapticControllerSpecifier
{
    left,
    right,
    both
}


