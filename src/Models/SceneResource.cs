namespace Snailer.GodotCSharp.SceneManager.Models;

/// <summary>
/// Represents a .tscn file loaded from Godot resources.
/// </summary>
public class SceneResource
{
  /// <summary>
  /// The UID of the scene.
  /// </summary>
  public string Id { get; set; }

  /// <summary>
  /// The absolute path to the .tscn file.
  /// </summary>
  public string Path { get; set; }

  /// <summary>
  /// The file name, without the extension.
  /// </summary>
  public string? Name { get; set; }

  public SceneResource(string id, string path)
  {
    Id = id;
    Path = path;
    Name = GetName();
  }

  private string? GetName()
  {
    var fileName = System.IO.Path.GetFileName(Path);
    if (fileName == null)
    {
      return null;
    }

    var lastDot = fileName.LastIndexOf('.');

    return lastDot < 0 ? null : fileName[..lastDot];
  }
}
