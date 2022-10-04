using Godot;
using System;

public partial class EyesCharacter : HealthObject
{
	[Export] public Sprite2D eyesSprite;

	public void EyesLookTowards(Vector2 position, int amount)
    {
		Vector2 eyePosGlobal = eyesSprite.GlobalPosition;
		eyePosGlobal = eyePosGlobal.MoveToward(position, amount);
		eyesSprite.Offset = eyesSprite.ToLocal(eyePosGlobal);
	}

}
