using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;

/// <summary>
/// A generic class for handling memorable objects.
/// </summary>
/// <typeparam name="T">The type of the memorable object.</typeparam>
public abstract class Memorable<T> : MonoBehaviour
{
    [SerializeField] private string pathInDb;

    /// <summary>
    /// A stack containing the memory of memorable objects.
    /// </summary>
    protected Stack<T> memory = new Stack<T>();

    private void OnEnable()
    {
        if (SnapshotManager.Instance == null)
        {
            EventManager.StartListening(CoreEventCollection.SnapshotManagerReady, RegisterToManager);
        }
        else
        {
            RegisterToManager();
        }
    }

    private void RegisterToManager(object input = null)
    {
        EventManager.StopListening(CoreEventCollection.SnapshotManagerReady, RegisterToManager);
        SnapshotManager.Instance.RegisterMemorable(this);
        Initialize();
    }

    private void OnDisable()
    {
        SnapshotManager.Instance.RemoveMemorable(this);
    }

    /// <summary>
    /// Initializes the memorable object.
    /// </summary>
    public void Initialize()
    {
        if (!SnapshotManager.Instance.ActiveSnapshot.IsNull())
        {
            LoadActiveSnapshot();
        }
    }

    /// <summary>
    /// Takes a snapshot of the memorable object.
    /// </summary>
    /// <param name="snapshot">The snapshot to take.</param>
    public abstract void TakeSnapshot(Snapshot snapshot);

    /// <summary>
    /// Loads the active snapshot.
    /// </summary>
    public void LoadActiveSnapshot()
    {
        LoadSnapshot(SnapshotManager.Instance.ActiveSnapshot);
    }

    /// <summary>
    /// Loads a snapshot based on the specified Snapshot object.
    /// </summary>
    /// <param name="snapshot">The Snapshot object to load the snapshot from.</param>
    protected abstract void LoadSnapshot(Snapshot snapshot);

    /// <summary>
    /// Loads a snapshot based on the specified offset.
    /// </summary>
    /// <param name="offset">The offset to load the snapshot from.</param>
    public virtual void LoadSnapshot(int offset)
    {
        for (int i = 0; i < offset; i++)
        {
            memory.Pop();
        }
    }

    /// <summary>
    /// Checks if there is sufficient memory for the specified offset.
    /// </summary>
    /// <param name="offset">The offset to check.</param>
    /// <returns>True if there is sufficient memory, otherwise false.</returns>
    protected bool HasSufficientMemory(int offset)
    {
        if (offset >= memory.Count)
        {
            memory.Clear();
            return false;
        }
        return true;
    }
}