using System;
using SanguineArchives.Common.Commands.Converters;
using ProjectM.Network;
using System.Collections.Generic;
using SanguineArchives.VBloodArchives.Data;
using VampireCommandFramework;
using SanguineArchives.VBloodArchives.Services;

namespace SanguineArchives.VBloodArchives.Commands;

public class VBloodRecordsCommand
{
    public static void SendVBloodRecordsToChat(ChatCommandContext ctx, PrefabGUID vbloodPrefab, VBloodRecord record)
    {
        var vbloodLabel = ChatColor.Purple(vbloodPrefab.GetLocalizedName());
        var characterNameLabel = ChatColor.Yellow($"{record.CharacterName}");
        var durationLabel = ChatColor.Green($"{record.CombatDuration:F2}s");
        // var vbloodLevelLabel = ChatColor.Gray($"(Lvl {Core.VBloodRecordsService.GetVBloodLevel(vblood)})");
        var dateLabel = ChatColor.Gray($"({DateTime.Parse(record.DateTime).ToString("d")})");
        ctx.Reply($"{vbloodLabel}: {characterNameLabel} - {durationLabel} {dateLabel}");
    }

    [Command("vbloodrecordsboss", "vbrb", description: "Show records for a V Blood boss", adminOnly: false)]
    public static void ShowVBloodRecords(ChatCommandContext ctx, FoundVBlood foundVBlood)
    {
        var vbloodPrefab = foundVBlood.Value;
        var vblood = vbloodPrefab.GetPrefabName();
        var vbloodLabel = ChatColor.Purple(vbloodPrefab.GetLocalizedName());
        List<VBloodRecord> topRecords = Core.VBloodRecordsService.GetRecordsForVBlood(vblood);

        ctx.Reply(ChatColor.Green($"*** V Blood Records for {vbloodLabel} ***"));
        foreach (var vbloodRecord in topRecords)
        {
            SendVBloodRecordsToChat(ctx, vbloodPrefab, vbloodRecord);
        }
    }

    [Command("vbloodrecords", "vbr", description: "Show top records for V Blood", adminOnly: false)]
    public static void ShowTopRecords(ChatCommandContext ctx, int act = 0)
    {
        Dictionary<string, VBloodRecord> topRecords;

        if (act == 0)
        {
            ctx.Reply(ChatColor.Green($"*** Recent V Blood Records ***"));
            topRecords = Core.VBloodRecordsService.GetTopRecordsByDate(10);
        }
        else
        {
            var actLabel = $"Act {act}";
            ctx.Reply(ChatColor.Green($"*** {actLabel} V Blood Records ***"));
            switch (act)
            {
                case 1:
                    topRecords = Core.VBloodRecordsService.GetTopRecordsForVBloods(VBloodCollectionData.VBloods_Act1);
                    break;
                case 2:
                    topRecords = Core.VBloodRecordsService.GetTopRecordsForVBloods(VBloodCollectionData.VBloods_Act2);
                    break;
                case 3:
                    topRecords = Core.VBloodRecordsService.GetTopRecordsForVBloods(VBloodCollectionData.VBloods_Act3);
                    break;
                case 4:
                default:
                    topRecords = Core.VBloodRecordsService.GetTopRecordsForVBloods(VBloodCollectionData.VBloods_Act4);
                    break;
            }
        }

        foreach (var (vblood, record) in topRecords)
        {
            if (Core.Prefabs.TryGetItem(vblood, out var vbloodPrefab))
            {
                SendVBloodRecordsToChat(ctx, vbloodPrefab, record);
            }
        }
    }

    [Command("vbloodrecordsplayer", "vbrp", description: "Show V Blood records for player", adminOnly: false)]
    public static void ShowRecordsForPlayer(ChatCommandContext ctx, int act = 0, OnlinePlayer player = null)
    {
        var name = player?.Value.UserEntity.Read<User>().CharacterName ?? ctx.Event.User.CharacterName;
        var charEntity = player?.Value.CharEntity ?? ctx.Event.SenderCharacterEntity;

        var actLabel = $"Act {act}";
        var nameLabel = ChatColor.Yellow($"{name.ToString()}");
        Dictionary<string, VBloodRecord> playerRecords;

        if (act == 0)
        {
            ctx.Reply(ChatColor.Green($"*** Recent V Blood Records for {nameLabel} ***"));
            playerRecords = Core.VBloodRecordsService.GetRecordsForPlayerByDate(name.ToString(), 10);
        }
        else
        {
            ctx.Reply(ChatColor.Green($"*** {actLabel} V Blood Records for {nameLabel} ***"));
            switch (act)
            {
                case 1:
                    playerRecords = Core.VBloodRecordsService.GetTopRecordsForVBloods(VBloodCollectionData.VBloods_Act1);
                    break;
                case 2:
                    playerRecords = Core.VBloodRecordsService.GetTopRecordsForVBloods(VBloodCollectionData.VBloods_Act2);
                    break;
                case 3:
                    playerRecords = Core.VBloodRecordsService.GetTopRecordsForVBloods(VBloodCollectionData.VBloods_Act3);
                    break;
                case 4:
                default:
                    playerRecords = Core.VBloodRecordsService.GetTopRecordsForVBloods(VBloodCollectionData.VBloods_Act4);
                    break;
            }
        }

        foreach (var (vblood, record) in playerRecords)
        {
            if (Core.Prefabs.TryGetItem(vblood, out var vbloodPrefab))
            {
                SendVBloodRecordsToChat(ctx, vbloodPrefab, record);
            }
        }
    }
}
