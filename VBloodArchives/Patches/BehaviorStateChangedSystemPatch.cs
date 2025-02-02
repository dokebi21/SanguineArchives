using HarmonyLib;
using ProjectM;
using ProjectM.Behaviours;
using ProjectM.Gameplay.Systems;
using Unity.Collections;
using Unity.Entities;
using System.Collections;
using SanguineArchives.Common.BloodyNotify.Systems;
using SanguineArchives.VBloodArchives.Services;
using Stunlock.Core;

namespace SanguineArchives.VBloodArchives.Patches;

 [HarmonyPatch(typeof(CreateGameplayEventOnBehaviourStateChangedSystem), nameof(CreateGameplayEventOnBehaviourStateChangedSystem.OnUpdate))]
internal static class BehaviorStateChangedSystemPatch
{
    static void Prefix(CreateGameplayEventOnBehaviourStateChangedSystem __instance)
    {
        NativeArray<Entity> entities = __instance.__query_221632411_0.ToEntityArray(Allocator.Temp);
        try
        {
            foreach (Entity entity in entities)
            {
                if (!entity.Has<BehaviourTreeStateChangedEvent>()) continue;
                var behaviourTreeStateChangedEvent = entity.Read<BehaviourTreeStateChangedEvent>();
                if (!behaviourTreeStateChangedEvent.Entity.Has<VBloodUnit>()) continue;
                var prevState = behaviourTreeStateChangedEvent.PreviousState;
                var newState = behaviourTreeStateChangedEvent.NewState;
                var vbloodEntity = behaviourTreeStateChangedEvent.Entity;
                var health = vbloodEntity.Read<Health>();
                var vbloodPrefabGUID = vbloodEntity.Read<PrefabGUID>();
                var vbloodString = Core.PrefabCollectionSystem.PrefabGuidToNameDictionary[vbloodPrefabGUID];
                Core.Log.LogInfo($"BehaviourTreeStateChangedEvent: {vbloodString} - {prevState}->{newState} HP:{health.Value}/{health.MaxHealth.Value}");
                if ((prevState & GenericEnemyState.AnyCombat) != 0 && (newState & GenericEnemyState.AnyCombat) == 0)
                {
                    Core.StartCoroutine(WaitForFullRecovery(vbloodString, vbloodEntity));
                }
            }
        }
        finally
        {
            entities.Dispose();
        }
    }
    
    static IEnumerator WaitForFullRecovery(string vbloodString, Entity vbloodEntity)
    {
        var health = vbloodEntity.Read<Health>();
        var isFullHealth = health.Value >= health.MaxHealth.Value;
        var currState = vbloodEntity.Read<BehaviourTreeState>().Value;
        while ((currState & GenericEnemyState.AnyCombat) == 0 && !isFullHealth)
        {
            TrackVBloodCombat.vbloodStillRecovering[vbloodString] = !isFullHealth;
            health = vbloodEntity.Read<Health>();
            isFullHealth = health.Value >= health.MaxHealth.Value;
            currState = vbloodEntity.Read<BehaviourTreeState>().Value;
            yield return null; // Wait for the next frame
        }
        TrackVBloodCombat.vbloodStillRecovering[vbloodString] = false;
        Core.Log.LogInfo($"WaitForFullRecovery: {vbloodString} - {currState} HP:{health.Value}/{health.MaxHealth.Value} - Done");
    }
}
