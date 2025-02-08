using Unity.Collections;
using HarmonyLib;
using ProjectM;
using System;
using System.Collections.Generic;
using ProjectM.Network;

namespace SanguineArchives.VBloodArchives.Patches;


[HarmonyPatch]
internal class DeathVBloodPatch
{
    [HarmonyPatch(typeof (VBloodSystem), nameof(VBloodSystem.OnUpdate))]
    [HarmonyPrefix]
    private static void OnUpdate_Prefix(VBloodSystem __instance)
    {
        try
        {
            OnDeathVBlood(__instance, __instance.EventList);
        }
        catch (Exception ex)
        {
            Core.Log.LogError(ex);
        }
    }

    private const double SendMessageDelay = 2;
    private static bool _checkKiller = false;

    public static void OnDeathVBlood(VBloodSystem sender, NativeList<VBloodConsumed> deathEvents)
    {
        if (deathEvents.Length > 0)
        {
            foreach (var deathEvent in deathEvents)
            {
                if (Core.EntityManager.HasComponent<PlayerCharacter>(deathEvent.Target))
                {
                    var player = Core.EntityManager.GetComponentData<PlayerCharacter>(deathEvent.Target);
                    var user = Core.EntityManager.GetComponentData<User>(player.UserEntity);
                    var vbloodString = Core.PrefabCollectionSystem.PrefabGuidToNameDictionary[deathEvent.Source];
                    Core.KillVBloodService.AddKiller(vbloodString.ToString(), user.CharacterName.ToString());
                    Core.KillVBloodService.lastKillerUpdate[vbloodString.ToString()] = DateTime.Now;
                    _checkKiller = true;
                }
            }
        }
        else if (_checkKiller)
        {
            var didSkip = false;
            foreach (KeyValuePair<string, DateTime> kvp in Core.KillVBloodService.lastKillerUpdate)
            {
                var lastUpdateTime = kvp.Value;
                if (DateTime.Now - lastUpdateTime < TimeSpan.FromSeconds(SendMessageDelay))
                {
                    didSkip = true;
                    continue;
                }
                Core.KillVBloodService.AnnounceVBloodKill(kvp.Key);
            }
            _checkKiller = didSkip;
        }
    }
}
