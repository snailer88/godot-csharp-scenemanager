# Using the manager

Once you have created the `scenes.json` file,  you should instantiate a new `SceneManager` at program start:

```cs
var mgr = new SceneManager();
```

You can use the manager in the place of any `SceneTree.ChangeSceneToFile` call in your program, allowing you to change scenes without knowing the full path of the scene.

> :bulb: Scenes loaded with `ChangeScene()` do not need to present in the JSON file!

```cs
// Loads the res://src/Scenes/Settings.tscn file
mgr.ChangeScene(GetTree(), "Settings");
```

To go to the next scene in the current branch, you can simply call `Next()`:

```cs
mgr.Next(GetTree());
```

When you are within a scene which transitions to other branches, pass an additional parameter to the call. For example, your game could split into 3 different branches after you ask the player to make a choice within a dialog box:

```cs
mgr.Next(GetTree(), "PlayerSelectedEvilOption");
// or
mgr.Next(GetTree(), "PlayerSelectedGoodOption");
// or
mgr.Next(GetTree(), "PlayerWalkedAway");
```

These strings should match the __Choice__ property of the desired branch within the JSON file.
