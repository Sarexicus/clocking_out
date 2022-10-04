using Godot;
using System;

public partial class Weapon : Node2D
{
	[ExportGroup("Nodes")]
	[Export] Sprite2D weaponSprite;
	[Export] Node2D bulletSpawnPosition;
	[Export] GPUParticles2D muzzleFlashParticles;

	[Export] Sprite2D handLSprite, handRSprite;
	[Export] Node2D handLOffset, handROffset;
	[Export] bool useRightHand = false;

	[Export] public Timer reloadTimer;
	[Export] AnimationPlayer reloadAnimation;

	[Export] AudioStreamPlayer2D audioPlayer;

	[Export] bool isEnemy = true;

    [ExportGroup("Prefabs")]
	[Export] PackedScene bulletPrefab;
	[Export] AudioStreamWAV soundReload1;
	[Export] AudioStreamWAV soundReload2;
	[Export] AudioStreamWAV soundShoot;


	[ExportGroup("Weapon Properties")]
	[Export] float reloadTime = 0.5f;
	[Export] float spreadRoot = 0.0f;
	[Export] int projectileCountRoot = 1;

	public bool doPierce = false;
	public bool doBounce = false;
	public bool doExplode = false;
	public bool doStun = false;

	[Export] public int weaponDamage = 10;

	public float reloadTimeMultiplier = 1f;
	public float damageMultiplier = 1f;
	public float projectileSpeedModifier = 1f;

	public int bonusProjectileCount = 0;
	public float bonusSpread = 0f;

	public bool canFire = false;
	float handLY, handRY;

	private const float bulletSpeed = 750f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		handLY = handLOffset.Position.y;
		if (useRightHand) handRY = handROffset.Position.y;

		reloadTimer.Timeout += Reload;
	}

	public void Reload()
    {
		canFire = true;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void RotateTowards(Vector2 globalPosition)
    {
		LookAt(globalPosition);

		// flip weapon sprite if facing left
		float rotMod = Rotation % (2 * Mathf.Pi);
		bool swap = ((rotMod < Mathf.DegToRad(-90) && rotMod > Mathf.DegToRad(-270)) ||
			(rotMod > Mathf.DegToRad(90) && rotMod < Mathf.DegToRad(270)));

		handLSprite.FlipV = weaponSprite.FlipV = swap;
		handLOffset.Position = new Vector2(handLOffset.Position.x, (swap) ? -handLY : handLY);

		if (useRightHand)
		{
			handRSprite.FlipV = swap;
			handROffset.Position = new Vector2(handROffset.Position.x, (swap) ? -handRY : handRY);
		}
	}

    public override void _PhysicsProcess(double delta)
    {

    }

	public void TryShoot()
    {
		if (canFire) FireBullet();
	}

    public void FireBullet()
	{
		canFire = false;

		float finalReloadTime = reloadTime * reloadTimeMultiplier;
		if (isEnemy) finalReloadTime *= GlobalEffects.EnemyWeaponFireRateModifier;

		if(audioPlayer != null)
        {
			audioPlayer.Stream = soundShoot;
			audioPlayer.Play();
		}

		reloadTimer.Start(finalReloadTime);

		if(reloadAnimation != null)
        {
			reloadAnimation.PlaybackSpeed = 1 / finalReloadTime;
			reloadAnimation.Play("reload");
		}

		muzzleFlashParticles.Emitting = true;
		muzzleFlashParticles.Restart();

		float spread = spreadRoot + bonusSpread;
		float spreadRad = Mathf.DegToRad(spread);

		int projectileCount = projectileCountRoot + bonusProjectileCount;

		for (int i = 0; i < projectileCount; i++)
        {
			var projectile = bulletPrefab.Instantiate() as Projectile;

			float rot = (projectileCount == 1) ? Rotation : Rotation - (spreadRad / 2) + (spreadRad / (projectileCount - 1) * (i));

			projectile.projectileDamage = weaponDamage;
			projectile.doPierce = doPierce;
			projectile.doBounce = doBounce;
			projectile.doExplode = doExplode;
			projectile.doStun = doStun;
			projectile.projectileDamage = (int)(weaponDamage * damageMultiplier);

			projectile.GlobalPosition = bulletSpawnPosition.GlobalPosition;
			projectile.Rotation = rot;
			projectile.ApplyImpulse(new Vector2(bulletSpeed * projectileSpeedModifier, 0).Rotated(rot));
			GetTree().Root.CallDeferred("add_child", projectile);
		}


	}
}
