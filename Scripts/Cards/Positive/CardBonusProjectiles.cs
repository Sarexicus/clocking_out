using Godot;
using System;

public partial class CardBonusProjectiles : Card
{
	public CardBonusProjectiles() : base()
	{
		cardDescription = "+2 projectiles per shot";
	}

    public override void StartEffect()
    {
		player.playerWeapon.bonusProjectileCount = 2;
    }

    public override void EndEffect()
	{
		player.playerWeapon.bonusProjectileCount = 0;
	}
}
