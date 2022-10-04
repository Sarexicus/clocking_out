using Godot;
using System;

public partial class CardFireSpeed : Card
{

	public CardFireSpeed() : base()
	{
		cardDescription = "Double firing speed";
	}

    public override void StartEffect()
    {
		player.playerWeapon.reloadTimeMultiplier = 0.5f;
    }

    public override void EndEffect()
	{
		player.playerWeapon.reloadTimeMultiplier = 1f;
	}
}
