using Godot;
using System;

public partial class CardDoubleDamageTaken : Card
{
	public CardDoubleDamageTaken() : base()
	{
		cardDescription = "2x damage taken";
	}

	public override void StartEffect()
	{
		player.damageTakenMultiplier = 2f;
	}

	public override void EndEffect()
	{
		player.damageTakenMultiplier = 1f;
	}
}
