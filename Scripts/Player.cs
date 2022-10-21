using Godot;
using System;

public partial class Player : EyesCharacter
{
	public const float m_speed = 300.0f;

	[Export] Node2D shoeL, shoeR;
	[Export] Area2D playerHitbox;

	[Export] public Weapon playerWeapon;
	[Export] AnimationPlayer animationPlayer;
	[Export] Camera2D playerCamera;

	private Vector2 slideVelocity = Vector2.Zero;
	private Vector2 lastDirection;

	[Export] Timer slideTimer;
	public bool canSlide = true;

	public float speedMultiplier = 1f;
	public bool canMove = false; 

    public override void _Ready()
    {
        base._Ready();
		OnDeath += Death;
	}

	public void Death() => CallDeferred(nameof(DetachCamera));

	public void DetachCamera()
	{
		RemoveChild(playerCamera);
		playerCamera.PositionSmoothingEnabled = false;
		playerCamera.GlobalPosition = GlobalPosition;
		GetTree().Root.AddChild(playerCamera);
    }

    private int GetMovementDirection(string positive, string negative)
    {
		if (!canMove) return 0;
		return Convert.ToInt32(Input.IsActionPressed(positive)) - Convert.ToInt32(Input.IsActionPressed(negative));
	}

	public void ResetSlide()
    {
		canSlide = true;
    }

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Movement();

		Vector2 targetPos = GetGlobalMousePosition();

		EyesLookTowards(targetPos, 1);
		playerWeapon.RotateTowards(targetPos);

		if (Input.IsActionPressed("fire"))
		{
			playerWeapon.TryShoot();
		}

		if (Input.IsActionPressed("slide") && canSlide && canMove)
		{
			Slide();
		}

	}

	private void Slide()
    {
		if (lastDirection != Vector2.Zero)
		{
			canSlide = false;
			slideVelocity = lastDirection.Normalized() * 2 * m_speed;
			slideTimer.Start();

			animationPlayer.PlaybackSpeed = 1.5f;
			string anim = (lastDirection.x > 0) ? "slideClockwise" : "slideAnticlockwise";
			animationPlayer.Play(anim);
		}
	}

    private void Movement()
    {
		int vertical_movement = GetMovementDirection("move_down", "move_up");
		int horizontal_movement = GetMovementDirection("move_right", "move_left");

		var motion = new Vector2(horizontal_movement, vertical_movement);

		if (motion != Vector2.Zero) lastDirection = motion;

		Velocity = (motion.Normalized() * m_speed + slideVelocity) * speedMultiplier;
		if (slideVelocity.Length() > 5)
		{
			slideVelocity *= 0.9f;
		} 
		else slideVelocity = Vector2.Zero;

		bool collided = MoveAndSlide();

		if ((vertical_movement != 0 || horizontal_movement != 0) && Velocity != Vector2.Zero)
		{
			float walkAnimationSpeed = 10f;

			float time = ((Time.GetTicksMsec() / 1000f) % (1.25f * Mathf.Pi)) * walkAnimationSpeed;

			shoeL.Rotation = Mathf.Sin(time) - Mathf.DegToRad(25);
			shoeR.Rotation = -Mathf.Sin(time) + Mathf.DegToRad(25);
		}
		else
		{
			shoeL.Rotation = shoeR.Rotation = 0;
		}

	}
}
