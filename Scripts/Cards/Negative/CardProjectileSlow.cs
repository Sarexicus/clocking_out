using Godot;
using System;

public partial class CardProjectileSlow : Card
{
	public CardProjectileSlow() : base()
	{
		cardDescription = "-75% projectile speed";
	}

	public override void StartEffect()
	{
		player.playerWeapon.projectileSpeedModifier = 0.25f;
	}

	public override void EndEffect()
	{
		player.playerWeapon.projectileSpeedModifier = 1f;
	}
}
