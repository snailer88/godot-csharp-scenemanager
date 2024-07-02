namespace Snailer.GodotCSharp.SceneManager;

using Godot;
using Snailer.GodotCSharp.SceneManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// Enables automatic progression of scenes in a linear or branching topology.
/// </summary>
public class SceneManager
{
  private const string FILENAME = "scenes.json";
  private const string RESOURCES_PATH = "res://";
  private Scene? _currentScene;
  private Branch? _currentBranch;
  private readonly ManagerConfig _managerConfig;
  private readonly List<SceneResource> _sceneResources = new();

  public SceneManager()
  {
    JsonFileHelper.EnsureJsonFile<ManagerConfig>(FILENAME);
    _managerConfig = JsonFileHelper.ReadJsonFile<ManagerConfig>(FILENAME);
    LoadSceneResources();
  }

  /// <summary>
  /// Loads the next scene.
  /// </summary>
  /// <param name="tree">The current <see cref="SceneTree" />.</param>
  /// <param name="choice">A choice (usually selected by the player) used when switching branches. If not provided, the next scene in the
  /// current branch is loaded.</param>
  /// <exception cref="InvalidOperationException"></exception>
  public void Next(SceneTree tree, string? choice = null)
  {
    if (choice is null)
    {
      // Get next scene on current branch
      if (_currentBranch is null)
      {
        throw new InvalidOperationException($"Attempted to move forward in current branch, but no branch is set.");
      }

      var scenesOnBranch = _currentBranch.Scenes.OrderBy(s => s.Id);
      var nextScene = scenesOnBranch.FirstOrDefault(x => x.Id > _currentScene?.Id)
        ?? throw new InvalidOperationException($"Attempted to move forward in current branch, but there is no next scene.");
      if (string.IsNullOrEmpty(nextScene.Name))
      {
        throw new InvalidOperationException($"Attempted to move forward in current branch, but the next scene has no name.");
      }

      ChangeScene(tree, nextScene.Name);
    }
    else
    {
      // Get new branch
      var targetBranch = _managerConfig.Branches
        .Where(b => b.Source == _currentScene?.Id && (b.Choice?.Equals(choice, StringComparison.OrdinalIgnoreCase) ?? false))
        .FirstOrDefault()
        ?? throw new InvalidOperationException($"Attempted to change branches, but no branch with choice '{choice}' and source scene {_currentScene?.Id} was found.");
      if (string.IsNullOrEmpty(targetBranch.Name))
      {
        throw new InvalidOperationException($"Attempted to change branches, but the target branch has no name.");
      }

      ChangeBranch(tree, targetBranch.Name);
    }
  }

  /// <summary>
  /// Loads a scene with the provided <paramref name="name"/>. As opposed to <see cref="Next"/>, there are no constraints to which scenes
  /// can be loaded, meaning you may jump branches or move backward in the scene topology.
  /// </summary>
  /// <param name="tree">The current <see cref="SceneTree" />.</param>
  /// <param name="name">The name of the scene to load (without the extension).</param>
  /// <exception cref="InvalidOperationException"></exception>
  public void ChangeScene(SceneTree tree, string name)
  {
    var newSceneResource = _sceneResources.FirstOrDefault(s => s.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false)
      ?? throw new InvalidOperationException($"No scene with name '{name}' found.");

    foreach (var branch in _managerConfig.Branches)
    {
      var newScene = branch.Scenes.FirstOrDefault(s => s.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false);
      if (newScene is null)
      {
        continue;
      }

      _currentScene = newScene;
      _currentBranch = branch;
      tree.ChangeSceneToFile(newSceneResource.Path);

      return;
    }

    // Scene is not in JSON file, but we can still switch to an arbitrary scene (like a main menu)
    tree.ChangeSceneToFile(newSceneResource.Path);
  }

  /// <summary>
  /// Loads the first scene in the specified <paramref name="branchName"/>.
  /// </summary>
  /// <param name="tree">The current <see cref="SceneTree" />.</param>
  /// <param name="branchName">The name of the branch to switch to.</param>
  /// <exception cref="InvalidOperationException"></exception>
  public void ChangeBranch(SceneTree tree, string branchName)
  {
    var newBranch = _managerConfig.Branches.FirstOrDefault(s => s.Name?.Equals(branchName, StringComparison.OrdinalIgnoreCase) ?? false)
      ?? throw new InvalidOperationException($"No branch with name '{branchName}' found.");
    var newScene = newBranch.Scenes.OrderBy(s => s.Id).First();
    if (newScene is null || string.IsNullOrEmpty(newScene.Name))
    {
      throw new InvalidOperationException($"The branch '{branchName}' contains no scenes, or the first scene has no name.");
    }

    ChangeScene(tree, newScene.Name);
  }

  private void LoadSceneResources()
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
