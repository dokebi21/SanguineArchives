namespace SanguineArchives;

public class ChatColor : Bloody.Core.API.v1.FontColorChatSystem
{
    public new static string Blue(string text) => ChatColor.Color("#256DFE", text);
    public new static string Yellow(string text) => ChatColor.Color("#FFE100", text);
    public static string Gray(string text) => ChatColor.Color("#A9A9A9", text);
    public static string Purple(string text) => ChatColor.Color("#D900FF", text);
}