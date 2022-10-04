using Godot;
using System;

public partial class VictoryScreen : Control
{
	[Export] Button playButton;
	[Export] string firstLevelScenePath;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playButton.Pressed += Play;
	}

	public void Play()
	{
		GetTree().ChangeSceneToFile(firstLevelScenePath);
	}
}
