# Godot Scene Manager

[![NuGet](https://img.shields.io/nuget/v/Snailer.GodotCSharp.SceneManager)](https://www.nuget.org/packages/Snailer.GodotCSharp.SceneManager#versions-body-tab)
[![build](https://github.com/snailer88/godot-csharp-scenemanager/actions/workflows/build.yml/badge.svg)](https://github.com/snailer88/godot-csharp-scenemanager/actions/workflows/build.yml)

The `SceneManager` allows you to easily create a sequence of linear scenes or a complex branching tree of scenes in a single JSON file.

## Pros

- Simplify: Load scenes by file name rather than absolute path
- Automate: Go to next scene automatically, even in another branch, without providing the name
- Collaborate: All scene transitions stored in single, easy-to-read JSON file

## Installation

Add the NuGet package to your Godot project:

```bash
dotnet add package Snailer.GodotCSharp.SceneManager
```

## Getting started

Create a `scenes.json` file in the root of your project containing your scene names and IDs indicating the order they appear:

```json
{
  "Branches": [
    {
      "Name": "Main",
      "Scenes": [
        {
          "Id": 1,
          "Name": "GoToWork"
        },
        {
          "Id": 2,
          "Name": "ComeHome"
        },
        {
          "Id": 3,
          "Name": "GameOver"
        }
      ]
    }
  ]
}
```

During your program's startup, create a new `SceneManager` which will be used to progress through the scenes.

> :bulb: You may want to store the manager in some static class to be accessed later!

```cs
GlobalHelper.SceneManager = new SceneManager();
```

You can now use the manager to move to the next scene automatically!

```cs
 GlobalHelper.SceneManager?.Next(GetTree());
```

## Documentation

Check out the documentation for detailed usage scenarios and examples!

- [Creating the scenes.json file](/docs/CreatingTheJsonFile.md)
- [Using the manager](/docs/UsingTheManager.md)
