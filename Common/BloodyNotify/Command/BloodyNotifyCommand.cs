using Bloody.Core;
using Bloody.Core.API.v1;
using Bloody.Core.GameData.v1;
using System.Linq;
using VampireCommandFramework;
using SanguineArchives.Common.BloodyNotify.DB;
using SanguineArchives.Common.Utils;

namespace SanguineArchives.Common.BloodyNotify.Command
{
    [CommandGroup("bn")]
    internal class BloodyNotifyCommand
    {
        [Command("online", "o", description: "To reload the configuration of the user messages online, offline or death of the VBlood boss", adminOnly: false)]
        public static void Online(ChatCommandContext ctx)
        {
            ctx.Reply("Users Online ---------");
            foreach (var user in GameData.Users.Online.OrderBy(x => x.IsAdmin))
            {
                if(user.IsAdmin)
                {
                    ctx.Reply($"{ChatColor.Green("[ADMIN]")} {ChatColor.Yellow(user.CharacterName)}");
                } else
                {
                    ctx.Reply($"{ChatColor.Yellow(user.CharacterName)}");
                }
            }
        }


        [Command("reload", "rl", description: "To reload the configuration of the user messages online, offline or death of the VBlood boss", adminOnly: true)]
        public static void RealoadMod(ChatCommandContext ctx)
        {
            LoadDatabase.LoadPrefabsName();
            LoadDatabase.LoadPrefabsIgnore();
            ctx.Reply("Reloaded configuration of BloodyNotify mod.");

        }

        [Command("vblood", "vba", usage: "ignore/unignore", description: "ignore/unignore vblood announce system.", adminOnly: false)]
        public static void Vbloodannounce(ChatCommandContext ctx, string action = "unignore")
        {

            var user = ctx.User;

            switch (action)
            {
                case "ignore":
                    Database.addVBloodNotifyIgnore(user.CharacterName.ToString());
                    ctx.Reply(ChatColor.Green($"You will not receive any more notifications about the death of the VBlood. To undo this option use the command {ChatColor.Yellow(".notify vbloodannounce unignore")}"));
                    break;
                case "unignore":
                    Database.removeVBloodNotifyIgnore(user.CharacterName.ToString());
                    ctx.Reply(ChatColor.Green($"You will receive notifications about the death of the VBlood. To undo this option use the command {ChatColor.Yellow(".notify vbloodannounce ignore")}"));
                    break;

            }
        }
    }
}
