namespace Snailer.GodotCSharp.SceneManager;

using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

/// <summary>
/// Parses Godot .tscn files.
/// </summary>
public class SceneFileReader
{
  private const string TAG_SCENE = "gd_scene";
  private const string TAG_SCENE_UID = "uid";
  private readonly string _content;

  /// <summary>
  /// Initializes a new <see cref="SceneFileReader" /> to read the file at the provided <paramref name="path"/>.
  /// </summary>
  /// <param name="path">The absolute path to the .tscn file.</param>
  public SceneFileReader(string path)
  {
    _content = FileAccess.GetFileAsString(path);
  }

  /// <summary>
  /// Gets the UID of the Godot scene.
  /// </summary>
  public string? GetSceneId()
  {
    var attributes = GetTagAttributes(TAG_SCENE);

    return attributes?.FirstOrDefault(a => a.Key.Equals(TAG_SCENE_UID, StringComparison.OrdinalIgnoreCase)).Value;
  }

  private Dictionary<string, string>? GetTagAttributes(string tagName)
  {
    var tagStart = _content.IndexOf($"[{tagName}");
    if (tagStart < 0)
    {
      return null;
    }

    var textStartingAtTag = _content.Substring(tagStart, _content.Length);
    var tagEnd = textStartingAtTag.IndexOf(']');
    if (tagEnd < 0)
    {
      return null;
    }

    var content = textStartingAtTag[1..tagEnd];
    var attrs = content.Split(' ');
    var dictionary = new Dictionary<string, string>();
    foreach (var attr in attrs)
    {
      if (!attr.Contains('='))
      {
        continue;
      }

      var vals = attr.Split('=');
      dictionary.Add(vals[0], vals[1].Replace("\"", ""));
    }

    return dictionary;
  }
}
