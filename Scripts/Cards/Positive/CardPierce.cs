using Godot;
using System;

public partial class CardPierce : Card
{

	// Called when the node enters the scene tree for the first time.
	public CardPierce() : base()
	{
		cardDescription = "Projectiles pierce enemies";
	}

    public override void StartEffect()
    {
		player.playerWeapon.doPierce = true;
    }

    public override void EndEffect()
	{
		player.playerWeapon.doPierce = false;
	}
}
