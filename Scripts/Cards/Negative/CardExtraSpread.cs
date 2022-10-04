using Godot;
using System;

public partial class CardExtraSpread : Card
{
	// Called when the node enters the scene tree for the first time.
	public CardExtraSpread() : base()
	{
		cardDescription = "+20 degrees bullet spread";
	}

	public override void StartEffect()
	{
		player.playerWeapon.bonusSpread = 0.2f;
	}

	public override void EndEffect()
	{
		player.playerWeapon.bonusSpread = 0;
	}
}
