using Bloodstone.API;
using Bloody.Core.GameData.v1;
using SanguineArchives.Common.BloodyNotify.DB;
using SanguineArchives.Common.Utils;
using SanguineArchives.VBloodArchives.Services;
using ProjectM.Network;
using ProjectM;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Unity.Collections;
using Unity.Entities;

namespace SanguineArchives.Common.BloodyNotify.Systems
{
    internal class KillVBloodSystem
    {
        private const double SendMessageDelay = 2;
        public static Dictionary<string, HashSet<string>> vbloodKills = new();
        private static EntityManager _entityManager = Core.EntityManager;
        private static PrefabCollectionSystem _prefabCollectionSystem = Core.PrefabCollectionSystem;
        private static bool checkKiller = false;
        private static Dictionary<string, DateTime> lastKillerUpdate = new();
        public static Dictionary<string, float> combatDuration = new();
        public static Dictionary<string, Dictionary<string, float>> maxPlayerLevels = new();
        public static Dictionary<string, bool> startedWhileRecovering = new();

        public static void OnDetahVblood(VBloodSystem sender, NativeList<VBloodConsumed> deathEvents)
        {
            if (deathEvents.Length > 0)
            {
                foreach (var event_vblood in deathEvents)
                {
                    if (_entityManager.HasComponent<PlayerCharacter>(event_vblood.Target))
                    {
                        var player = _entityManager.GetComponentData<PlayerCharacter>(event_vblood.Target);
                        var user = _entityManager.GetComponentData<User>(player.UserEntity);
                        var vbloodString = _prefabCollectionSystem.PrefabGuidToNameDictionary[event_vblood.Source];
                        AddKiller(vbloodString.ToString(), user.CharacterName.ToString());
                        lastKillerUpdate[vbloodString.ToString()] = DateTime.Now;
                        checkKiller = true;
                    }
                }
            }
            else if (checkKiller)
            {
                var didSkip = false;
                foreach (KeyValuePair<string, DateTime> kvp in lastKillerUpdate)
                {
                    var lastUpdateTime = kvp.Value;
                    if (DateTime.Now - lastUpdateTime < TimeSpan.FromSeconds(SendMessageDelay))
                    {
                        didSkip = true;
                        continue;
                    }
                    AnnounceVBloodKill(kvp.Key);
                }
                checkKiller = didSkip;
            }
        }

        public static void SetCombatDuration(string vblood, float duration)
        {
            combatDuration[vblood] = duration;
        }
        
        public static void SetMaxPlayerLevels(string vblood, Dictionary<string, float> playerLevels)
        {
            maxPlayerLevels[vblood] = playerLevels;
        }

        public static void ResetStartedWhileRecovering(string vblood)
        {
            startedWhileRecovering.Remove(vblood);
        }

        public static void AddKiller(string vblood, string killerCharacterName)
        {
            if (!vbloodKills.ContainsKey(vblood))
            {
                vbloodKills[vblood] = new HashSet<string>();
            }
            vbloodKills[vblood].Add(killerCharacterName);
        }

        public static void RemoveKillers(string vblood)
        {
            vbloodKills[vblood] = new HashSet<string>();
            SetCombatDuration(vblood, 0);
            maxPlayerLevels.Remove(vblood);
            startedWhileRecovering.Remove(vblood);
        }

        public static List<string> GetKillers(string vblood)
        {
            return vbloodKills[vblood].ToList();
        }
        
        /**
         * Player levels must not be above V Blood level during the fight.
         */
        public static bool CheckPlayerLevelsDuringCombat(string vblood)
        {
            var vbloodLevel = 0;
            if (Core.Prefabs.TryGetItem(vblood, out var prefabGuid))
            {
                if (Core.PrefabCollectionSystem._PrefabLookupMap.TryGetValue(prefabGuid, out var prefabEntity))
                {
                    vbloodLevel = prefabEntity.Read<UnitLevel>().Level;
                }
            }
            
            float maxLevel = -1;
            foreach (var (characterName, playerLevel) in maxPlayerLevels[vblood])
            {
                maxLevel = Math.Max(maxLevel, playerLevel);
            }
            return maxLevel <= vbloodLevel;
        }

