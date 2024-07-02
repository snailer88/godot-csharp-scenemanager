namespace Snailer.GodotCSharp.SceneManager.Models;

/// <summary>
/// Represents a scene on a branch.
/// </summary>
public class Scene
{
  /// <summary>
  /// The scene ID.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// The scene name, without the extension.
  /// </summary>
  public string? Name { get; set; }
}
