using Godot;
using System;

public partial class Explosion : Node2D
{
	[Export] public GPUParticles2D particles;
	[Export] public AudioStreamPlayer2D audioPlayer;
	[Export] public AudioStream audioStream;

	[Export] public Timer despawnTimer;

	public void Explode()
    {
		particles.Emitting = true;
		audioPlayer.Stream = audioStream;
		audioPlayer.Play();

		despawnTimer.Timeout += Despawn;
		despawnTimer.Start(5);
    }

	public void Despawn()
    {
		QueueFree();
    }
	
}
