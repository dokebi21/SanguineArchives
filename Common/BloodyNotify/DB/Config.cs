﻿using System.Collections.Generic;
using System.IO;
using BepInEx;

namespace SanguineArchives.Common.BloodyNotify.DB
{
    internal class Config
    {
        public static readonly string ConfigPath = Path.Combine(Paths.ConfigPath, "BloodyNotify");

        public static Dictionary<string, string> DefaultAnnounceDictionary => new Dictionary<string, string>()
        {
            { "online", "#user# is online!" },
            { "offline", "#user# has gone offline!" },
            { "newUser", "Welcome to server" },
            { "VBlood", "Congratulations to #user# for beating #vblood# in #combatDuration# seconds!" },
            { "VBloodRecords_BasePrefix", "Congratulations!" },
            { "VBloodRecords_NewRecordPrefix", "New Top Record!" },
            { "VBloodRecords_BaseSuffix", "#user# beat #vblood# in #combatDuration# seconds!" },
            { "VBloodRecords_PersonalRecordPrefix", "New Personal Record!" },
            { "VBloodRecords_PersonalRecordSuffix", "Your previous record was #combatDuration# seconds." },
            { "VBloodRecords_PersonalRecordFirstSuffix", "This is your first record." },
            { "VBloodRecords_AboveLevel", "The fight was not recorded. Your level was above the boss level." },
            { "VBloodRecords_CoopNotRecorded", "The fight was not recorded. Co-op fights are currently not recorded." }
        };
        public static Dictionary<string, string> PrefabToNamesDefault => new Dictionary<string, string>()
        {
            {"CHAR_Bandit_Frostarrow_VBlood", "Keely the Frost Archer"},
            {"CHAR_Bandit_Foreman_VBlood", "Rufus the Foreman"},
            {"CHAR_Bandit_StoneBreaker_VBlood", "Errol the Stonebreaker"},
            {"CHAR_Bandit_Chaosarrow_VBlood", "Lidia the Chaos Archer"},
            {"CHAR_Undead_BishopOfDeath_VBlood", "Goreswine the Ravager"},
            {"CHAR_Bandit_Stalker_VBlood", "Grayson the Armourer"},
            {"CHAR_Vermin_DireRat_VBlood", "Putrid Rat"},
            {"CHAR_Bandit_Bomber_VBlood", "Clive the Firestarter"},
            {"CHAR_Undead_Priest_VBlood", "Nicholaus the Fallen"},
            {"CHAR_Bandit_Tourok_VBlood", "Quincey the Bandit King"},
            {"CHAR_Villager_Tailor_VBlood", "Beatrice the Tailor"},
            {"CHAR_Militia_Guard_VBlood", "Vincent the Frostbringer"},
            {"CHAR_VHunter_Leader_VBlood", "Tristan the Vampire Hunter"},
            {"CHAR_Undead_BishopOfShadows_VBlood", "Leandra the Shadow Priestess"},
            {"CHAR_Geomancer_Human_VBlood", "Terah the Geomancer"},
            {"CHAR_Militia_Longbowman_LightArrow_Vblood", "Meredith the Bright Archer"},
            {"CHAR_Wendigo_VBlood", "Frostmaw the Mountain Terror"},
            {"CHAR_Militia_Leader_VBlood", "Octavian the Militia Captain"},
            {"CHAR_Militia_BishopOfDunley_VBlood", "Raziel the Shepherd"},
            {"CHAR_Spider_Queen_VBlood", "Ungora the Spider Queen"},
            {"CHAR_Cursed_ToadKing_VBlood", "Albert the Duke of Balaton"},
            {"CHAR_VHunter_Jade_VBlood", "Jade the Vampire Hunter"},
            {"CHAR_Undead_ZealousCultist_VBlood", "Foulrot the Soultaker"},
            {"CHAR_WerewolfChieftain_Human", "Willfred the Werewolf Chief"},
            {"CHAR_ArchMage_VBlood", "Mairwyn the Elementalist"},
            {"CHAR_Winter_Yeti_VBlood", "Terrorclaw the Ogre"},
            {"CHAR_Harpy_Matriarch_VBlood", "Morian the Stormwing Matriarch"},
            {"CHAR_Cursed_Witch_VBlood", "Matka the Curse Weaver"},
            {"CHAR_BatVampire_VBlood", "Lord Styx the Night Champion"},
            {"CHAR_Cursed_MountainBeast_VBlood", "Gorecrusher the Behemoth"},
            {"CHAR_Manticore_VBlood", "Talzur the Winged Horror"},
            {"CHAR_Bandit_GraveDigger_VBlood_UNUSED", "Boris the Gravedigger"},
            {"CHAR_Bandit_Leader_VBlood_UNUSED", "Quincey the Marauder"},
            {"CHAR_Bandit_Miner_VBlood_UNUSED", "Errol the Stonebreaker"},
            {"CHAR_ChurchOfLight_Cardinal_VBlood", "Azariel the Sunbringer"},
            {"CHAR_ChurchOfLight_Overseer_VBlood", "Sir Magnus the Overseer"},
            {"CHAR_ChurchOfLight_Paladin_VBlood", "Solarus the Immaculate"},
            {"CHAR_ChurchOfLight_Sommelier_VBlood", "Baron du Bouchon the Sommelier"},
            {"CHAR_Forest_Bear_Dire_Vblood", "Kodia the Ferocious Bear"},
            {"CHAR_Forest_Wolf_VBlood", "Alpha the White Wolf"},
            {"CHAR_Gloomrot_Iva_VBlood", "Ziva the Engineer"},
            {"CHAR_Gloomrot_Monster_VBlood", "Adam the Firstborn"},
            {"CHAR_Gloomrot_Purifier_VBlood", "Angram the Purifier"},
            {"CHAR_Gloomrot_RailgunSergeant_VBlood", "Voltatia the Power Master"},
            {"CHAR_Gloomrot_TheProfessor_VBlood", "Henry Blackbrew the Doctor"},
            {"CHAR_Gloomrot_Voltage_VBlood", "Domina the Blade Dancer"},
            {"CHAR_Militia_Glassblower_VBlood", "Grethel the Glassblower"},
            {"CHAR_Militia_Hound_VBlood", "Brutus the Watcher"},
            {"CHAR_Militia_HoundMaster_VBlood", "Boyo"},
            {"CHAR_Militia_Nun_VBlood", "Christina the Sun Priestess"},
            {"CHAR_Militia_Scribe_VBlood", "Maja the Dark Savant"},
            {"CHAR_Poloma_VBlood", "Polora the Feywalker"},
            {"CHAR_Undead_CursedSmith_VBlood", "Cyril the Cursed Smith"},
            {"CHAR_Undead_Infiltrator_VBlood", "Bane the Shadowblade"},
            {"CHAR_Undead_Leader_Vblood", "Kriig the Undead General"},
            {"CHAR_Villager_CursedWanderer_VBlood", "Ben the Old Wanderer"},
            {"CHAR_Bandit_Fisherman_VBlood", "Finn the Fisherman"},
            {"CHAR_VHunter_CastleMan", "Simon Belmont the Vampire Hunter"},
            {"CHAR_Vampire_BloodKnight_VBlood", "General Valencia the Depraved"},
            {"CHAR_Vampire_Dracula_VBlood", "Dracula the Immortal King"},
            {"CHAR_Vampire_HighLord_VBlood", "General Cassius the Betrayer"},
            {"CHAR_Vampire_IceRanger_VBlood", "General Elena the Hollow"},
            {"NoPrefabName", "VBlood Boss" }
        };

