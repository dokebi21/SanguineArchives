using System.Collections.Generic;
using SanguineArchives.Common.KindredCommands.Data;
using Stunlock.Core;

namespace SanguineArchives.VBloodArchives.Data;

public class VBloodCollectionData
{
    public static readonly List<string> VBloods_Act1 = new List<string>
    {
        "CHAR_Forest_Wolf_VBlood",
        "CHAR_Bandit_Frostarrow_VBlood",
        "CHAR_Bandit_StoneBreaker_VBlood",
        "CHAR_Bandit_Foreman_VBlood",
        "CHAR_Bandit_Stalker_VBlood",
        "CHAR_Undead_BishopOfDeath_VBlood",
        "CHAR_Bandit_Chaosarrow_VBlood",
        "CHAR_Bandit_Bomber_VBlood",
        "CHAR_Vermin_DireRat_VBlood",
        "CHAR_Bandit_Fisherman_VBlood",
        "CHAR_Poloma_VBlood",
        "CHAR_Forest_Bear_Dire_Vblood",
        "CHAR_Undead_Priest_VBlood",
        "CHAR_Bandit_Tourok_VBlood"
    };
    
    public static readonly List<string> VBloods_Act2 = new List<string>
    {
        "CHAR_Villager_Tailor_VBlood",
        "CHAR_Militia_Guard_VBlood",
        "CHAR_Militia_Nun_VBlood",
        "CHAR_VHunter_Leader_VBlood",
        "CHAR_Undead_Leader_Vblood",
        "CHAR_Undead_BishopOfShadows_VBlood",
        "CHAR_Militia_Scribe_VBlood",
        "CHAR_Undead_Infiltrator_VBlood",
        "CHAR_Militia_Glassblower_VBlood",
        "CHAR_Militia_Longbowman_LightArrow_Vblood",
        "CHAR_Geomancer_Human_VBlood",
        "CHAR_Wendigo_VBlood",
        "CHAR_Vampire_IceRanger_VBlood",
        "CHAR_Vampire_HighLord_VBlood",
        "CHAR_VHunter_Jade_VBlood",
        "CHAR_Militia_BishopOfDunley_VBlood",
        "CHAR_Militia_Leader_VBlood"
    };
    
    public static readonly List<string> VBloods_Act3 = new List<string>
    {
        "CHAR_Gloomrot_Iva_VBlood",
        "CHAR_Gloomrot_Voltage_VBlood",
        "CHAR_Gloomrot_Purifier_VBlood",
        "CHAR_Spider_Queen_VBlood",
        "CHAR_Villager_CursedWanderer_VBlood",
        "CHAR_Undead_ZealousCultist_VBlood",
        "CHAR_Cursed_ToadKing_VBlood",
        "CHAR_WerewolfChieftain_Human",
        "CHAR_Undead_CursedSmith_VBlood"
    };
    
    public static readonly List<string> VBloods_Act4 = new List<string>
    {
        "CHAR_ChurchOfLight_Overseer_VBlood",
        "CHAR_ChurchOfLight_Sommelier_VBlood",
        "CHAR_Harpy_Matriarch_VBlood",
        "CHAR_ArchMage_VBlood", 
        "CHAR_Gloomrot_TheProfessor_VBlood",
        "CHAR_Cursed_Witch_VBlood",
        "CHAR_Winter_Yeti_VBlood",
        "CHAR_ChurchOfLight_Cardinal_VBlood",
        "CHAR_Gloomrot_RailgunSergeant_VBlood",
        "CHAR_VHunter_CastleMan",
        "CHAR_BatVampire_VBlood",
        "CHAR_Cursed_MountainBeast_VBlood",
        "CHAR_Vampire_BloodKnight_VBlood",
        "CHAR_ChurchOfLight_Paladin_VBlood",
        "CHAR_Manticore_VBlood",
        "CHAR_Gloomrot_Monster_VBlood",
        "CHAR_Vampire_Dracula_VBlood"
    };
    
