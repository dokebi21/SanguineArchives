using System.Collections;
using System.Collections.Generic;
using SanguineArchives.Common.KindredCommands.Data;
using SanguineArchives.Common.Utils;
using ProjectM;
using Unity.Entities;

namespace SanguineArchives.Services
{
	internal class DeadlyModeService
	{
		readonly HashSet<Entity> deadlyModePlayers = [];

		public DeadlyModeService()
		{
			foreach(var charEntity in Helper.GetEntitiesByComponentType<PlayerCharacter>(includeDisabled: true))
			{
				LoadDeadlyModePlayers(charEntity);
			}
		}
		
		public bool IsDeadlyModePlayer(Entity charEntity)
		{
			return deadlyModePlayers.Contains(charEntity);
		}
		
		public IEnumerable<Entity> GetDeadlyModePlayers()
		{
			foreach (var charEntity in Helper.GetEntitiesByComponentType<PlayerCharacter>(includeDisabled: true))
			{
				if (IsDeadlyModePlayer(charEntity))
					yield return charEntity;
			}
		}
		
		public bool ToggleDeadlyMode(Entity charEntity)
		{
			if (IsDeadlyModePlayer(charEntity))
			{
				deadlyModePlayers.Remove(charEntity);
				return false;
			}
			deadlyModePlayers.Add(charEntity);
			return true;
		}

		void LoadDeadlyModePlayers(Entity charEntity)
		{
			if (BuffUtility.TryGetBuff(Core.Server.EntityManager, charEntity, Prefabs.DeadlyModeBuff, out var buffEntity) &&
			    buffEntity.Has<ModifyUnitStatBuff_DOTS>())
			{
				foreach (var buff in buffEntity.ReadBuffer<ModifyUnitStatBuff_DOTS>())
				{
					switch (buff.StatType)
					{
						case UnitStatType.ResistVsVampires:
							deadlyModePlayers.Add(charEntity);
							break;
					}
				}
			}
		}
		
		public void UpdateDeadlyModePlayer(Entity charEntity)
		{
			if(!IsDeadlyModePlayer(charEntity))
			{
				ClearDeadlyModeBuff(charEntity);
				return;
			}

			var userEntity = charEntity.Read<PlayerCharacter>().UserEntity;
			Core.StartCoroutine(RemoveAndAddDeadlyModeBuff(userEntity, charEntity));
		}
		
		IEnumerator RemoveAndAddDeadlyModeBuff(Entity userEntity, Entity charEntity)
		{
			Buffs.RemoveBuff(charEntity, Prefabs.DeadlyModeBuff);
			while (BuffUtility.HasBuff(Core.EntityManager, charEntity, Prefabs.DeadlyModeBuff))
				yield return null;

			Buffs.AddBuff(userEntity, charEntity, Prefabs.DeadlyModeBuff, -1, true);
		}


		public void UpdateDeadlyModeBuff(Entity buffEntity)
		{
			var charEntity = buffEntity.Read<EntityOwner>().Owner;
			var modifyStatBuffer = Core.EntityManager.AddBuffer<ModifyUnitStatBuff_DOTS>(buffEntity);
			modifyStatBuffer.Clear();
			
			if (Core.DeadlyModeService.IsDeadlyModePlayer(charEntity))
			{
				foreach (var deadlyModeBuff in DeadlyModeService.deadlyModeBuffs)
				{
					var modifiedBuff = deadlyModeBuff;
					modifyStatBuffer.Add(modifiedBuff);
				}
			}
			
			long buffModificationFlags = 0;
			buffEntity.Add<BuffModificationFlagData>();
			var buffModificationFlagData = new BuffModificationFlagData()
			{
				ModificationTypes = buffModificationFlags,
				ModificationId = ModificationId.NewId(0),
			};
			buffEntity.Write(buffModificationFlagData);
		}
		
		void ClearDeadlyModeBuff(Entity charEntity)
		{
			Buffs.RemoveBuff(charEntity, Prefabs.DeadlyModeBuff);
		}
		
		#region Deadly Buff Definitions
		private const float ResistStatMultiplier = -4.0f; // Take x5 more damage

