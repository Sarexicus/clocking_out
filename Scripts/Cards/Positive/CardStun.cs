using Godot;
using System;

public partial class CardStun : Card
{

	// Called when the node enters the scene tree for the first time.
	public CardStun() : base()
	{
		cardDescription = "Projectiles stun enemies";
	}

    public override void StartEffect()
    {
		player.playerWeapon.doStun = true;
    }

    public override void EndEffect()
	{
		player.playerWeapon.doStun = false;
	}
}
