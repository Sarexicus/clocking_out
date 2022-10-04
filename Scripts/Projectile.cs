using Godot;
using System;

public partial class Projectile : RigidBody2D
{
	[Export] public PackedScene explosionPrefab;
	[Export] public PackedScene explosionPrefabLarge;

	public bool doExplode = false;

	public int projectileDamage = 10;
	public bool doPierce = false;
	public bool doBounce = false;
	public bool doStun = false; 

	int bounceCount = 0;

	[Export] public Area2D hitbox;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hitbox.BodyEntered += OnCollisionStart;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		DespawnTimer();
	}

    private async void DespawnTimer()
    {
		await ToSignal(GetTree().CreateTimer(3.0), "timeout");
		Explode();
	}

    public void OnCollisionStart(Node2D other)
    {
		if (other is HealthObject character)
		{
			Vector2 dir = GlobalPosition.DirectionTo(character.GlobalPosition);

			if (doStun && other is Enemy e)
			{
				e.Stun();
			}

			character.Knockback(dir*100f);
			character.Damage(projectileDamage);

			if (!doPierce)
			{
				if (doExplode)
				{
					Explode();
				}
				else Remove();
			}
		}
		if(other is TileMap)
        {
			if(doBounce)
            {
				bounceCount++;
				if (bounceCount >= 3) Explode();
            }
			else
			{
				Explode();
			}
		}
	}

	public void Remove()
    {
		QueueFree();
	}
	public void Explode()
    {
		CallDeferred("DoExplode");
    }

	public void DoExplode()
    {
		var prefab = (doExplode) ? explosionPrefabLarge : explosionPrefab;
		var explosion = prefab.Instantiate() as Explosion;
		GetTree().Root?.AddChild(explosion);
		explosion.GlobalPosition = GlobalPosition;
		explosion.CallDeferred("Explode");
		CallDeferred("Remove");
	}
}
