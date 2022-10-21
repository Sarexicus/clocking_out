using Godot;
using System;

public partial class DiscordIntegration : Node
{
	Discord.Discord discord;
	public const long CLIENT_ID = 1027137898333618227;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		discord = new Discord.Discord(CLIENT_ID, (UInt64)Discord.CreateFlags.Default);
	}

	public static void SetDetails()
    {

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        var activity = new Discord.Activity
		{
			Details = "Wave 1/4",
			State = "",
			Assets =
			{
				LargeImage = "f game_logo",
				LargeText = "f game_logo",
			},
			Instance = true
        };
        discord.GetActivityManager().UpdateActivity(activity, result => {
			//if (result != Discord.Result.Ok)
			//	GD.PrintErr("Error from discord (" + result.ToString() + ")");
			//else
			//	GD.Print("Discord Result = " + result.ToString());
		});

		discord?.RunCallbacks();
	}
}
