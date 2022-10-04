using Godot;
using System;

public partial class GameManager : Node
{
	[Export] public WaveManager waveManager;
	[Export] public CardManager cardManager;
	[Export] public Player player;
	[Export] public AudioStreamPlayer musicPlayer;
	[Export] public Control GUI;

    [ExportGroup("UI")]
	[Export] CanvasLayer canvas;
	[Export] PackedScene cardOrderSelectorScene;
	[Export] PackedScene gameOverScene;
	[Export] PackedScene youWinScene;

	[Export] PackedScene nextLevelScene;

	[Export] Camera2D deathCamera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player.OnDeath += GameOver;

		var cardOrderSelector = cardOrderSelectorScene.Instantiate() as CardOrderSelector;
		cardOrderSelector.manager.gameManager = this;
		canvas.CallDeferred("add_child",cardOrderSelector);
		GUI.Visible = false;

		waveManager.OnWin += YouWin;
	}

	public void SetupGameUI()
	{
		cardManager.InitCards();
		cardManager.SetUIDirty();

		waveManager.UpdateUI();
		GUI.Visible = true;
	}

	public void StartGame()
	{
		GlobalEffects.GameOver = false;
		player.canMove = true;
		player.playerWeapon.canFire = true;
		cardManager.SetupAndStart();
		waveManager.Start();

		musicPlayer.Play();
    }

	public async void GameOver()
    {
		musicPlayer.Stop();
		GlobalEffects.GameOver = true;

		await ToSignal(GetTree().CreateTimer(3.0), "timeout");

		canvas.CallDeferred("add_child", gameOverScene.Instantiate());
		//musicPlayer.Play();
	}

	public async void YouWin()
	{
		musicPlayer.Stop();
		GlobalEffects.GameOver = true;

		player.canMove = false;
		player.playerWeapon.canFire = false;

		cardManager.StopTimer();

		await ToSignal(GetTree().CreateTimer(1.0), "timeout");

		var winScreen = youWinScene.Instantiate() as GameOverScreen;
		winScreen.sceneToLoad = nextLevelScene;
		canvas.CallDeferred("add_child", winScreen);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("debug2"))
        {
			YouWin();
        }

	}
}
