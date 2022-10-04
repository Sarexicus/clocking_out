using Godot;
using System;

public partial class CardExplodingProjectiles : Card
{
	public CardExplodingProjectiles() : base()
	{
		cardDescription = "Projectiles explode on contact";
	}

    public override void StartEffect()
    {
		player.playerWeapon.doExplode = true;
		player.playerWeapon.damageMultiplier = 2f;
	}

    public override void EndEffect()
	{
		player.playerWeapon.doExplode = false;
		player.playerWeapon.damageMultiplier = 1f;
	}
}
