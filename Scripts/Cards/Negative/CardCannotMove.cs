using Godot;
using System;

public partial class CardCannotMove : Card
{
	public CardCannotMove() : base()
	{
		cardDescription = "Cannot move";
	}

	public override void StartEffect()
	{
		player.canMove = false;
	}

	public override void EndEffect()
	{
		player.canMove = true;
	}
}
