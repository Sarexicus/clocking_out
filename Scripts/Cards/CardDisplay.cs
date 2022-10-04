using Godot;
using System;

public partial class CardDisplay : TextureRect
{
	[Export] public TextureRect cardIcon;

	public void SetIcon(CompressedTexture2D texture)
    {
		cardIcon.Texture = texture;
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
