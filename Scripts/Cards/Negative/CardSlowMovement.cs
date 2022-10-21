using Godot;
using System;

public partial class CardSlowMovement : Card
{
	public CardSlowMovement() : base()
	{
		cardDescription = "-80% movement speed";
	}

	public override void StartEffect()
	{
		player.speedMultiplier = 0.2f;
	}

	public override void EndEffect()
	{
        player.speedMultiplier = 1f;
	}
}
