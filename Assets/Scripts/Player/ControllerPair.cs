using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;

public class ControllerPair : MonoBehaviour
{
    [SerializeField] private JumpAddedController normalController;
    [SerializeField] private SwimController swimController;

    void OnEnable() {
        EventManager.StartListening(StaticEvent.Core_SwitchToOtherWorld, ChangeToOther);
        EventManager.StartListening(StaticEvent.Core_SwitchToRealWorld, ChangeToReal);
    }

    void OnDisable() {
        EventManager.StopListening(StaticEvent.Core_SwitchToOtherWorld, ChangeToOther);
        EventManager.StopListening(StaticEvent.Core_SwitchToRealWorld, ChangeToReal);
    }

    private void ChangeToReal(object input = null) {
        normalController.transform.position = swimController.transform.position;
        normalController.gameObject.SetActive(true);
        swimController.gameObject.SetActive(false);
    }

    private void ChangeToOther(object input = null) {
        swimController.transform.position = normalController.transform.position;
        normalController.gameObject.SetActive(false);
        swimController.gameObject.SetActive(true);
    }
}
