namespace Snailer.GodotCSharp.SceneManager.Models;

/// <summary>
/// Represents a branch with scenes in the scene topology.
/// </summary>
public class Branch
{
    /// <summary>
    /// The branch name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The <see cref="Scene.Id"/> which can cause a switch into this branch.
    /// </summary>
    public int Source { get; set; }

    /// <summary>
    /// An arbitrary string representing a choice made by the player which causes a switch into this branch.
    /// </summary>
    public string? Choice { get; set; }

    /// <summary>
    /// A list of game scenes in the branch.
    /// </summary>
    public IEnumerable<Scene> Scenes { get; set; } = Enumerable.Empty<Scene>();
}