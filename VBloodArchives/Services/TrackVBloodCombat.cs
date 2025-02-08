using Bloody.Core.GameData.v1;
using System.Collections;
using System.Collections.Generic;
using ProjectM;
using System;
using System.Linq;

namespace SanguineArchives.VBloodArchives.Services;

public class TrackVBloodCombat
{
	// For each vblood in combat, keep track of the levels of aggro'd players. O(numVBlood * numPlayers)
	readonly Dictionary<string, Dictionary<string, float>> vbloodPlayerLevels = new();
	public static Dictionary<string, bool> vbloodStillRecovering = new();

    public TrackVBloodCombat()
    {
        Core.StartCoroutine(MonitorForPlayerLevels());
    }

    public void StartTrackingForVBlood(string vblood, ref Entity playerEntity, ref Entity vbloodEntity)
    {
	    if (!vbloodPlayerLevels.ContainsKey(vblood))
	    {
		    vbloodPlayerLevels[vblood] = new();
	    }
	    var characterName = playerEntity.Read<PlayerCharacter>().Name.ToString();
	    var user = GameData.Users.GetUserByCharacterName(characterName);
	    if (vbloodPlayerLevels[vblood].ContainsKey(user.CharacterName))
	    {
		    vbloodPlayerLevels[vblood][user.CharacterName] = Math.Max(vbloodPlayerLevels[vblood][user.CharacterName], user.Equipment.Level);
	    }
	    else
	    {
			vbloodPlayerLevels[vblood][user.CharacterName] = user.Equipment.Level;
	    }

	    if (vbloodPlayerLevels[vblood].Count > 1)
	    {
		    if (!Core.Prefabs.TryGetItem(vblood, out var vbloodPrefab)) return;
		    var vbloodLabel = ChatColor.Purple(vbloodPrefab.GetLocalizedName());
		    // More than 1 player fighting. Send warning about not recording fight.
		    var msg = $"Your fight with {vbloodLabel} will not be recorded. {ChatColor.Yellow(characterName)} joined the fight.";
		    // var msg = $"Fight not recorded. {ChatColor.Yellow(characterName)} joined the fight against {vbloodLabel}.";
		    Core.KillVBloodService.SendVBloodMessageToPlayers(vbloodPlayerLevels[vblood].Keys.ToList(), msg);
	    }
	    else
	    {
		    var vbloodLevel = 0;
		    if (Core.Prefabs.TryGetItem(vblood, out var prefabGuid))
		    {
			    if (Core.PrefabCollectionSystem._PrefabLookupMap.TryGetValue(prefabGuid, out var prefabEntity))
			    {
				    vbloodLevel = prefabEntity.Read<UnitLevel>().Level;
				    if (!Core.Prefabs.TryGetItem(vblood, out var vbloodPrefab)) return;
				    var vbloodLabel = ChatColor.Purple(vbloodPrefab.GetLocalizedName());
				    var userMaxLevel = vbloodPlayerLevels[vblood][user.CharacterName];
				    if (userMaxLevel > vbloodLevel)
				    {
					    var msg = $"Your fight with {vbloodLabel} will not be recorded. Your level is too high.";
					    // var msg = $"Fight not recorded. Your level is higher than {vbloodLabel}.";
					    Core.KillVBloodService.SendVBloodMessageToPlayers([characterName], msg);
				    }
				    else
				    {
					    if (vbloodStillRecovering.ContainsKey(vblood) && vbloodStillRecovering[vblood])
					    {
						    Core.KillVBloodService.startedWhileRecovering[vblood] = true;
							var msg = $"Your fight with {vbloodLabel} will not be recorded. Enemy not fully recovered yet.";
						    Core.KillVBloodService.SendVBloodMessageToPlayers([characterName], msg);
					    }
					    else
					    {
						    Core.KillVBloodService.startedWhileRecovering[vblood] = false;
						    var msg = $"Your fight with {vbloodLabel} started. Good luck!";
						    // var msg = $"Fight started. Good luck against {vbloodLabel}!";
						    Core.KillVBloodService.SendVBloodMessageToPlayers([characterName], msg);
					    }
				    }
			    }
		    }
	    }
    }

    public void StopTrackingForVBlood(string vblood, bool isDead)
    {
	    if (vbloodPlayerLevels.ContainsKey(vblood))
	    {
		    if (!isDead)
		    {
			    if (!Core.Prefabs.TryGetItem(vblood, out var vbloodPrefab)) return;
			    var vbloodLabel = ChatColor.Purple(vbloodPrefab.GetLocalizedName());
			    var msg = $"Your fight with {vbloodLabel} ended. Aggro was reset.";
			    // var msg = $"Fight ended. Aggro was lost for {vbloodLabel}.";
			    Core.KillVBloodService.SendVBloodMessageToPlayers(vbloodPlayerLevels[vblood].Keys.ToList(), msg);
		    }
			Core.KillVBloodService.SetMaxPlayerLevels(vblood, new Dictionary<string, float>(vbloodPlayerLevels[vblood]));
			vbloodPlayerLevels.Remove(vblood);
	    }
    }

    IEnumerator MonitorForPlayerLevels()
	{
		while(true)
		{
			foreach (var (vblood, cachedLevels) in vbloodPlayerLevels)
			{
				// Get VBlood Entity. Check AggroBuffer for Players.
				var usersOnline = GameData.Users.Online;
				foreach (var user in usersOnline)
				{
					if (cachedLevels.TryGetValue(user.CharacterName, out var cachedLevel))
					{
						var currentLevel = user.Equipment.Level;
						if (cachedLevel < currentLevel)
						{
							var vbloodLevel = 0;
							if (Core.Prefabs.TryGetItem(vblood, out var prefabGuid))
							{
								if (Core.PrefabCollectionSystem._PrefabLookupMap.TryGetValue(prefabGuid, out var prefabEntity))
								{
									vbloodLevel = prefabEntity.Read<UnitLevel>().Level;
									// Check if level was previously fine.
									if (cachedLevel <= vbloodLevel && currentLevel > vbloodLevel)
									{
										if (!Core.Prefabs.TryGetItem(vblood, out var vbloodPrefab)) break;
										var vbloodLabel = ChatColor.Purple(vbloodPrefab.GetLocalizedName());
										var msg = $"Your fight with {vbloodLabel} will not be recorded. A player level is too high ({ChatColor.Yellow(user.CharacterName)}).";
										// var msg = $"Fight not recorded. Player level ({ChatColor.Yellow(user.CharacterName)}) is higher than {vbloodLabel}.";
										Core.KillVBloodService.SendVBloodMessageToPlayers(vbloodPlayerLevels[vblood].Keys.ToList(), msg);
									}
								}
							}
						}
						cachedLevels[user.CharacterName] = Math.Max(cachedLevel, currentLevel);
					}
				}
			}
			yield return null;
		}
	}
}
