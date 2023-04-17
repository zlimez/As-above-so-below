using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures;
using Algorithms;
using System;

namespace Chronellium.Hacking.UI {
    public class HopDestinationUI : MonoBehaviour
    {
        private static OrderedHackable<float> leftWaypointHackable = new OrderedHackable<float>(null, 180);
        [SerializeField] private MainCamera mainCamera;
        [SerializeField] private float heightFrameBuffer = 2.0f, widthFrameBuffer = 2.0f;
        [SerializeField] private HackableManager hackableManager;
        [SerializeField] private HackModeMenu hackModeMenu;
        [SerializeField] private HackableUI[] hackableUis;
        // Next goes in counter-clockwise direction
        private CircularList<OrderedHackable<float>> hoppableHackables;
        [SerializeField] private Hackable focusedHackable;
        private bool isRotatingReachableHackables = false;
        private KeyCode keyUsedForAdjHop;
        private bool hackscapeActivated = false;
        private bool focusedHackableChanged = false;

        void OnEnable() {
            hackableManager.OnActivated += DisplayHackableUIs;
            hackableManager.OnDeactivated += HideHackableUIs;
        }

        void OnDisable() {
            hackableManager.OnActivated -= DisplayHackableUIs;
            hackableManager.OnDeactivated -= HideHackableUIs;
        }

        private void DisplayHackableUIs() {
            hackscapeActivated = true;
            Array.ForEach(hackableUis, hackableUI => hackableUI.Display());
        }

        private void HideHackableUIs() {
            hackscapeActivated = false;
            focusedHackable.OnUnfocused?.Invoke();
            Array.ForEach(hackableUis, hackableUI => hackableUI.Hide());
        }

        void InitFirstBase() {
            focusedHackable = hackableManager.BasedHackable;
            OrderHoppableHackables(focusedHackable);
            focusedHackable.OnFocused?.Invoke();
        }

        private void OrderHoppableHackables(Hackable newBasedHackable) {
            var orderedStaticAdjHackables = newBasedHackable.OrderedStaticAdjHackables;
            var orderedDynamicAdjHackables = newBasedHackable.OrderDynamicAdjHackables();
            hoppableHackables = new CircularList<OrderedHackable<float>>(Tools.Merge(orderedStaticAdjHackables, orderedDynamicAdjHackables));
        }

        private bool IsHopTriggerKey => keyUsedForAdjHop == KeyCode.RightArrow || keyUsedForAdjHop == KeyCode.LeftArrow;

        void Update() {
            if (Input.GetKeyDown(KeyCode.H)) {
                if (hackscapeActivated) {
                    hackableManager.DeactivateHackscape();
                } else {
                    hackableManager.ActivateHackscape();
                    InitFirstBase();
                }
                return;
            }

            if (!hackscapeActivated) return;

            // Submit should always act on focused
            if (Input.GetButtonDown("Submit")) {
                // Dummy character
                keyUsedForAdjHop = KeyCode.Asterisk;
                isRotatingReachableHackables = false;
                if (focusedHackable.HackableState == Hackable.State.alien) {
                    hackModeMenu.OpenMenu(focusedHackable, hackableManager.BasedHackable);
                } else if (focusedHackable.HackableState == Hackable.State.friendly) {
                    focusedHackable.Base();
                } else if (focusedHackable.HackableState == Hackable.State.based) {
                    focusedHackable.TakeControl();
                }
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                if (keyUsedForAdjHop != KeyCode.LeftArrow && isRotatingReachableHackables) return;
                focusedHackable.OnBased -= OrderHoppableHackables;
                focusedHackable.OnUnfocused?.Invoke();
                if (keyUsedForAdjHop == KeyCode.LeftArrow) {
                    focusedHackable = hackableManager.BasedHackable;
                    focusedHackableChanged = true;
                    isRotatingReachableHackables = false;
                    keyUsedForAdjHop = KeyCode.Asterisk;
                } else if (!isRotatingReachableHackables) {
                    keyUsedForAdjHop = KeyCode.RightArrow;
                    isRotatingReachableHackables = true;
                    focusedHackable = hoppableHackables.Get(0).WrappedHackable;
                    focusedHackableChanged = true;
                } else {
                    Debug.LogError("Guard should not have allowed fallthrough");
                }
                focusedHackable.OnBased += OrderHoppableHackables;
                focusedHackable.OnFocused?.Invoke();
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (keyUsedForAdjHop != KeyCode.RightArrow && isRotatingReachableHackables) return;
                focusedHackable.OnBased -= OrderHoppableHackables;
                focusedHackable.OnUnfocused?.Invoke();
                if (keyUsedForAdjHop == KeyCode.RightArrow) {
                    focusedHackable = hackableManager.BasedHackable;
                    focusedHackableChanged = true;
                    isRotatingReachableHackables = false;
                    keyUsedForAdjHop = KeyCode.Asterisk;
                } else if (!isRotatingReachableHackables) {
                    keyUsedForAdjHop = KeyCode.LeftArrow;
                    isRotatingReachableHackables = true;
                    focusedHackable = hoppableHackables.Get(0).WrappedHackable;
                    focusedHackable = hoppableHackables.Get(Search.BinarySearchClosest(hoppableHackables.Items, leftWaypointHackable)).WrappedHackable;
                    focusedHackableChanged = true;
                } else {
                    Debug.LogError("Guard should not have allowed fallthrough");
                }
                focusedHackable.OnBased += OrderHoppableHackables;
                focusedHackable.OnFocused?.Invoke();
            } else if (isRotatingReachableHackables && Input.GetKeyDown(KeyCode.DownArrow)) {
                if (hoppableHackables.Count == 1 || !IsHopTriggerKey) return;
                focusedHackable.OnBased -= OrderHoppableHackables;
                focusedHackable.OnUnfocused?.Invoke();
                if (keyUsedForAdjHop == KeyCode.LeftArrow) {
                    focusedHackable = hoppableHackables.Next().WrappedHackable;
                    focusedHackableChanged = true;
                } else {
                    focusedHackable = hoppableHackables.Previous().WrappedHackable;
                    focusedHackableChanged = true;
                }
                focusedHackable.OnBased += OrderHoppableHackables;
                focusedHackable.OnFocused?.Invoke();
            } else if (isRotatingReachableHackables && Input.GetKeyDown(KeyCode.UpArrow)) {
                if (hoppableHackables.Count == 1 || !IsHopTriggerKey) return;
                focusedHackable.OnBased -= OrderHoppableHackables;
                focusedHackable.OnUnfocused?.Invoke();
                if (keyUsedForAdjHop == KeyCode.LeftArrow) {
                    focusedHackable = hoppableHackables.Previous().WrappedHackable;
                    focusedHackableChanged = true;
                } else {
                    focusedHackable = hoppableHackables.Next().WrappedHackable;
                    focusedHackableChanged = true;
                }
                focusedHackable.OnBased += OrderHoppableHackables;
                focusedHackable.OnFocused?.Invoke();
            }

            if (focusedHackableChanged){
                Debug.Log("Setting new cam followed hackable to " + focusedHackable.name);
                mainCamera.SetFollowTransform(focusedHackable.NetworkCenter, focusedHackable.SignalDiameter + widthFrameBuffer, focusedHackable.SignalDiameter + heightFrameBuffer);
                focusedHackableChanged = false;
            } 
        }
    }
}
