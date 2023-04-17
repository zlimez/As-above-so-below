using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Chronellium.EventSystem;
using Chronellium.SceneSystem;

/// <summary>
/// Manages snapshots in the game.
/// </summary>
public class SnapshotManager : MonoBehaviour
{
    public static SnapshotManager Instance;

    [SerializeField] private UnityEvent<Snapshot> _onSnapshotRequested;
    [SerializeField] private UnityEvent<int> _onSnapshotLoaded;

    private Stack<Snapshot> _snapshots = new Stack<Snapshot>();

    private int _snapshotCounter;
    private Snapshot _activeSnapshot;

    /// <summary>
    /// Indicates whether the snapshot manager is currently rewinding.
    /// </summary>
    public bool IsRewinding { get; private set; }

    /// <summary>
    /// Differentiates between rewinding and initializing from a save file.
    /// </summary>
    public bool IsForInit { get; set; }

    /// <summary>
    /// The snapshot counter.
    /// </summary>
    public int SnapshotCounter => _snapshotCounter;

    /// <summary>
    /// A stack containing snapshots.
    /// </summary>
    public Stack<Snapshot> Snapshots => _snapshots;

    /// <summary>
    /// The snapshot chronologically closest to the current game time.
    /// </summary>
    public Snapshot ActiveSnapshot => _activeSnapshot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _activeSnapshot = Snapshot.NoSnapshot;
            EventManager.InvokeEvent(CoreEventCollection.SnapshotManagerReady);
        }
    }

    /// <summary>
    /// Initializes a test snapshot with the given dummy parameters.
    /// </summary>
    /// <param name="dummyStartScene">Dummy start scene name.</param>
    /// <param name="dummyDescription">Dummy snapshot description.</param>
    /// <param name="dummySprite">Dummy snapshot sprite.</param>
    public void TestTakeSnapshot(string dummyStartScene, string dummyDescription, Sprite dummySprite)
    {
        Snapshot dummySnapshot = new Snapshot(_snapshotCounter, dummyStartScene, dummySprite, dummyDescription);
        _snapshots.Push(dummySnapshot);
        _activeSnapshot = dummySnapshot;
        _snapshotCounter++;
    }

    /// <summary>
    /// Takes a snapshot with the specified description.
    /// </summary>
    /// <param name="description">The description of the snapshot.</param>
    public void TakeSnapshot(string description)
    {
        TakeSnapshot(description, null);
    }

    /// <summary>
    /// Takes a snapshot with the specified description and screenshot.
    /// </summary>
    /// <param name="description">The description of the snapshot.</param>
    /// <param name="screenshot">The screenshot of the snapshot.</param>
    public void TakeSnapshot(string description, Sprite screenshot)
    {
        Snapshot snapshot = new Snapshot(_snapshotCounter, SceneLoader.Instance.ActiveScene.ToString(), screenshot, description);
        _activeSnapshot = snapshot;
        _snapshots.Push(snapshot);
        _onSnapshotRequested?.Invoke(snapshot);
        _snapshotCounter++;
        EventManager.InvokeEvent(CoreEventCollection.SnapshotTaken);
    }

    /// <summary>
    /// Loads a snapshot based on the specified offset.
    /// </summary>
    /// <param name="offset">The offset to load the snapshot from.</param>
    public void LoadSnapshot(int offset)
    {
        if (_snapshots.Count == 0 || _snapshots.Count < offset + 1)
        {
            return;
        }

        IsRewinding = true;

        for (int i = 0; i < offset; i++)
        {
            _snapshots.Pop();
        }

        if (_snapshots.Count == 0) return;

        _activeSnapshot = _snapshots.Peek();
        _onSnapshotLoaded?.Invoke(offset);

        if (SceneLoader.Instance.ActiveScene.ToString() != _activeSnapshot.SceneName)
        {
            EventManager.QueueEvent(CoreEventCollection.SnapshotLoaded);
            EventManager.StartListening(CoreEventCollection.SnapshotLoaded, OnCrossSceneRewind);
            SceneLoader.Instance.PrepLoadWithMaster(_activeSnapshot.SceneName);
        }
        else
        {
            _onSnapshotLoaded?.Invoke(offset);
            SceneLoader.Instance.PrepLoadWithMaster(SceneLoader.Instance.ActiveScene);
            IsRewinding = false;
        }
    }

    /// <summary>
    /// Handles cross-scene rewinding.
    /// </summary>
    private void OnCrossSceneRewind(object input = null)
    {
        IsRewinding = false;
        EventManager.StopListening(CoreEventCollection.SnapshotLoaded, OnCrossSceneRewind);
    }

    /// <summary>
    /// Registers a memorable object with the snapshot manager.
    /// </summary>
    /// <typeparam name="T">The type of the memorable object.</typeparam>
    /// <param name="memorable">The memorable object to register.</param>
    public void RegisterMemorable<T>(Memorable<T> memorable)
    {
        _onSnapshotRequested.AddListener(memorable.TakeSnapshot);
        _onSnapshotLoaded.AddListener(memorable.LoadSnapshot);
    }

    /// <summary>
    /// Removes a memorable object from the snapshot manager.
    /// </summary>
    /// <typeparam name="T">The type of the memorable object.</typeparam>
    /// <param name="memorable">The memorable object to remove.</param>
    public void RemoveMemorable<T>(Memorable<T> memorable)
    {
        _onSnapshotRequested.RemoveListener(memorable.TakeSnapshot);
        _onSnapshotLoaded.RemoveListener(memorable.LoadSnapshot);
    }
}