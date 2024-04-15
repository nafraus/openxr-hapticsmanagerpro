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
   
    //Sending haptic impulse *maybe* needs to calculate and apply collective impulses
    //Could change fixedupdate to be own coroutine
    private void FixedUpdate()
    {
        //All haptic impulses are subscribed to this action, and update their time

        ProcessImpulses();

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
                //modify this to account for multipliers
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
        //TODO figure out how to do pointers in C# to optimize removal?

        //There is definetly a way to optimize this
        //This might remove any instance of data from the impulses. Might not cause errors, but will be suboptimal
        foreach(var pair in ActiveHapticImpulses)
        {
            List<HapticImpulse> impulses = pair.Value;
            if (impulses.Contains(data))
            {
                //Garbage collecter wil destroy the class for me
                impulses.Remove(data);
            }
        }

        AddTimeStepAction -= data.AddTime;
    }

    #region Sending Haptic Impulses from Haptic Data objects. Add new functions for each type
    public void SendHapticImpulse(HapticImpulse impulse, XRDirectInteractor interactor)
    {
        ActiveHapticImpulses[interactor].Add(impulse);
    }
    #region Adapters
    public void SendHapticImpulse(HapticImpulse impulse, XRBaseInteractable interactable)
    {
        //TODO Add optional parameter for number of interactors to apply to
        List<IXRSelectInteractor> interactors = interactable.interactorsSelecting;

        foreach (IXRSelectInteractor interactor in interactors)
        {
            SendHapticImpulse(impulse, interactor.transform.GetComponent<XRDirectInteractor>());
        }
    }

    public void SendHapticImpulse(HapticImpulse impulse, XRBaseController controller)
    {
        SendHapticImpulse(impulse, controller.GetComponent<XRDirectInteractor>());
    }

    public void SendHapticImpulse(HapticImpulse impulse, XRHapticControllerSpecifier type)
    {
        switch (type)
        {
            case XRHapticControllerSpecifier.left:
                SendHapticImpulse(impulse, leftInteractor);
                break;
            case XRHapticControllerSpecifier.right:
                SendHapticImpulse(impulse, rightInteractor);
                break;
            case XRHapticControllerSpecifier.both:
                SendHapticImpulse(impulse, leftInteractor);
                SendHapticImpulse(impulse, rightInteractor);
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


