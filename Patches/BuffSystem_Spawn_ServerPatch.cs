using HarmonyLib;
using ProjectM;
using Stunlock.Core;
using Unity.Collections;
using Unity.Entities;
using SanguineArchives.VBloodRecords.Data;

namespace SanguineArchives.Patches;

[HarmonyPatch(typeof(BuffSystem_Spawn_Server), nameof(BuffSystem_Spawn_Server.OnUpdate))]
public static class BuffSystem_Spawn_ServerPatch
{
	public static void Prefix(BuffSystem_Spawn_Server __instance)
	{
		if (Core.DeadlyModeService == null) return;

		EntityManager entityManager = __instance.EntityManager;
		NativeArray<Entity> entities = __instance.__query_401358634_0.ToEntityArray(Allocator.Temp);

		foreach (var buffEntity in entities)
		{
			var prefabGUID = buffEntity.Read<PrefabGUID>();
			Entity owner = buffEntity.Read<EntityOwner>().Owner;
			if (!owner.Has<PlayerCharacter>()) continue;
			if (!Core.DeadlyModeService.IsDeadlyModePlayer(owner)) continue;
			if (prefabGUID == Data.Prefabs.DeadlyModeBuff)
			{
				Core.DeadlyModeService.UpdateDeadlyModeBuff(buffEntity);
			}
		}
	}
}
