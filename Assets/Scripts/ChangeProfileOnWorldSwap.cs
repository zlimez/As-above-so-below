using Chronellium.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChangeProfileOnWorldSwap : MonoBehaviour
{
    public Volume volumeComponent; // reference to the Volume component
    public VolumeProfile spiritProfile;
    private VolumeProfile realWorldProfile;

    void OnEnable()
    {
        EventManager.StartListening(StaticEvent.Core_SwitchToRealWorld, TurnOnRealWorld);
        EventManager.StartListening(StaticEvent.Core_SwitchToOtherWorld, TurnOnSpiritWorld);
    }

    void OnDisable()
    {
        EventManager.StopListening(StaticEvent.Core_SwitchToRealWorld, TurnOnRealWorld);
        EventManager.StopListening(StaticEvent.Core_SwitchToOtherWorld, TurnOnSpiritWorld);
    }

    private void TurnOnSpiritWorld(object input = null)
    {
        realWorldProfile = volumeComponent.profile;
        volumeComponent.profile = spiritProfile;
    }

    private void TurnOnRealWorld(object input = null)
    {
        volumeComponent.profile = realWorldProfile;
    }
}
