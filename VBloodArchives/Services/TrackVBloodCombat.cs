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
	    var userEntity = playerEntity.Read<PlayerCharacter>().UserEntity;
	    var player = new Player(userEntity);
	    var playerLevel = Helper.GetPlayerEquipmentLevel(player);
	    if (vbloodPlayerLevels[vblood].ContainsKey(player.Name))
	    {
		    vbloodPlayerLevels[vblood][player.Name] = Math.Max(vbloodPlayerLevels[vblood][player.Name], playerLevel);
	    }
	    else
	    {
			vbloodPlayerLevels[vblood][player.Name] = playerLevel;
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
				    var userMaxLevel = vbloodPlayerLevels[vblood][player.Name];
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
				var usersOnline = Core.Players.GetCachedUsersOnline();

				foreach (var user in usersOnline)
				{
					Player player = new(user);
					if (cachedLevels.TryGetValue(player.Name, out var cachedLevel))
					{

						var currentLevel = Helper.GetPlayerEquipmentLevel(player);
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
										var msg = $"Your fight with {vbloodLabel} will not be recorded. A player level is too high ({ChatColor.Yellow(player.Name)}).";
										// var msg = $"Fight not recorded. Player level ({ChatColor.Yellow(user.CharacterName)}) is higher than {vbloodLabel}.";
										Core.KillVBloodService.SendVBloodMessageToPlayers(vbloodPlayerLevels[vblood].Keys.ToList(), msg);
									}
								}
							}
						}
						cachedLevels[player.Name] = Math.Max(cachedLevel, currentLevel);
					}
				}
			}
			yield return null;
		}
	}
}