    public static readonly HashSet<PrefabGUID> Buff_InCombat_VBlood_Set = new HashSet<PrefabGUID>
    {
        /* CHAR_Forest_Wolf_VBlood (Alpha the White Wolf) */
        Prefabs.Buff_InCombat_Forest_Wolf,
        /* CHAR_Bandit_Frostarrow_VBlood (Keely the Frost Archer) */
        Prefabs.Buff_InCombat_VBlood_Frostarrow,
        /* CHAR_Bandit_StoneBreaker_VBlood (Errol the Stonebreaker) */
        Prefabs.Buff_InCombat_VBlood_StoneBreaker,
        /* CHAR_Bandit_Foreman_VBlood (Rufus the Foreman) */
        Prefabs.Buff_InCombat_VBlood_Foreman,
        /* CHAR_Bandit_Stalker_VBlood (Grayson the Armourer) */
        Prefabs.Buff_InCombat_VBlood_Armorer,
        Prefabs.Buff_InCombat_VBlood_Armorer_ALREADY_EXISTS_2,
        /* CHAR_Undead_BishopOfDeath_VBlood (Goreswine the Ravager) */
        Prefabs.Buff_InCombat_VBlood_BishopOfDeath,
        /* CHAR_Bandit_Chaosarrow_VBlood (Lidia the Chaos Archer) */
        Prefabs.Buff_InCombat_VBlood_Chaosarrow,
        /* CHAR_Bandit_Bomber_VBlood (Clive the Firestarter) */
        Prefabs.Buff_InCombat_VBlood_Bandit_Bomber,
        /* CHAR_Vermin_DireRat_VBlood (Putrid Rat) */
        Prefabs.Buff_InCombat_VBlood_DireRat,
        /* CHAR_Bandit_Fisherman_VBlood (Finn the Fisherman) */
        Prefabs.Buff_InCombat_Fisherman,
        /* CHAR_Poloma_VBlood (Polora the Feywalker) */
        Prefabs.Buff_InCombat_VBlood_Poloma,
        /* CHAR_Forest_Bear_Dire_Vblood (Kodia the Ferocious Bear) */
        Prefabs.Buff_InCombat_VBlood_Bear_Dire,
        /* CHAR_Undead_Priest_VBlood (Nicholaus the Fallen) */
        Prefabs.Buff_InCombat_VBlood_Undead_Priest,
        /* CHAR_Bandit_Tourok_VBlood (Quincey the Bandit King) */
        Prefabs.Buff_InCombat_VBlood_Tourok,
        /* CHAR_Villager_Tailor_VBlood (Beatrice the Tailor) Age tricky*/
        Prefabs.Buff_InCombat_VBlood_Tailor,
        Prefabs.Buff_Tailor_VBlood_Flee,
        /* CHAR_Militia_Guard_VBlood (Vincent the Frostbringer) */
        Prefabs.Buff_InCombat_VBlood_Militia_Guard,
        /* CHAR_Militia_Nun_VBlood (Christina the Sun Priestess) */
        Prefabs.Buff_InCombat_VBlood_Nun,
        /* CHAR_VHunter_Leader_VBlood (Tristan the Vampire Hunter) */
        Prefabs.Buff_VHunter_Leader_InCombat,
        /* CHAR_Undead_Leader_Vblood (Kriig the Undead General) */
        Prefabs.Buff_Undead_Leader_VBlood_InCombat,
        /* CHAR_Undead_BishopOfShadows_VBlood (Leandra the Shadow Priestess) */
        Prefabs.Buff_InCombat_VBlood_BishopOfShadows,
        /* CHAR_Militia_Scribe_VBlood (Maja the Dark Savant) */
        Prefabs.Buff_InCombat_VBlood_Militia_Scribe,
        /* CHAR_Undead_Infiltrator_VBlood (Bane the Shadowblade) */
        Prefabs.Buff_Undead_Infiltrator_VBlood_InCombat,
        /* CHAR_Militia_Glassblower_VBlood (Grethel the Glassblower) */
        Prefabs.Buff_InCombat_VBlood_Militia_Glassblower,
        /* CHAR_Militia_Longbowman_LightArrow_Vblood (Meredith the Bright Archer) */
        Prefabs.Buff_InCombat_VBlood_Militia_LightArrow,
        /* CHAR_Geomancer_Human_VBlood (Terah the Geomancer) */
        Prefabs.Buff_InCombat_VBlood_Geomancer_Human,
        /* CHAR_Wendigo_VBlood (Frostmaw the Mountain Terror) */
        Prefabs.Buff_InCombat_VBlood_Wendigo,
        /* CHAR_Vampire_IceRanger_VBlood (General Elena the Hollow) */
        Prefabs.Buff_InCombat_VBlood_IceRanger,
        /* CHAR_Vampire_HighLord_VBlood (General Cassius the Betrayer) */
        Prefabs.Buff_InCombat_Npc_HighLord,
        /* CHAR_VHunter_Jade_VBlood (Jade the Vampire Hunter) */
        Prefabs.Buff_InCombat_VHunter_Jade,
        /* CHAR_Militia_BishopOfDunley_VBlood (Raziel the Shepherd) */
        Prefabs.Buff_InCombat_VBlood_BishopOfDunley,
        /* CHAR_Militia_Leader_VBlood (Octavian the Militia Captain) */
        Prefabs.Buff_InCombat_VBlood_Militia_Leader,
        /* CHAR_Gloomrot_Iva_VBlood (Ziva the Engineer) */
        Prefabs.Buff_InCombat_VBlood_Iva,
        /* CHAR_Gloomrot_Voltage_VBlood (Domina the Blade Dancer) */
        Prefabs.Buff_InCombat_VBlood_Voltage,
        /* CHAR_Gloomrot_Purifier_VBlood (Angram the Purifier) */
        Prefabs.Buff_InCombat_VBlood_Purifier,
        /* CHAR_Spider_Queen_VBlood (Ungora the Spider Queen) */
        Prefabs.Buff_InCombat_VBlood_Spider_Queen,
        /* CHAR_Villager_CursedWanderer_VBlood (Ben the Old Wanderer) */
        Prefabs.Buff_InCombat_VBlood_CursedWanderer,
        Prefabs.Buff_CursedWanderer_VBlood_Flee,
        /* CHAR_Undead_ZealousCultist_VBlood (Foulrot the Soultaker) */
        Prefabs.Buff_Undead_ZealousCultist_InCombat,
        /* CHAR_Cursed_ToadKing_VBlood (Albert the Duke of Balaton) */
        Prefabs.Buff_InCombat_VBlood_ToadKing,
        /* CHAR_WerewolfChieftain_Human (Willfred the Werewolf Chief) */
        Prefabs.Buff_InCombat_VBlood_WerewolfChieftain_Werewolf,
        /* CHAR_Undead_CursedSmith_VBlood (Cyril the Cursed Smith) */
        Prefabs.Buff_Undead_CursedSmith_InCombat,
        /* CHAR_ChurchOfLight_Overseer_VBlood (Sir Magnus the Overseer) */
        Prefabs.Buff_InCombat_VBlood_Overseer,
        /* CHAR_ChurchOfLight_Sommelier_VBlood (Baron du Bouchon the Sommelier) */
        Prefabs.Buff_InCombat_VBlood_Sommelier,
        /* CHAR_Harpy_Matriarch_VBlood (Morian the Stormwing Matriarch) */
        Prefabs.Buff_InCombat_VBlood_Harpy_Matriarch,
        /* CHAR_ArchMage_VBlood (Mairwyn the Elementalist) */
        Prefabs.Buff_InCombat_VBlood_Archmage, 
        /* CHAR_Gloomrot_TheProfessor_VBlood (Henry Blackbrew the Doctor) */
        Prefabs.Buff_InCombat_TheProfessor,
        /* CHAR_Cursed_Witch_VBlood (Matka the Curse Weaver) */
        Prefabs.Buff_InCombat_VBlood_Witch,
        /* CHAR_Winter_Yeti_VBlood (Terrorclaw the Ogre) */
        Prefabs.Buff_InCombat_VBlood_Yeti,
        /* CHAR_ChurchOfLight_Cardinal_VBlood (Azariel the Sunbringer) */
        Prefabs.Buff_InCombat_VBlood_ChurchOfLight_Cardinal,
        /* CHAR_Gloomrot_RailgunSergeant_VBlood (Voltatia the Power Master) */
        Prefabs.Buff_InCombat_Gloomrot_RailgunSergeant,
        /* CHAR_VHunter_CastleMan (Simon Belmont the Vampire Hunter) */
        Prefabs.Buff_InCombat_Npc_CastleMan,
        /* CHAR_BatVampire_VBlood (Lord Styx the Night Champion) */
        Prefabs.Buff_BatVampire_InCombat,
        /* CHAR_Cursed_MountainBeast_VBlood (Gorecrusher the Behemoth) */
        Prefabs.Buff_InCombat_VBlood_MountainBeast,
        /* CHAR_Vampire_BloodKnight_VBlood (General Valencia the Depraved) */
        Prefabs.Buff_InCombat_BloodKnight,
        /* CHAR_ChurchOfLight_Paladin_VBlood (Solarus the Immaculate) */
        Prefabs.Buff_InCombat_ChurchOfLight_Paladin,
        /* CHAR_Manticore_VBlood (Talzur the Winged Horror) */
        Prefabs.Buff_InCombat_Manticore,
        /* CHAR_Gloomrot_Monster_VBlood (Adam the Firstborn) */
        Prefabs.Buff_InCombat_Npc_Monster,
        /* CHAR_Vampire_Dracula_VBlood (Dracula the Immortal King) */
        Prefabs.Buff_InCombat_Npc_Boss
        /* CHAR_Bandit_GraveDigger_VBlood_UNUSED (Boris the Gravedigger) */
        /* CHAR_Bandit_Leader_VBlood_UNUSED (Quincey the Marauder) */
        /* CHAR_Bandit_Miner_VBlood_UNUSED (Errol the Stonebreaker) */
        /* CHAR_Militia_Hound_VBlood (Brutus the Watcher) */
        /* CHAR_Militia_HoundMaster_VBlood (Boyo) */
    };
}