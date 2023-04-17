using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saveable;
// TESTCODE
using KinematicCharacterController;

public class TransformMemorable : Memorable<SaveableTransform>
{
    // Should always be matching pairs starting from stack top with SnapshotManager.memorables
    // private Stack<SaveableTransform> memory = new Stack<SaveableTransform>()

    public override void TakeSnapshot(Snapshot snapshot)
    {
        memory.Push(new SaveableTransform(transform));
        Debug.Log($"{name} position {transform.position} saved");
    }

    public override void LoadSnapshot(int offset)
    {
        if (!HasSufficientMemory(offset)) LoadActiveSnapshot();
        base.LoadSnapshot(offset);

        SaveableTransform targetTransform = memory.Peek();
        Debug.Log($"{name} position {targetTransform.position}");

        // TESTCODE: Should have a unified scheme for all transform
        if (CompareTag("Player"))
        {
            Debug.Log("Setting scientist position");
            GetComponent<KinematicCharacterMotor>().SetPositionAndRotation(targetTransform.position, targetTransform.rotation, true);
            return;
        }

        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
        transform.localScale = targetTransform.scale;
    }

    protected override void LoadSnapshot(Snapshot snapshot)
    {
        // TODO: Get from db
    }
}
