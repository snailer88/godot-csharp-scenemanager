namespace Snailer.GodotCSharp.SceneManager.Models;

/// <summary>
/// Represents a choice presented to the player which brings them to a new branch in the scene topography.
/// </summary>
public class Choice
{
  /// <summary>
  /// The ID of the scene which presented the player with a choice.
  /// </summary>
  public int SourceScene { get; set; }

  /// <summary>
  /// The ID of the scene to load when the player chooses the matching <see cref="Option"/>.
  /// </summary>
  public int TargetScene { get; set; }

  /// <summary>
  /// An arbitrary string representing one of the options displayed to the player.
  /// </summary>
  public string? Option { get; set; }
}
