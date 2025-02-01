using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System;
using SanguineArchives.Common.KindredCommands.Data;
using Unity.Entities;
using ProjectM;
using System.Linq;

namespace SanguineArchives.VBloodArchives.Services
{
    public struct VBloodRecord
    {
        public string CharacterName { get; set; }
        public string DateTime { get; set; }
        public float CombatDuration { get; set; }
    }

    public class VBloodRecordsService
    {
        /**
         * CombatRecord for each VBlood.
         */
        public Dictionary<string, List<VBloodRecord>> vbloodRecords = new();
        
        protected static readonly string CONFIG_PATH = Path.Combine(BepInEx.Paths.ConfigPath, MyPluginInfo.PLUGIN_NAME);
        protected string RecordsConfigPath => Path.Combine(CONFIG_PATH, "vblood_combat_records.json");

        public VBloodRecordsService()
        {
            LoadRecords();
        }

        public float GetVBloodLevel(string vblood)
        {
            var vbloodLevel = 0;
            if (Core.Prefabs.TryGetItem(vblood, out var prefabGuid))
            {
                if (Core.PrefabCollectionSystem._PrefabLookupMap.TryGetValue(prefabGuid, out var prefabEntity))
                {
                    vbloodLevel = prefabEntity.Read<UnitLevel>().Level;
                }
            }
            return vbloodLevel;
        }

        /**
         * If player level is above boss level, the record is not valid.
         * Use Max Player Level eventually to avoid cheating.
         */
        public bool IsPlayerLevelAcceptable(string vblood, string characterName)
        {
            if (Core.Players.TryFindName(characterName.ToLower(), out var playerData))
            {
                var charEntity = playerData.CharEntity;
                var equipment = charEntity.Read<Equipment>();
                var characterLevel = equipment.ArmorLevel + equipment.SpellLevel + equipment.WeaponLevel;
                var vbloodLevel = GetVBloodLevel(vblood);
                return characterLevel <= vbloodLevel;
            }
            return false;
        }

        /**
         * Only the top player record for a vblood is saved.
         */
        public void AddRecord(string vblood, VBloodRecord newRecord)
        {
            var index = vbloodRecords[vblood].FindIndex(r => r.CharacterName == newRecord.CharacterName);
            if (index == -1)
            {
                vbloodRecords[vblood].Add(newRecord);
            }
            else
            {
                var currentRecord = vbloodRecords[vblood][index];
                if (newRecord.CombatDuration < currentRecord.CombatDuration)
                {
                    vbloodRecords[vblood][index] = newRecord;
                }
            }
            SaveRecords();
        }
        
        public List<VBloodRecord> GetRecordsForVBlood(string vblood)
        {
            if (vbloodRecords.ContainsKey(vblood))
            {
                var sortedRecords = vbloodRecords[vblood].OrderBy(kvp => kvp.CombatDuration).ToList();
                return sortedRecords;
            }
            return new List<VBloodRecord>();
        }

        /**
         * Returns the records for the player.
         */
        public Dictionary<string, VBloodRecord> GetRecordsForPlayer(string characterName)
        {
            Dictionary<string, VBloodRecord> playerRecords = new();
            foreach (var kvp in vbloodRecords)
            {
                var vblood = kvp.Key;
                var index = vbloodRecords[vblood].FindIndex(r => r.CharacterName == characterName);
                if (index != -1)
                {
                    playerRecords.Add(vblood, vbloodRecords[vblood][index]);
                }
            }
            return playerRecords;
        }
        
