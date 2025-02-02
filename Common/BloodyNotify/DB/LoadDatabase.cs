using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SanguineArchives.Common.BloodyNotify.DB
{
    internal class LoadDatabase
    {
        public static void LoadAllConfig()
        {
            LoadPrefabsName();
            LoadPrefabsIgnore();
            VBloodNotifyIgnoreConfig();
        }

        public static void LoadPrefabsName()
        {
            var json = File.ReadAllText(Path.Combine(Config.ConfigPath, "prefabs_names.json"));
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Database.setPrefabsNames(dictionary);
        }

        public static void LoadPrefabsIgnore()
        {
            var json = File.ReadAllText(Path.Combine(Config.ConfigPath, "prefabs_names_ignore.json"));
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, bool>>(json);
            Database.setPrefabsIgnore(dictionary);
        }

        public static void VBloodNotifyIgnoreConfig()
        {
            var json = File.ReadAllText(Path.Combine(Config.ConfigPath, "vbloodannounce_ignore_users.json"));
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, bool>>(json);
            Database.setVBloodNotifyIgnore(dictionary);
        }
    }
}
