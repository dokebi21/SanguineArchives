using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanguineArchives.Common.Commands.Converters;
using SanguineArchives.Common.Utils;
using ProjectM;
using ProjectM.Network;
using Unity.Collections;
using Unity.Transforms;
using UnityEngine;
using VampireCommandFramework;

namespace SanguineArchives.Common.Commands;

internal class DeadlyModeCommands
{
	[Command("deadlymodetoggle", "dmt", description: "Toggle deadly mode", adminOnly: false)]
	public static void ToggleDeadlyMode(ChatCommandContext ctx)
	{
		ToggleDeadlyModeForPlayer(ctx);
	}
	
	[Command("deadlymodetoggleplayer", "dmtp", description: "Toggle deadly mode", adminOnly: true)]
	public static void ToggleDeadlyModeForPlayer(ChatCommandContext ctx, OnlinePlayer player = null)
	{
		var name = player?.Value.UserEntity.Read<User>().CharacterName ?? ctx.Event.User.CharacterName;
		var charEntity = player?.Value.CharEntity ?? ctx.Event.SenderCharacterEntity;

		if (Helper.IsPlayerInCombat(charEntity))
		{
			ctx.Reply($"Cannot toggle Deadly Mode for <color=white>{name}</color>. Player is in combat.");
			return;
		}

		if (Core.DeadlyModeService.ToggleDeadlyMode(charEntity))
		{
			ctx.Reply($"Deadly Mode ON for <color=white>{name}</color>. Deal and take more damage.");
		}
		else
		{
			ctx.Reply($"Deadly Mode OFF for <color=white>{name}</color>");
		}
		Core.DeadlyModeService.UpdateDeadlyModePlayer(charEntity);
	}
	
	[Command("deadlymodeplayers", "dmp", description: "Show players in deadly mode", adminOnly: false)]
	public static void ListDeadlyModePlayers(ChatCommandContext ctx)
	{
		var deadlyPlayers = Core.DeadlyModeService.GetDeadlyModePlayers();
		if (deadlyPlayers == null)
		{
			ctx.Reply("No players are playing on Deadly Mode");
			return;
		}
		
		var sb = new StringBuilder();
		sb.Append("Deadly Mode players: ");
		var first = true;
		foreach (var player in deadlyPlayers)
		{
			var playerCharacter = player.Read<PlayerCharacter>();
			var name = $"<color=white>{playerCharacter.Name}</color>";
			if (sb.Length + name.Length + 2 > Core.MAX_REPLY_LENGTH)
			{
				ctx.Reply(sb.ToString());
				sb.Clear();
				first = true;
			}
			if (first)
				sb.Append(name);
			else
				sb.Append($", {name}");
			first = false;
		}
		ctx.Reply(sb.ToString());
	}
}