        public static Dictionary<string, bool> PrefabsIgnoreDefaul => new Dictionary<string, bool>()
        {
            {"CHAR_Bandit_Frostarrow_VBlood", false},
            {"CHAR_Bandit_Foreman_VBlood", false},
            {"CHAR_Bandit_StoneBreaker_VBlood", false},
            {"CHAR_Bandit_Chaosarrow_VBlood", false},
            {"CHAR_Undead_BishopOfDeath_VBlood", false},
            {"CHAR_Bandit_Stalker_VBlood", false},
            {"CHAR_Vermin_DireRat_VBlood", false},
            {"CHAR_Bandit_Bomber_VBlood", false},
            {"CHAR_Undead_Priest_VBlood", false},
            {"CHAR_Bandit_Tourok_VBlood", false},
            {"CHAR_Villager_Tailor_VBlood", false},
            {"CHAR_Militia_Guard_VBlood", false},
            {"CHAR_VHunter_Leader_VBlood", false},
            {"CHAR_Undead_BishopOfShadows_VBlood", false},
            {"CHAR_Geomancer_Human_VBlood", false},
            {"CHAR_Militia_Longbowman_LightArrow_Vblood", false},
            {"CHAR_Wendigo_VBlood", false},
            {"CHAR_Militia_Leader_VBlood", false},
            {"CHAR_Militia_BishopOfDunley_VBlood", false},
            {"CHAR_Spider_Queen_VBlood", false},
            {"CHAR_Cursed_ToadKing_VBlood", false},
            {"CHAR_VHunter_Jade_VBlood", false},
            {"CHAR_Undead_ZealousCultist_VBlood", false},
            {"CHAR_WerewolfChieftain_Human", false},
            {"CHAR_ArchMage_VBlood", false},
            {"CHAR_Winter_Yeti_VBlood", false},
            {"CHAR_Harpy_Matriarch_VBlood", false},
            {"CHAR_Cursed_Witch_VBlood", false},
            {"CHAR_BatVampire_VBlood", false},
            {"CHAR_Cursed_MountainBeast_VBlood", false},
            {"CHAR_Manticore_VBlood", false},
            {"CHAR_Paladin_VBlood", false},
            {"CHAR_Bandit_GraveDigger_VBlood_UNUSED", false},
            {"CHAR_Bandit_Leader_VBlood_UNUSED", false},
            {"CHAR_Bandit_Miner_VBlood_UNUSED", false},
            {"CHAR_ChurchOfLight_Cardinal_VBlood", false},
            {"CHAR_ChurchOfLight_Overseer_VBlood", false},
            {"CHAR_ChurchOfLight_Sommelier_VBlood", false},
            {"CHAR_Forest_Bear_Dire_Vblood", false},
            {"CHAR_Forest_Wolf_VBlood", false},
            {"CHAR_Gloomrot_Iva_VBlood", false},
            {"CHAR_Gloomrot_Monster_VBlood", false},
            {"CHAR_Gloomrot_Purifier_VBlood", false},
            {"CHAR_Gloomrot_RailgunSergeant_VBlood", false},
            {"CHAR_Gloomrot_TheProfessor_VBlood", false},
            {"CHAR_Gloomrot_Voltage_VBlood", false},
            {"CHAR_Militia_Glassblower_VBlood", false},
            {"CHAR_Militia_Hound_VBlood", false},
            {"CHAR_Militia_HoundMaster_VBlood", false},
            {"CHAR_Militia_Nun_VBlood", false},
            {"CHAR_Militia_Scribe_VBlood", false},
            {"CHAR_Poloma_VBlood", false},
            {"CHAR_Undead_CursedSmith_VBlood", false},
            {"CHAR_Undead_Infiltrator_VBlood", false},
            {"CHAR_Undead_Leader_Vblood", false},
            {"CHAR_Villager_CursedWanderer_VBlood", false},
            {"CHAR_Bandit_Fisherman_VBlood", false},
            {"CHAR_VHunter_CastleMan", false},
            {"CHAR_Vampire_BloodKnight_VBlood", false},
            {"CHAR_Vampire_Dracula_VBlood", false},
            {"CHAR_Vampire_HighLord_VBlood", false},
            {"CHAR_Vampire_IceRanger_VBlood", false},
            {"NoPrefabName", false }
        };


        public static Dictionary<string, string> DefaultOnline => new Dictionary<string, string>()
        {
            {  "nick" , "#user# is online!" }
        };


        public static Dictionary<string, string> DefaultOffline => new Dictionary<string, string>()
        {
            {  "nick" , "#user# is ofline!" }
        };


        public static Dictionary<string, bool> DefaultVBloodAnnounceIgnoreUsers => new Dictionary<string, bool>()
        {
            {  "nick" , true }
        };

        public static List<string> DefaultMessageOfTheDay => new List<string>()
        {
            {  "#user# this is Message of the day Line 1" },
            {  "Message of the day Line 2" },
            {  "Message of the day Line 3" }
        };
    }
}
