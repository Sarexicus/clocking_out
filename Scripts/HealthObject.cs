using Godot;
using System;

public partial class HealthObject : CharacterBody2D
{
    [ExportGroup("Health")]
	[Export] public int maxHealth;
	[Export] public PackedScene healthBarPrefab;
    public int health;

	[Export] public int invincibilityFrames = 5;
	public int remainingInvincibilityFrames;

	public float damageTakenMultiplier = 1f;

	public ProgressBar healthBar;

	bool isDead = false;
	bool isInit = false;

	float healthBarSlidePercent = 0.1f;

    [ExportGroup("Sounds")]
	[Export] public AudioStreamPlayer2D audioPlayer;
	[Export] public AudioStreamWAV soundHurt;

	[Export] public PackedScene explosionPrefab;

	[Signal] public delegate void DeathSignalEventHandler();
	public event DeathSignalEventHandler OnDeath;

	public override void _Ready()
	{
		health = maxHealth;
		healthBar = healthBarPrefab.Instantiate() as ProgressBar;
		healthBar.Position = new Vector2(-32, -28);
		healthBar.Value = healthBar.MaxValue = maxHealth;
		FindChild("HealthWrapper", true, false).CallDeferred("add_child", healthBar);

		remainingInvincibilityFrames = 0;

		isInit = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		healthBar.Value = Mathf.MoveToward((float)healthBar.Value, health, 2);

		if (remainingInvincibilityFrames > 0)
		{
			remainingInvincibilityFrames--;
		}
	}

	public virtual void Knockback(Vector2 knockback)
    {
		
    }

	private void Remove()
	{
		QueueFree();
	}
	
	private void AddExplosion(Explosion explosion)
    {
		GetTree().Root.AddChild(explosion);
		explosion.GlobalPosition = GlobalPosition;
		explosion.Explode();
		Remove();
	}

	private void Explode()
    {
		var explosion = explosionPrefab.Instantiate() as Explosion;
		CallDeferred("AddExplosion", explosion);
	}

	private void CheckHealth()
    {
		if (health <= 0 && !isDead && isInit)
		{
			isDead = true;
			OnDeath?.Invoke();
			Explode();
		}
    }

	public void Damage(int delta)
    {
		if (delta > 0)
		{
			if (remainingInvincibilityFrames > 0) return;
			remainingInvincibilityFrames = invincibilityFrames;
		}

		int damage = delta;
		if (delta > 0) damage = (int)(damage*damageTakenMultiplier);

		health -= damage;
		health = Mathf.Clamp(health, 0, maxHealth);

		if(health > 0)
        {
			audioPlayer.Stream = soundHurt;
			audioPlayer.Play();
        }
		CheckHealth();
    }
}
