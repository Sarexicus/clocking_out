using Godot;
using System;

public partial class CardEnemyFireRate : Card
{
	public CardEnemyFireRate() : base()
	{
		cardDescription = "2x enemy fire rate";
	}

	public override void StartEffect()
	{
		GlobalEffects.EnemyWeaponFireRateModifier = 0.5f;
	}

	public override void EndEffect()
	{
		GlobalEffects.EnemyWeaponFireRateModifier = 1.0f;
	}
}
