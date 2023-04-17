using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Represents a snapshot with an ID, scene name, screenshot, and description.
/// </summary>
public struct Snapshot
{
    /// <summary>
    /// Represents an empty snapshot with no data.
    /// </summary>
    public static Snapshot NoSnapshot { get; private set; }

    private int _id;

    /// <summary>
    /// The name of the scene associated with the snapshot.
    /// </summary>
    public string SceneName { get; private set; }

    /// <summary>
    /// The screenshot associated with the snapshot.
    /// </summary>
    public Sprite Screenshot { get; private set; }

    /// <summary>
    /// A description for the snapshot.
    /// </summary>
    public string Description { get; private set; }

    static Snapshot()
    {
        NoSnapshot = new Snapshot(-1, "EmptyScene", null, "");
    }

    /// <summary>
    /// Checks if the snapshot is null or empty.
    /// </summary>
    /// <returns>True if the snapshot is null or empty, otherwise false.</returns>
    public bool IsNull()
    {
        return _id == NoSnapshot._id && SceneName.Equals(NoSnapshot.SceneName)
                && Description == "" && Screenshot == null;
    }

    /// <summary>
    /// Initializes a new snapshot with the given ID, scene name, and description.
    /// Automatically takes a screenshot.
    /// </summary>
    /// <param name="id">The ID of the snapshot.</param>
    /// <param name="sceneName">The name of the scene.</param>
    /// <param name="description">The description of the snapshot.</param>
    public Snapshot(int id, string sceneName, string description)
    {
        _id = id;
        SceneName = sceneName;
        Screenshot = null;
        ScreenshotTaker.Instance.TakeScreenshot(this.Screenshot);
        Description = description;
    }

    /// <summary>
    /// Initializes a new snapshot with the given ID, scene name, screenshot, and description.
    /// </summary>
    /// <param name="id">The ID of the snapshot.</param>
    /// <param name="sceneName">The name of the scene.</param>
    /// <param name="screenshot">The screenshot to be associated with the snapshot.</param>
    /// <param name="description">The description of the snapshot.</param>
    public Snapshot(int id, string sceneName, Sprite screenshot, string description)
    {
        _id = id;
        SceneName = sceneName;
        Screenshot = screenshot;
        Description = description;
    }
}