# Creating the scenes.json file

The `scenes.json` file contains all scenes managed by the `SceneManager`. The format is easy to read, even for non-technical users, and because the scene topology is managed in a single file, it makes versioning/collaboration easy!

You can use the JSON file to a simple, linear game, or a game with complex "branches" e.g. a story-driven game where the player's choices affect the ending. The JSON file contains a list of branches, where each branch contains 1 or more scenes.

## Branches

A branch within the JSON file must contain the following properties:

- __Name__: An arbitrary name for the branch
- __Scenes__: A list of [scenes](#scenes) in the branch

The following properties are _optional_ and used for non-linear games:

- __Source__: The ID of a scene that transitions into this branch
- __Choice__: An arbitrary string (usually from player input) used to match which branch to transition to

## Scenes

A scene within the JSON file must contain the following properties:

- __Name__: The name of the Godot .tscn file, without the extension. The file can be contained in any directory of your project, e.g. `res://src/Scenes/Dialog1.tscn` and `res://src/Dialogs/Dialog1.tscn` would both be named "Dialog1," so ensure your files have unique names
- __Id__: An arbitrary, unique number which indicates the order of your scenes, where a higher number means the scene will be loaded later in the game

## Examples

### Linear games

A game may contain a single, linear branch:

![Linear diagram](/img/lineargame.png)

The JSON file then only needs to contain a single branch:

```json
{
  "Branches": [
    {
      "Name": "Main",
      "Scenes": [
        {
          "Id": 1,
          "Name": "IntroScene"
        },
        {
          "Id": 2,
          "Name": "Dialog1"
        },
        {
          "Id": 3,
          "Name": "Dialog2"
        }
      ]
    }
  ]
}
```

### Branching games

The player's decisions during the game may bring them to a different scene progression:

![Branching diagram](/img/branchinggame.png)

Within the JSON file, you would then create the 3 branches labelled above. To automatically transition into new branches, the __Source__ should be set to the scene where the branch "splits" and the __Choice__ determines the direction the player will go:

```json
{
  "Branches": [
    {
      "Name": "Intro",
      "Scenes": [
        {
          "Id": 1,
          "Name": "IntroScene"
        },
        {
          "Id": 2,
          "Name": "Dialog1"
        }
      ]
    },
    {
      "Source": 2,
      "Choice": "Yes",
      "Name": "Happy",
      "Scenes": [
        {
          "Id": 3,
          "Name": "HappyScene"
        },
        // And more scenes..
      ]
    },
    {
      "Source": 2,
      "Choice": "No",
      "Name": "Sad",
      "Scenes": [
        {
          "Id": 4,
          "Name": "SadScene"
        },
        // And more scenes..
      ]
    }
  ]
}

```
