using Godot;
using System;

public partial class WaveProgress : NinePatchRect
{
	[Export] ProgressBar progress;
	[Export] Label label;


	public void SetProgress(int wave, int totalWaves, int enemiesRemaining, int totalEnemiesThisWave)
    {
		progress.Value = enemiesRemaining;
		progress.MaxValue = totalEnemiesThisWave;
		label.Text = $"Wave {wave+1} / {totalWaves}: {enemiesRemaining} enemies remaining";
    }
}
