using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DigitalRuby.Tween;
using UnityEngine.Playables;

public class ExitAttemptLogic : MonoBehaviour
{
    public PlayableDirector timeline;
    public CinemachineVirtualCamera virtualCam;
    [SerializeField] Conversation inquiry1;
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

        // System.Action<ITween<float>> BandInCallBack = (t) =>
        // {
        //     noise.m_AmplitudeGain = t.CurrentValue;
        // };

        // // completion defaults to null if not passed in
        // gameObject.Tween("BandIn", 1.0f, 0.0f, 2.0f, TweenScaleFunctions.CubicEaseInOut, BandInCallBack);

        timeline.Play();
    }

}
