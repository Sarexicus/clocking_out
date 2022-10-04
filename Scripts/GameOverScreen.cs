using Godot;
using System;

public partial class GameOverScreen : Control
{
	[Export] public PackedScene sceneToLoad;

	[Export] Button retryButton;
	Color transparent = new Color("#000000", 0.0f);
	Color full = new Color("#FFFFFF", 1.0f);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Modulate = transparent;
		Tween t = CreateTween();
		t.TweenProperty(this, "modulate", full, 1.0);
		t.Play();

		retryButton.Pressed += RestartScene;
	}

    private void RestartScene()
    {
		if (sceneToLoad != null)
		{
			GetTree().ChangeSceneToPacked(sceneToLoad);
		} 
		else GetTree().ReloadCurrentScene();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
