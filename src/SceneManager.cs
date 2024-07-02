namespace Snailer.GodotCSharp.SceneManager;

using Godot;
using Snailer.GodotCSharp.SceneManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// Enables automatic progression of scenes in a linear or branching topography.
/// </summary>
public class SceneManager
{
  private const string FILENAME = "scenes.json";
  private const string RESOURCES_PATH = "res://";
  private int _currentSceneId;
  private readonly ManagerConfig _managerConfig;
  private readonly List<SceneResource> _sceneResources = new();

  public SceneManager()
  {
    JsonFileHelper.EnsureJsonFile<ManagerConfig>(FILENAME);
    _managerConfig = JsonFileHelper.ReadJsonFile<ManagerConfig>(FILENAME);
    LoadScenes();
  }

  /// <summary>
  /// Loads the next scene.
  /// </summary>
  /// <param name="tree">The current <see cref="SceneTree" />.</param>
  /// <param name="option">A choice (usually selected by the player) used when switching branches. If not provided, the next scene in the
  /// current branch is loaded.</param>
  /// <exception cref="InvalidOperationException"></exception>
  public void Next(SceneTree tree, string? option = null)
  {
    var currentScene = _managerConfig.Scenes.FirstOrDefault(s => s.Id == _currentSceneId);
    Scene? nextScene = null;
    if (option is null)
    {
      // Get next scene on current branch
      var scenesOnBranch = _managerConfig.Scenes
        .Where(s => s.Branch?.Equals(currentScene?.Branch, StringComparison.OrdinalIgnoreCase) ?? false)
        .ToList()
        .OrderBy(s => s.Id);
      nextScene = scenesOnBranch.FirstOrDefault(x => x.Id > _currentSceneId);
    }
    else
    {
      // Get next scene on new branch
      var selectedChoice = _managerConfig.Choices.FirstOrDefault(c => c.SourceScene == _currentSceneId &&
        (c.Option?.Equals(option, StringComparison.OrdinalIgnoreCase) ?? false))
        ?? throw new InvalidOperationException($"Attempted to change branches, but no choice with source {_currentSceneId} and option {option} was found");

      nextScene = _managerConfig.Scenes.FirstOrDefault(x => x.Id == selectedChoice.TargetScene);
    }

    if (nextScene is not null && !string.IsNullOrEmpty(nextScene.Name))
    {
      ChangeScene(tree, nextScene.Name);
    }
  }

  /// <summary>
  /// Loads a scene with the provided <paramref name="name"/>. As opposed to <see cref="Next"/>, there are no constraints to which scenes
  /// can be loaded, meaning you may jump branches or move backward in the scene topography.
  /// </summary>
  /// <param name="tree">The current <see cref="SceneTree" />.</param>
  /// <param name="name">The name of the scene to load (without the extension).</param>
  /// <exception cref="InvalidOperationException"></exception>
  public void ChangeScene(SceneTree tree, string name)
  {
    var resource = _sceneResources.FirstOrDefault(s => s.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false)
      ?? throw new InvalidOperationException($"No scene with name '{name}' found.");

    var newScene = _managerConfig.Scenes.FirstOrDefault(s => s.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false);
    if (newScene is not null)
    {
      _currentSceneId = newScene.Id;
    }

    tree.ChangeSceneToFile(resource.Path);
  }

  private void LoadScenes()
  {
    var scenes = GetSceneFiles(RESOURCES_PATH);
    foreach (var scene in scenes)
    {
      var reader = new SceneFileReader(scene);
      var id = reader.GetSceneId();
      if (id is null)
      {
        continue;
      }

      _sceneResources.Add(new(id, scene));
    }
  }

  private static List<string> GetSceneFiles(string path)
  {
    using var dir = DirAccess.Open(path);
    if (dir is null)
    {
      return new();
    }

    var sceneFiles = dir.GetFiles()
      .Where(f => f.EndsWith("tscn", StringComparison.OrdinalIgnoreCase))
      .Select(f => Path.Combine(dir.GetCurrentDir(), f))
      .ToList();
    var subDirs = dir.GetDirectories();
    foreach (var subDir in subDirs)
    {
      var subDirPath = Path.Combine(dir.GetCurrentDir(), subDir);
      sceneFiles.AddRange(GetSceneFiles(subDirPath));
    }

    return sceneFiles;
  }
}
