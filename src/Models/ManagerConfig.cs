namespace Snailer.GodotCSharp.SceneManager.Models;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents the <see cref="SceneManager" /> JSON file.
/// </summary>
public class ManagerConfig
{
  /// <summary>
  /// A list of scene branches.
  /// </summary>
  public IEnumerable<Branch> Branches { get; set; } = Enumerable.Empty<Branch>();
}