        public static bool CheckStartedWhileRecovering(string vblood)
        {
            Core.Log.LogInfo($"CheckStartedWhileRecovering: {startedWhileRecovering.ContainsKey(vblood)} vs {startedWhileRecovering.ContainsKey(vblood) && startedWhileRecovering[vblood]}");
            return startedWhileRecovering.ContainsKey(vblood) && startedWhileRecovering[vblood];
        }

        public static void AnnounceVBloodKill(string vblood)
        {
            if (CheckIfBloodyBoss(vblood))
            {
                RemoveKillers(vblood);
                return;
            }
            if (Database.getPrefabIgnoreValue(vblood))
            {
                RemoveKillers(vblood);
                return;
            }
            
            var killers = GetKillers(vblood);
            
            if (killers.Count == 0)
            {
                RemoveKillers(vblood);
                return;
            }
            
            combatDuration.TryGetValue(vblood, out var combatDurationSeconds);
            var killersLabel = ChatColor.Yellow(CombinedKillersLabel(vblood));
            var vbloodLabel = ChatColor.Purple(Database.getPrefabNameValue(vblood));
            // var basePrefixLabel = ChatColor.Green("Congratulations!");
            // var baseSuffixLabel = $"{vbloodLabel} was defeated by {killersLabel} in {combatDurationSeconds:F2} seconds!";
            var defaultCongratsMessage =
                ChatColor.Green(
                    $"Congratulations to {killersLabel} for defeating {vbloodLabel} in {combatDurationSeconds:F2} seconds!");
            Core.Log.LogInfo($"AnnounceVBloodKill: VBlood {vblood} was killed by {killersLabel}!");
            
            if (killers.Count == 1 && maxPlayerLevels[vblood].Count == 1)
            {
                var newRecord = new VBloodRecord
                {
                    CharacterName = killers[0],
                    DateTime = DateTime.Now.ToString("o"),
                    CombatDuration = combatDurationSeconds,
                };

                if (newRecord.CombatDuration == 0)
                {
                    SendVBloodMessageToAll(defaultCongratsMessage);
                    RemoveKillers(vblood);
                    return;
                }

                if (!CheckPlayerLevelsDuringCombat(vblood) || CheckStartedWhileRecovering(vblood))
                {
                    SendVBloodMessageToAll(defaultCongratsMessage);
                    RemoveKillers(vblood);
                    return;
                }
                
                if (Core.VBloodRecordsService.IsNewTopRecord(vblood, newRecord))
                {
                    SendVBloodMessageToAll(defaultCongratsMessage);
                    var newTopRecordLabel = ChatColor.Green($"** A new top record has been set! **");
                    SendVBloodMessageToAll(newTopRecordLabel);
                    if (Core.VBloodRecordsService.TryGetTopRecordForVBlood(vblood, out VBloodRecord topRecord))
                    {
                        var difference = Core.VBloodRecordsService.GetCurrentTopRecord(vblood) - combatDurationSeconds;
                        var differenceLabel = ChatColor.Green($"{difference:F2}");
                        var topRecordNoticeLabel = $"The new record is faster by {differenceLabel} seconds.";
                        SendVBloodMessageToAll(topRecordNoticeLabel);
                    }
                }
                else
                {
                    SendVBloodMessageToAll(defaultCongratsMessage);
                }

                if (Core.VBloodRecordsService.IsNewPlayerRecord(vblood, newRecord))
                {
                    var personalRecordPrefixLabel = ChatColor.Green("** A new personal record has been set! **");
                    SendVBloodMessageToPlayers(killers, personalRecordPrefixLabel);
                    if (Core.VBloodRecordsService.TryGetPlayerRecordForVBlood(vblood, newRecord.CharacterName,
                            out var currentPlayerRecord))
                    {
                        var difference = Core.VBloodRecordsService.GetCurrentPlayerRecord(vblood, newRecord.CharacterName) - combatDurationSeconds;
                        var differenceLabel = ChatColor.Green($"{difference:F2}");
                        var personalRecordSuffixLabel = $"Your new record is faster by {differenceLabel} seconds.";
                        SendVBloodMessageToPlayers(killers, personalRecordSuffixLabel);    
                    }
                }
                Core.VBloodRecordsService.AddRecord(vblood, newRecord);
            }
            if (killers.Count > 1 || maxPlayerLevels[vblood].Count > 1)
            {
                // More than 1 killer or fighter.
                SendVBloodMessageToAll(defaultCongratsMessage);
                // var notRecordedLabel = Database.getDefaultAnnounceValue("VBloodRecords_CoopNotRecorded");
                // SendVBloodMessageToPlayers(killers, FontColorChatSystem.Gray($"{notRecordedLabel}"));
            }
            RemoveKillers(vblood);
        }

