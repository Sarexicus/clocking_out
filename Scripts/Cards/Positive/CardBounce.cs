using Godot;
using System;

public partial class CardBounce : Card
{
	public CardBounce() : base()
	{
		cardDescription = "Projectiles bounce off walls\n(up to 3 times)";
	}

    public override void StartEffect()
    {
		player.playerWeapon.doBounce = true;
    }

    public override void EndEffect()
	{
		player.playerWeapon.doBounce = false;
	}
}
