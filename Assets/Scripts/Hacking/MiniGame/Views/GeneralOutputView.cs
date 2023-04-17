using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GeneralOutputView : PipeView
{
    [SerializeField] protected VirusBase target;
    [SerializeField] private SpriteRenderer arrow;
    private ParticleSystem bustedEmitter;
    protected OutputNode outputNode;

    protected virtual void Awake() {
        outputNode = outputNode ?? new OutputNode(target);
        bustedEmitter = GetComponent<ParticleSystem>();
        bustedEmitter.Stop();
    }

    void OnEnable() {
        RegisterDestroyResponse(BustNode);
    }

    void OnDisable() {
        UnregisterDestroyResponse(BustNode);
    }

    public void RegisterDestroyResponse(UnityAction response) {
        // NOTE: In case Awake called after another gameoject tries to register listeners
        outputNode = outputNode ?? new OutputNode(target);
        outputNode.onDestroyed.AddListener(response);
    }

    public void UnregisterDestroyResponse(UnityAction response) {
        outputNode.onDestroyed.RemoveListener(response);
    }

    private void BustNode() {
        // TODO: Add particle effect
        arrow.color = Color.black;
        bustedEmitter.Play();
    }

    protected override void AbsorbFromUpstream(PipeView providedUpstream) {
        base.AbsorbFromUpstream(providedUpstream);

        outputNode.ParentPipe = upstream.GetPipe();
        outputNode.SetInput();
    }

    public override Pipe GetPipe() { return outputNode; }

    public override float GetStreamSpeed() { return 0; }
}
