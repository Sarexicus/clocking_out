using Godot;
using System;

public partial class Enemy : EyesCharacter
{
	public HealthObject target;
	[Export] public NavigationAgent2D navAgent;

	[Export] public Weapon enemyWeapon;
	[Export] public MeleeAttack meleeAttack;

	public int minDistanceToTarget = 40;

	[Export] Timer stunTimer;
	[Export] GPUParticles2D stunParticles;

	public Vector2 slideVelocity = Vector2.Zero;

	public bool canSeeTarget;
	public bool isStunned = false;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		target = FindTarget();
		if(enemyWeapon != null) enemyWeapon.canFire = true;

		stunTimer.Timeout += StunEnd;
		target = FindTarget();
		if(meleeAttack != null) meleeAttack.target = target;
	}

    private void StunEnd()
    {
		stunParticles.Emitting = false;
		isStunned = false;
    }

    public override void Knockback(Vector2 knockback)
    {
		slideVelocity = knockback;
	}

	public void Stun()
    {
		isStunned = true;
		float stunTime = 0.5f;
		stunTimer.Start(stunTime);
		stunParticles.Emitting = true;
	}

	HealthObject FindTarget()
    {
		return (HealthObject)GetTree().Root.FindChild("Player", true, false);
	}

    public override void _PhysicsProcess(double delta)
	{
		if (navAgent == null || GetTree() == null ||
			GlobalEffects.GameOver || !IsInsideTree()) return;

		if (target == null || !IsInstanceValid(target)) return;

		base._PhysicsProcess(delta);

		navAgent.SetTargetLocation(target.GlobalPosition);

		TryToFindTarget();

		if (canSeeTarget && meleeAttack != null)
		{
			meleeAttack.TryMelee();
		}

		if (navAgent.IsTargetReachable() && !navAgent.IsTargetReached() && !isStunned)
        {
			var nextPos = canSeeTarget ? target.GlobalPosition : navAgent.GetNextLocation();
			var moveDirection = Position.DirectionTo(nextPos);

			enemyWeapon?.RotateTowards(nextPos);
			meleeAttack?.RotateHandsTowards(nextPos);
			EyesLookTowards(nextPos, 2);

			if (GlobalPosition.DistanceTo(target.GlobalPosition) <= minDistanceToTarget) return;

			Velocity = moveDirection * 50f + slideVelocity;
			if (slideVelocity.Length() > 5)
			{
				slideVelocity *= 0.9f;
			}
			else slideVelocity = Vector2.Zero;

			navAgent.SetVelocity(Velocity);
			MoveAndSlide();
		}


		if (canSeeTarget && enemyWeapon != null)
		{
			enemyWeapon.TryShoot();
		}
	}

	private void TryToFindTarget()
    {
		PhysicsRayQueryParameters2D param = new PhysicsRayQueryParameters2D()
		{
			From = GlobalPosition,
			To = target.GlobalPosition,
			Exclude = new Godot.Collections.Array<RID>() { new RID(this) }
		};

		var spaceState = GetWorld2d().DirectSpaceState;
		var result = spaceState.IntersectRay(param);

        if (result.ContainsKey("collider"))
        {
			canSeeTarget = ((Node2D)result["collider"] == target);
		} 
		else
        {
			canSeeTarget = false;
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
		if(enemyWeapon != null) enemyWeapon.reloadTimer.Paused = isStunned;
	}
}
