using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DigitalRuby.Tween;
using UnityEngine.Playables;
using Chronellium.EventSystem;

public class ExitAttemptLogic : MonoBehaviour
{
    public CinemachineBasicMultiChannelPerlin noise;
    public GameObject player;
    public Transform UnderWaterStartPos;
    public PlayableDirector timeline;
    public CinemachineVirtualCamera virtualCam;
    [SerializeField] Conversation inquiry1;
    private void Start()
    {
        StartChase();
        EventManager.StartListening(StaticEvent.Core_ResetPuzzle, ResetPuzzle);
    }

    private void ResetPuzzle(object input = null)
    {
        Debug.Log("Reseting puzzle");
        player.transform.position = UnderWaterStartPos.position;

        System.Action<ITween<float>> BandInCallBack = (t) =>
        {
            noise.m_AmplitudeGain = t.CurrentValue;
        };

        // // completion defaults to null if not passed in
        gameObject.Tween("Remove camera shake", noise.m_AmplitudeGain, 0.0f, 2.0f, TweenScaleFunctions.CubicEaseInOut, BandInCallBack);
    }
    public void StartChase()
    {
        EventManager.InvokeEvent(StaticEvent.Core_LowBreath);

    }

    // Update is called once per frame
    public void StartBreathTimer()
    {
        BreathTimer.Instance.gameObject.SetActive(true);
    }

    public void startOhShtDialog()
    {
        DialogueManager.Instance.StartConversation(inquiry1);
    }

    void OnTriggerEnter(Collider other)
    {
        CinemachineBasicMultiChannelPerlin noise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();


        timeline.Play();
    }

}