		static ModifyUnitStatBuff_DOTS DMResistVsUndeads = new()
		{
			StatType = UnitStatType.ResistVsUndeads,
			Value = ResistStatMultiplier,
			ModificationType = ModificationType.Set,
			Modifier = 1,
			Id = ModificationId.NewId(0)
		};
		
		static ModifyUnitStatBuff_DOTS DMResistVsHumans = new()
		{
			StatType = UnitStatType.ResistVsHumans,
			Value = ResistStatMultiplier,
			ModificationType = ModificationType.Set,
			Modifier = 1,
			Id = ModificationId.NewId(0)
		};
		
		static ModifyUnitStatBuff_DOTS DMResistVsDemons = new()
		{
			StatType = UnitStatType.ResistVsDemons,
			Value = ResistStatMultiplier,
			ModificationType = ModificationType.Set,
			Modifier = 1,
			Id = ModificationId.NewId(0)
		};
		
		static ModifyUnitStatBuff_DOTS DMResistVsMechanical = new()
		{
			StatType = UnitStatType.ResistVsMechanical,
			Value = ResistStatMultiplier,
			ModificationType = ModificationType.Set,
			Modifier = 1,
			Id = ModificationId.NewId(0)
		};
		
		static ModifyUnitStatBuff_DOTS DMResistVsBeasts = new()
		{
			StatType = UnitStatType.ResistVsBeasts,
			Value = ResistStatMultiplier,
			ModificationType = ModificationType.Set,
			Modifier = 1,
			Id = ModificationId.NewId(0)
		};
		
		static ModifyUnitStatBuff_DOTS DMResistVsVampires = new()
		{
			StatType = UnitStatType.ResistVsVampires,
			Value = ResistStatMultiplier,
			ModificationType = ModificationType.Set,
			Modifier = 1,
			Id = ModificationId.NewId(0)
		};
		
		private const float DamageStatMultiplier = 2.0f; // Deal 100% more damage
		
		static ModifyUnitStatBuff_DOTS DMDamageVsUndeads = new()
		{
			StatType = UnitStatType.DamageVsUndeads,
			Value = DamageStatMultiplier,
			ModificationType = ModificationType.Set,
			Modifier = 1,
			Id = ModificationId.NewId(0)
		};
		
		static ModifyUnitStatBuff_DOTS DMDamageVsHumans = new()
		{
			StatType = UnitStatType.DamageVsHumans,
			Value = DamageStatMultiplier,
			ModificationType = ModificationType.Set,
			Modifier = 1,
			Id = ModificationId.NewId(0)
		};
		
		static ModifyUnitStatBuff_DOTS DMDamageVsDemons = new()
		{
			StatType = UnitStatType.DamageVsDemons,
			Value = DamageStatMultiplier,
			ModificationType = ModificationType.Set,
			Modifier = 1,
			Id = ModificationId.NewId(0)
		};
		
		static ModifyUnitStatBuff_DOTS DMDamageVsMechanical = new()
		{
			StatType = UnitStatType.DamageVsMechanical,
			Value = DamageStatMultiplier,
			ModificationType = ModificationType.Set,
			Modifier = 1,
			Id = ModificationId.NewId(0)
		};
		
		static ModifyUnitStatBuff_DOTS DMDamageVsBeasts = new()
		{
			StatType = UnitStatType.DamageVsBeasts,
			Value = DamageStatMultiplier,
			ModificationType = ModificationType.Set,
			Modifier = 1,
			Id = ModificationId.NewId(0)
		};
		
		static ModifyUnitStatBuff_DOTS DMDamageVsVampires = new()
		{
			StatType = UnitStatType.DamageVsVampires,
			Value = DamageStatMultiplier,
			ModificationType = ModificationType.Set,
			Modifier = 1,
			Id = ModificationId.NewId(0)
		};

		public static readonly List<ModifyUnitStatBuff_DOTS> deadlyModeBuffs =
		[
			DMDamageVsUndeads,
			DMDamageVsHumans,
			DMDamageVsDemons,
			DMDamageVsMechanical,
			DMDamageVsBeasts,
			DMDamageVsVampires,
			DMResistVsUndeads,
			DMResistVsHumans,
			DMResistVsDemons,
			DMResistVsMechanical,
			DMResistVsBeasts,
			DMResistVsVampires
		];
		#endregion
	}
}
