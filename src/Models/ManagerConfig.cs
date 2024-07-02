namespace Snailer.GodotCSharp.SceneManager.Models;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents the <see cref="SceneManager" /> JSON file.
/// </summary>
public class ManagerConfig
{
  /// <summary>
  /// A list of game scenes.
  /// </summary>
  public IEnumerable<Scene> Scenes { get; set; } = Enumerable.Empty<Scene>();

  /// <summary>
  /// The scene transitions based on user choices.
  /// </summary>
  public IEnumerable<Choice> Choices { get; set; } = Enumerable.Empty<Choice>();
}
