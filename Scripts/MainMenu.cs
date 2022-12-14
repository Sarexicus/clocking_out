using Godot;
using System;
using Discord;

public partial class MainMenu : Control
{
	[Export] Button playButton;
	[Export] PackedScene firstLevelScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playButton.Pressed += Play;
	}

	public void Play()
    {
		GetTree().ChangeSceneToPacked(firstLevelScene);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
	}

}
