using HarmonyLib;
using ProjectM;
using Stunlock.Core;
using Unity.Collections;
using Unity.Entities;

namespace SanguineArchives.VBloodArchives.Patches;

[HarmonyPatch]
internal static class PlayerCombatBuffSystemPatch
{
    [HarmonyPatch(typeof(PlayerCombatBuffSystem_OnAggro), nameof(PlayerCombatBuffSystem_OnAggro.OnUpdate))]
    [HarmonyPrefix]
    static void OnUpdate_Prefix(PlayerCombatBuffSystem_OnAggro __instance)
    {
        NativeArray<Entity> entities = __instance.EntityQueries[0].ToEntityArray(Allocator.Temp);
        try
        {
            foreach (Entity entity in entities)
            {
                if (!entity.Has<InverseAggroEvents.Added>()) continue;
                var inverseAggroEvent = entity.Read<InverseAggroEvents.Added>();
                if (!inverseAggroEvent.Producer.Has<PlayerCharacter>()) continue;
                if (!inverseAggroEvent.Consumer.Has<VBloodUnit>()) continue;
                var vbloodPrefabGUID = inverseAggroEvent.Consumer.Read<PrefabGUID>();
                var vbloodString = vbloodPrefabGUID.GetPrefabName();
                Core.TrackVBloodCombat.StartTrackingForVBlood(vbloodString, ref inverseAggroEvent.Producer, ref inverseAggroEvent.Consumer);
                Core.Log.LogInfo($"PlayerCombatBuffSystemPatch: Start Tracking {vbloodString} for {inverseAggroEvent.Producer}...");
            }
        }
        finally
        {
            entities.Dispose();
        }
    }

    [HarmonyPatch(typeof(PlayerCombatBuffSystem_Reapplication), nameof(PlayerCombatBuffSystem_Reapplication.OnUpdate))]
    [HarmonyPrefix]
    static void OnUpdate_Prefix(PlayerCombatBuffSystem_Reapplication __instance)
    {

    }
}