        public Dictionary<string, VBloodRecord> GetRecordsForPlayerByDate(string characterName, int count)
        {
            var playerRecords = GetRecordsForPlayer(characterName);
            var sortedByDate = playerRecords.OrderByDescending(kvp => kvp.Value.DateTime).Take(count);
            return sortedByDate.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        
        
        public Dictionary<string, VBloodRecord> GetTopRecords()
        {
            Dictionary<string, VBloodRecord> topRecords = new();
            foreach (var kvp in vbloodRecords)
            {
                // Find lowest the CombatDuration for the vblood boss.
                var vblood = kvp.Key;
                if (vbloodRecords[vblood].Count == 0) continue;
                var topRecord = vbloodRecords[vblood][0];
                foreach (var record in vbloodRecords[vblood])
                {
                    if (topRecord.CombatDuration > record.CombatDuration)
                    {
                        topRecord = record;
                    }
                }
                topRecords[vblood] = topRecord;
            }
            return topRecords;
        }
        
        public Dictionary<string, VBloodRecord> GetTopRecordsByDate(int count)
        {
            var topRecords = GetTopRecords();
            var sortedByDate = topRecords.OrderByDescending(kvp => kvp.Value.DateTime).Take(count);
            return sortedByDate.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        
        public Dictionary<string, VBloodRecord> GetTopRecordsForVBloods(List<string> vbloods)
        {
            Dictionary<string, VBloodRecord> topRecords = new();
            foreach (var vblood in vbloods)
            {
                if (vbloodRecords[vblood].Count == 0) continue;
                var topRecord = vbloodRecords[vblood][0];
                foreach (var record in vbloodRecords[vblood])
                {
                    if (topRecord.CombatDuration > record.CombatDuration)
                    {
                        topRecord = record;
                    }
                }
                topRecords[vblood] = topRecord;
            }
            return topRecords;
        }

        
        /**
         * Check if new record is better than current top record,
         */
        public bool IsNewTopRecord(string vblood, VBloodRecord record)
        {
            if (TryGetTopRecordForVBlood(vblood, out var topRecord))
            {
                return record.CombatDuration < topRecord.CombatDuration;
            }
            return true;
        }

        public float GetCurrentPlayerRecord(string vblood, string characterName)
        {
            if (TryGetPlayerRecordForVBlood(vblood, characterName, out var record))
            {
                return record.CombatDuration;
            }

            return -1;
        }
        
        public float GetCurrentTopRecord(string vblood)
        {
            if (TryGetTopRecordForVBlood(vblood, out var record))
            {
                return record.CombatDuration;
            }

            return -1;
        }
        
        /**
         * Check if new record is better than current player record,
         */
        public bool IsNewPlayerRecord(string vblood, VBloodRecord record)
        {
            if (TryGetPlayerRecordForVBlood(vblood, record.CharacterName, out var topRecord))
            {
                return record.CombatDuration < topRecord.CombatDuration;
            }
            return true;
        }

        public bool TryGetPlayerRecordForVBlood(string vblood, string characterName, out VBloodRecord playerRecord)
        {
            if (vbloodRecords[vblood].Count == 0)
            {
                playerRecord = default;
                return false;
            }
            var index = vbloodRecords[vblood].FindIndex(r => r.CharacterName == characterName);
            if (index == -1)
            {
                playerRecord = default;
                return false;
            }
            playerRecord = vbloodRecords[vblood][index];
            return true;
        }
        
        public bool TryGetTopRecordForVBlood(string vblood, out VBloodRecord topRecord)
        {
            if (vbloodRecords[vblood].Count == 0)
            {
                topRecord = default;
                return false;
            }
            topRecord = vbloodRecords[vblood][0];
            foreach (var record in vbloodRecords[vblood])
            {
                if (topRecord.CombatDuration > record.CombatDuration)
                {
                    topRecord = record;
                }
            }
            return true;
        }

        public string formatDateTime(string dateTime)
        {
            DateTime parsedDateTime = DateTime.Parse(dateTime);
            return parsedDateTime.ToString("yyyy-MM-dd hh:mm tt");
        }

        public void SaveRecords()
        {
            if (!Directory.Exists(CONFIG_PATH))
            {
                Directory.CreateDirectory(CONFIG_PATH);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true // Makes the JSON output human-readable
            };

            var jsonString = JsonSerializer.Serialize(vbloodRecords, options);
            File.WriteAllText(RecordsConfigPath, jsonString);
        }
        
        public void LoadRecords()
        {
            if (File.Exists(RecordsConfigPath))
            {
                var jsonString = File.ReadAllText(RecordsConfigPath);
                vbloodRecords = JsonSerializer.Deserialize<Dictionary<string, List<VBloodRecord>>>(jsonString) ?? new Dictionary<string, List<VBloodRecord>>();
            }
            else
            {
                SaveRecords();
            }
        }
    }
}