using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using Unity.Entities;
using VampireCommandFramework;
using System.IO;

using Bloody.Core;
using Bloody.Core.API.v1;
using SanguineArchives.Common.BloodyNotify.AutoAnnouncer;
using SanguineArchives.Common.BloodyNotify.DB;
using SanguineArchives.Common.BloodyNotify.Systems;

namespace SanguineArchives;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("gg.deca.VampireCommandFramework")]
[BepInDependency("gg.deca.Bloodstone")]
[BepInDependency("trodi.Bloody.Core")]
[Bloodstone.API.Reloadable]
public class Plugin : BasePlugin
{
    internal static Harmony Harmony;
    internal static ManualLogSource PluginLog;
    public static ManualLogSource LogInstance { get; private set; }
    
    #region BloodyNotify
    public static Bloody.Core.Helper.v1.Logger Logger;
    public static SystemsCore SystemsCore;

    public static ConfigEntry<bool> AnnounceOnline;
    public static ConfigEntry<bool> AnnounceeOffline;
    public static ConfigEntry<bool> AnnounceNewUser;
    public static ConfigEntry<bool> AnnounceVBlood;
    public static ConfigEntry<string> VBloodFinalConcatCharacters;
    public static ConfigEntry<bool> AutoAnnouncerConfig;
    public static ConfigEntry<int> IntervalAutoAnnouncer;
    public static ConfigEntry<bool> MessageOfTheDay;

    public static readonly string ConfigPath = Path.Combine(Paths.ConfigPath, "BloodyNotify");
    #endregion

    public override void Load()
    {
        if (Application.productName != "VRisingServer")
            return;
        
        Logger = new(Log);
        PluginLog = Log;
        // Plugin startup logic
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");
        LogInstance = Log;
        // Harmony patching
        Harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        Harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

        // Register all commands in the assembly with VCF
        CommandRegistry.RegisterAll();
        
        EventsHandlerSystem.OnInitialize += GameDataOnInitialize;
        InitConfig();
        LoadDatabase.LoadAllConfig();
    }
    
    private void GameDataOnInitialize(World world)
    {
        SystemsCore = Bloody.Core.Core.SystemsCore;

        Database.EnabledFeatures[NotifyFeature.online] = AnnounceOnline.Value;
        Database.EnabledFeatures[NotifyFeature.offline] = AnnounceeOffline.Value;
        Database.EnabledFeatures[NotifyFeature.newuser] = AnnounceNewUser.Value;
        Database.EnabledFeatures[NotifyFeature.vblood] = AnnounceVBlood.Value;
        Database.EnabledFeatures[NotifyFeature.auto] = AutoAnnouncerConfig.Value;
        Database.EnabledFeatures[NotifyFeature.motd] = MessageOfTheDay.Value;

        Database.setVBloodFinalConcatCharacters(VBloodFinalConcatCharacters.Value);
        Database.setIntervalAutoAnnouncer(IntervalAutoAnnouncer.Value);

        
        EventsHandlerSystem.OnUserConnected += OnlineOfflineSystem.OnUserOnline;
        EventsHandlerSystem.OnUserDisconnected += OnlineOfflineSystem.OnUserOffline;
        EventsHandlerSystem.OnDeathVBlood += KillVBloodSystem.OnDetahVblood;

        
        AutoAnnouncerFunction.StartAutoAnnouncer();
    }
    
    private void InitConfig()
    {
        AnnounceOnline = Config.Bind("UserOnline", "enabled", true, "Enable Announce when user online");
        AnnounceeOffline = Config.Bind("UserOffline", "enabled", true, "Enable Announce when user offline");
        AnnounceNewUser = Config.Bind("NewUserOnline", "enabled", true, "Enable Announce when new user create in server");
        AnnounceVBlood = Config.Bind("AnnounceVBlood", "enabled", true, "Enable Announce when user/users kill a VBlood Boss.");
        VBloodFinalConcatCharacters = Config.Bind("AnnounceVBlood", "VBloodFinalConcatCharacters", "and", "Final string for concat two or more players kill a VBlood Boss.");
        AutoAnnouncerConfig = Config.Bind("AutoAnnouncer", "enabled", false, "Enable AutoAnnouncer.");
        IntervalAutoAnnouncer = Config.Bind("AutoAnnouncer", "interval", 300, "Interval seconds for spam AutoAnnouncer.");
        MessageOfTheDay = Config.Bind("MessageOfTheDay", "enabled", false, "Enable Message Of The Day.");

        if (!Directory.Exists(ConfigPath)) Directory.CreateDirectory(ConfigPath);

        SanguineArchives.Common.BloodyNotify.DB.Config.CheckAndCreateConfigs();
    }
    
    public void OnGameInitialized()
    {
        
    }

    public override bool Unload()
    {
        CommandRegistry.UnregisterAssembly();
        Harmony?.UnpatchSelf();
        return true;
    }
}