        /**
         * Send message to all users who didn't turn off VBlood notifications. 
         */
        public static void SendVBloodMessageToAll(string message)
        {
            var usersOnline = GameData.Users.Online;
            foreach (var user in usersOnline)
            {
                var isUserIgnore = Database.getVBloodNotifyIgnore(user.CharacterName);
                if (!isUserIgnore)
                {
                    ServerChatUtils.SendSystemMessageToClient(VWorld.Server.EntityManager, user.Internals.User.Value, ChatColor.Gray(message));
                }
            }
        }
        
        /**
         * Send message to killers who didn't turn off VBlood notifications.
         */
        public static void SendVBloodMessageToPlayers(List<string> players, string message)
        {
            var usersOnline = GameData.Users.Online;
            foreach (var user in usersOnline)
            {
                var isUserIgnore = Database.getVBloodNotifyIgnore(user.CharacterName);
                if (!isUserIgnore && players.Contains(user.CharacterName))
                {
                    ServerChatUtils.SendSystemMessageToClient(VWorld.Server.EntityManager, user.Internals.User.Value, ChatColor.Gray(message));
                }
            }
        }

        private static string CombinedKillersLabel(string vblood)
        {
            var killers = GetKillers(vblood);
            var sbKillersLabel = new StringBuilder();
            if (killers.Count == 0) return null;
            if (killers.Count == 1)
            {
                sbKillersLabel.Append(ChatColor.Yellow(killers[0]));
            }
            if (killers.Count == 2)
            {
                sbKillersLabel.Append($"{ChatColor.Yellow(killers[0])} {Database.getVBloodFinalConcatCharacters()} {ChatColor.Yellow(killers[1])}");
            }
            if (killers.Count > 2)
            {
                for (int i = 0; i < killers.Count; i++)
                {
                    if (i == killers.Count - 1)
                    {
                        sbKillersLabel.Append($"{Database.getVBloodFinalConcatCharacters()} {ChatColor.Yellow(killers[i])}");
                    }
                    else
                    {
                        sbKillersLabel.Append($"{ChatColor.Yellow(killers[i])}, ");
                    }
                }
            }
            return sbKillersLabel.ToString();
        }

        private static bool CheckIfBloodyBoss(string vblood)
        {
            var entitiesQuery = Bloody.Core.Helper.v1.QueryComponents.GetEntitiesByComponentTypes<VBloodUnit, NameableInteractable, LifeTime>(EntityQueryOptions.Default, false);
            foreach (var entity in entitiesQuery)
            {
                try
                {
                    var npc = GameData.Npcs.FromEntity(entity);
                    var vbloodString = _prefabCollectionSystem.PrefabGuidToNameDictionary[npc.PrefabGUID];
                    if (vbloodString == vblood)
                    {
                        NameableInteractable _nameableInteractable = entity.Read<NameableInteractable>();
                        if (_nameableInteractable.Name.Value.Contains("bb"))
                        {
                            var health = entity.Read<Health>();
                            if (health.IsDead)
                            {
                                entitiesQuery.Dispose();
                                return true;
                            }

                            entitiesQuery.Dispose();
                            return false;
                        }
                    }
                } catch {
                    continue;
                }
            }

            entitiesQuery.Dispose();
            return false;
        }
    }
}
