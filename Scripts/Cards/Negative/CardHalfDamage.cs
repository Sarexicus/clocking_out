using Godot;
using System;

public partial class CardHalfDamage : Card
{
	public CardHalfDamage() : base()
	{
		cardDescription = "-50% bullet damage dealt";
	}

	public override void StartEffect()
	{
		player.playerWeapon.damageMultiplier = 0.5f;
	}

	public override void EndEffect()
	{
		player.playerWeapon.damageMultiplier = 1f;
	}
}
