using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using Godot.Collections;

public partial class WaveManager : Node2D
{
	[Export] Array<PackedScene> enemyPrefabs;
	[Export] Node2D spawnLocationsRoot;
	[Export] Control GUI;

	[Export] AudioStreamPlayer waveSFX;
	WaveProgress waveProgress;

	Array<SpawnLocation> spawnLocations;

	public int currentEnemyCount = 0;

	[Export] public Node enemyWaveRoot;
	Array<Wave> waves;

	[Signal] public delegate void WinSignalEventHandler();
	public event WinSignalEventHandler OnWin;

	public bool isStarted = false;


	int waveID = 0;
	int waveProgressIndex = 0;

	int enemiesDefeatedThisWave = 0;

	public Player player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnLocations = new Array<SpawnLocation>(spawnLocationsRoot.GetChildren().Cast<SpawnLocation>());
		var children = enemyWaveRoot.GetChildren().Cast<Wave>();

		waves = new Array<Wave>(children);

		waveProgress = GUI.FindChild("WaveProgress", true, false) as WaveProgress;

	}

	public void Start()
    {
		isStarted = true;
    }

	public void SpawnEnemy(int type, Vector2 spawnPosition)
    {
		var enemy = enemyPrefabs[type].Instantiate() as Enemy;

		enemy.OnDeath += OnEnemyDeath;

		GetTree().Root.CallDeferred("add_child", enemy);
		enemy.GlobalPosition = spawnPosition;

		waveProgressIndex++;
		currentEnemyCount++;
    }
    void OnEnemyDeath()
    {
		currentEnemyCount--;
		enemiesDefeatedThisWave++;
		UpdateUI();
	}

	public void UpdateUI()
    {
		if (waves[waveID] == null) return;
		int count = waves[waveID].enemies.Count;
		int left = count - enemiesDefeatedThisWave;
		waveProgress.SetProgress(waveID, waves.Count, left, count);
	}

	public bool FindFreeSpawnSpace(out SpawnLocation space)
    {
		float spawnDistanceThreshold = 250f;

		spawnLocations.Shuffle();
		foreach (SpawnLocation node in spawnLocations)
        {
			if(!node.IsEnemyNearby() && node.GlobalPosition.DistanceTo(player.GlobalPosition) > spawnDistanceThreshold)
            {
				space = node;
				return true;
            }
        }
		space = null;
		return false;
    }

	public void NextWave()
	{
		waveSFX.Play();

		// heal player to full
		player.Damage(-1000);

		waveID++;
		waveProgressIndex = 0;
		enemiesDefeatedThisWave = 0;
		UpdateUI();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GlobalEffects.GameOver) return;
		if (player == null)
        {
			player = GetTree().Root.FindChild("Player", true, false) as Player;
		}

		if (isStarted && currentEnemyCount < waves[waveID].maxEnemies)
        {
			if(waveProgressIndex < waves[waveID].enemies.Count)
            {
				if (FindFreeSpawnSpace(out SpawnLocation freeSpace))
				{
					SpawnEnemy(waves[waveID].enemies[waveProgressIndex], freeSpace.GlobalPosition);
				}
			} 
			else
            {
				if(currentEnemyCount == 0)
                {
					// next wave
					if(waveID < waves.Count-1)
                    {
						NextWave();
                    } else
                    {
						OnWin();
                    }
				}
			}
			
        }
	}
}
