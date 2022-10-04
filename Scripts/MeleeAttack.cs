using Godot;
using System;

public partial class MeleeAttack : Node2D
{
	public HealthObject target;
	public bool canAttack = true;

	[Export] public Node2D handsOrigin;
	[Export] public Sprite2D handLSprite, handRSprite, weaponSprite;
	[Export] public Node2D handLOffset, handROffset;
	float handLY, handRY;

	public int distanceThreshold = 80;

    public override void _Ready()
    {
        base._Ready();
        handLY = handLOffset.Position.y;
        handRY = handROffset.Position.y;
    }

    public void TryMelee()
    {
		if(canAttack && GlobalPosition.DistanceTo(target.GlobalPosition) < distanceThreshold)
        {
			Attack();
        }
    }

	public void RotateHandsTowards(Vector2 target)
    {
		handsOrigin.LookAt(target);

        // flip weapon sprite if facing left
        float rotMod = handsOrigin.Rotation % (2 * Mathf.Pi);
        bool swap = ((rotMod < Mathf.DegToRad(-90) && rotMod > Mathf.DegToRad(-270)) ||
            (rotMod > Mathf.DegToRad(90) && rotMod < Mathf.DegToRad(270)));

        handLSprite.FlipV = handRSprite.FlipV = swap;
        if (weaponSprite != null) weaponSprite.FlipV = swap;
        handLOffset.Position = new Vector2(handLOffset.Position.x, (swap) ? -handLY : handLY);
        handROffset.Position = new Vector2(handROffset.Position.x, (swap) ? -handRY : handRY);
    }

    private async void Attack()
    {
		canAttack = false;
		target.Damage(10);

		await ToSignal(GetTree().CreateTimer(1.0), "timeout");
		canAttack = true;
	}
}
