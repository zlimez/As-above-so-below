using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saveable;
using Chronellium.EventSystem;

// TESTCODE
using KinematicCharacterController;

public class KinematicTransformMemorable : Memorable<SaveableTransform>
{
    [SerializeField] protected int id;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private SaveableTransform targetTransform;

    // Should always be matching pairs starting from stack top with SnapshotManager.memorables
    // private Stack<SaveableTransform> memory = new Stack<SaveableTransform>();

    public override void TakeSnapshot(Snapshot snapshot)
    {
        SaveableTransform saveableTransform = new SaveableTransform(transform, spriteRenderer.flipX);
        TransformMemorableManager.Instance.PushToMemory(id, saveableTransform);

        Debug.Log($"{name} position {transform.position} saved");
    }

    public override void LoadSnapshot(int offset)
    {
        EventManager.StartListening(CommonEventCollection.CurtainFullyDrawn, UpdateTransform);
        memory = TransformMemorableManager.Instance.GetMemory(id);

        base.LoadSnapshot(offset);

        TransformMemorableManager.Instance.UpdateMemory(id, memory);

        targetTransform = memory.Peek();
    }

    public void UpdateTransform(object o = null)
    {
        Debug.Log($"{name} position {targetTransform.position}");
        // transform should only be updated when the curtain is drawn
        this.GetComponent<KinematicCharacterMotor>().SetTransientPosition(targetTransform.position);
        spriteRenderer.flipX = targetTransform.scale == VectorUtils.horizontalFlipped;
        // reset
        targetTransform = new SaveableTransform();
        // unsubscribe after finishing updating
        EventManager.StopListening(CommonEventCollection.CurtainFullyDrawn, UpdateTransform);
    }

    protected override void LoadSnapshot(Snapshot snapshot)
    {
        // TODO: Get from db
    }
}
