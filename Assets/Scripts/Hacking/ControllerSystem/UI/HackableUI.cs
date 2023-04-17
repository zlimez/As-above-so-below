using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronellium.Hacking.UI {
    public class HackableUI : MonoBehaviour
    {
        public static Color alienRed = ColorUtils.HexToRGB("D44F50", 0.5f), friendlyBlue = ColorUtils.HexToRGB("18908C", 0.5f);
        public Hackable hackable;
        public GameObject signalSphere, basedCursor;
        public SpriteRenderer sphereRenderer;
        public GameObject signalCore;
        // Used to indicate "focused" state of adjacent hackable to current base
        [SerializeField] private float oscillationAmplitude = 0.2f;
        [SerializeField] private float oscillationInterval = 1f;
        [SerializeField] private float cursorRotationInterval = 1f;
        [SerializeField] private float sphereColorChangeInterval = 0.5f;
        private IEnumerator oscillateRoutine;
        private IEnumerator cursorRotateRoutine;

        void Awake() {
            sphereRenderer = signalSphere.GetComponent<SpriteRenderer>();
        }

        void OnEnable() {
            hackable.OnBased += Base;
            hackable.OnUnbased += Unbase;
            hackable.OnFocused += StartOscillate;
            hackable.OnUnfocused += EndOscillate;
            if (!hackable.IsFriendly) hackable.OnMadeFriendly += StartSphereColorChange;
        }

        void OnDisable() {
            hackable.OnBased -= Base;
            hackable.OnUnbased -= Unbase;
            hackable.OnFocused -= StartOscillate;
            hackable.OnUnfocused -= EndOscillate;
        }

        void Start() {
            var bounds = signalSphere.GetComponent<SpriteRenderer>().sprite.bounds;
            var scaleXY = hackable.SignalDiameter / bounds.size.x;
            signalSphere.transform.localScale = new Vector3(scaleXY, scaleXY, 1);
            sphereRenderer.color = hackable.IsFriendly ? friendlyBlue : alienRed;
        }

        public void Display() {
            // TODO: Add animation zoom to show sphere
            signalSphere.SetActive(true);
            signalCore.SetActive(true);
        }

        public void Hide() {
            signalSphere.SetActive(false);
            signalCore.SetActive(false);
            basedCursor.SetActive(false);
        }

        public void Base(Hackable input = null) {
            basedCursor.SetActive(true);
            cursorRotateRoutine = RotateBaseCursor();
            StartCoroutine(cursorRotateRoutine);
        }

        private void Unbase() {
            StopCoroutine(cursorRotateRoutine);
            cursorRotateRoutine = null;
            basedCursor.SetActive(false);
            basedCursor.transform.rotation = Quaternion.identity;
        }

        public void EndOscillate() {
            if (oscillateRoutine == null) return;
            StopCoroutine(oscillateRoutine);
            oscillateRoutine = null;
            StartCoroutine(EndOscillation());
        }

        public void StartOscillate() {
            oscillateRoutine = Oscillate();
            StartCoroutine(oscillateRoutine);
        }

        public void StartSphereColorChange() {
            StartCoroutine(IndicateFriendly());
        }

        IEnumerator IndicateFriendly() {
            float timeElapsed = 0;
            while (timeElapsed < sphereColorChangeInterval) {
                timeElapsed += Time.deltaTime;
                sphereRenderer.color = ColorUtils.CubicLerpColor(alienRed, friendlyBlue, timeElapsed / sphereColorChangeInterval);
                yield return null;
            }
        }

        IEnumerator RotateBaseCursor() {
            float timeElapsed = 0;
            Vector3 initialRotation = basedCursor.transform.localRotation.eulerAngles;
            Vector3 fullRotation = new Vector3(initialRotation.x, initialRotation.y, 360);
            while (true) {
                timeElapsed += Time.deltaTime;
                timeElapsed = timeElapsed > cursorRotationInterval ? timeElapsed - cursorRotationInterval : timeElapsed;
                basedCursor.transform.localRotation = Quaternion.Euler(Vector3.Lerp(initialRotation, fullRotation, timeElapsed / cursorRotationInterval));
                yield return null;
            }
        }

        IEnumerator EndOscillation() {
            float timeElapsed = 0;
            Vector3 initialScale = signalCore.transform.localScale;
            Vector3 targetScale = Vector3.one;
            float intervalRemainder = Mathf.Abs((targetScale - initialScale).x) / oscillationAmplitude * oscillationInterval;
            while (timeElapsed < intervalRemainder) {
                timeElapsed += Time.deltaTime;
                signalCore.transform.localScale = VectorUtils.CubicLerpVector(initialScale, targetScale, timeElapsed / intervalRemainder);
                yield return null;
            }
        }

        IEnumerator Oscillate() {
            float timeElapsed = 0;
            Vector3 initialScale = signalCore.transform.localScale;
            Vector3 amplitude = Vector3.one * oscillationAmplitude;
            while (true) {
                timeElapsed += Time.deltaTime;
                timeElapsed = timeElapsed > oscillationInterval ? timeElapsed - oscillationInterval : timeElapsed;
                signalCore.transform.localScale = VectorUtils.SinLerpVector(initialScale, amplitude, timeElapsed / oscillationInterval);
                yield return null;
            }
        }
    }
}
