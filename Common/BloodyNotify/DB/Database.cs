using System.Collections.Generic;

namespace SanguineArchives.Common.BloodyNotify.DB
{
    internal class Database
    {
        private static string VBloodFinalConcatCharacters = "and";

        private static Dictionary<string, string> PrefabToNames { get; set; } = new Dictionary<string, string>();

        private static Dictionary<string, bool> PrefabsIgnore { get; set; } = new Dictionary<string, bool>();

        private static Dictionary<string, bool> VBloodNotifyIgnore { get; set; } = new Dictionary<string, bool>();

        public static string getVBloodFinalConcatCharacters()
        {
            return VBloodFinalConcatCharacters;
        }

        public static bool setPrefabsNames(Dictionary<string, string> value)
        {
            if (value == null)
                return false;

            PrefabToNames = value;
            return true;
        }
        public static bool setPrefabsIgnore(Dictionary<string, bool> value)
        {
            if (value == null)
                return false;

            PrefabsIgnore = value;

            return true;
        }
        public static bool setVBloodNotifyIgnore(Dictionary<string, bool> value)
        {
            if (value == null)
                return false;

            VBloodNotifyIgnore = value;
            return true;
        }
        
        public static string getPrefabNameValue(string prefabName)
        {
            if (prefabName == null)
            {
                return "NoPrefabName";
            }

            if (PrefabToNames.ContainsKey(prefabName))
            {
                return PrefabToNames[prefabName];
            }
            else
            {
                return PrefabToNames["NoPrefabName"];
            }
        }

        public static bool getPrefabIgnoreValue(string prefabName)
        {
            if (prefabName == null)
            {
                return true;
            }

            if (PrefabsIgnore.ContainsKey(prefabName))
            {
                return PrefabsIgnore[prefabName];
            }
            else
            {
                return true;
            }
        }

        public static bool getVBloodNotifyIgnore(string characterName)
        {
            if (characterName == null)
            {
                return false;
            }

            if (VBloodNotifyIgnore.ContainsKey(characterName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool addVBloodNotifyIgnore(string characterName)
        {

            if (VBloodNotifyIgnore.ContainsKey(characterName))
            {
                return true;
            }
            else
            {
                VBloodNotifyIgnore.Add(characterName, true);
                return true;
            }
        }

        public static bool removeVBloodNotifyIgnore(string characterName)
        {

            if (VBloodNotifyIgnore.ContainsKey(characterName))
            {
                VBloodNotifyIgnore.Remove(characterName);
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
