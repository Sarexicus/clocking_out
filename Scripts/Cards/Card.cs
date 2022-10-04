using Godot;
using System;

public abstract partial class Card : Node
{
	public Player player;

	public string cardDescription = "Card";
	public CompressedTexture2D cardImage;

    public void SetPlayer(Player p)
    {
        player = p;
    }

    public string GetText()
    {
        return cardDescription;
    }

    public void Init()
    {
        string loc = $"res://Sprites/CardIcons/{GetType().Name}.png";
        cardImage = ResourceLoader.Load<CompressedTexture2D>(loc);
    }

    // when effect starts
    public abstract void StartEffect();

	// when effect ends
	public abstract void EndEffect();
}
