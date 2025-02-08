using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using Unity.Entities;
using VampireCommandFramework;

using Bloody.Core.API.v1;
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
    public static ManualLogSource LogInstance { get; private set; }

    public override void Load()
    {
        if (Application.productName != "VRisingServer")
            return;

        // Plugin startup logic
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");
        LogInstance = Log;
        // Harmony patching
        Harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        Harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

        // Register all commands in the assembly with VCF
        CommandRegistry.RegisterAll();

        EventsHandlerSystem.OnInitialize += GameDataOnInitialize;
        LoadDatabase.LoadAllConfig();
    }

    private void GameDataOnInitialize(World world)
    {
        EventsHandlerSystem.OnDeathVBlood += KillVBloodSystem.OnDetahVblood;
    }

    public override bool Unload()
    {
        CommandRegistry.UnregisterAssembly();
        Harmony?.UnpatchSelf();
        return true;
    }
}
