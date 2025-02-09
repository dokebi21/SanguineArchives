![](logo.png)
# SanguineArchives for V Rising

SanguineArchives is a server mod for V Rising. Keep records of combat times whenever players defeat V Blood bosses.

This is an unstable release version to get feedback. There will be bugs.

You can reach out to me on the V Rising Mod Community Discord (dokebi) for issues, questions, or suggestions.

## Commands

- `.archives boss [BossName]` or `.sga b [BossName]`
  - Show records for the specified V Blood boss.
- `.archives player (0-4) (PlayerName)` or `.sga p (0-4) (PlayerName)`
  - Show player records for V Blood bosses in the given Act. Shows recent records for Act 0 or when no Act is specified. If no player is provided, it defaults to yourself.
- `.archives top (0-4)` or `.sga t (0-4)`
  - Show the top records for V Blood bosses in the given Act. Shows recent records for Act 0 or when no Act is specified.

## Notes

- Records are only kept for solo fights.
- Records are only kept if the player level is equal to or less than the V Blood boss level.
- Records are only kept if the combat begins with the V Blood boss at full HP.

## Future Plans

- [ ] Support tracking more than 1 of the same V Blood bosses at the same time.
- [ ] Allow owners to configure messages.
- [ ] Configure restrictions on gear levels.
- [ ] Allow players to opt-out of messages.
- [ ] Staff commands to clear records. Currently, you must edit the data file.
- [ ] Localization

## Known Issues

- Messaging for Beatrice in Brutal mode is confusing, but combat times are recorded. This will be fixed this soon. 
- I don't know how this works with BloodyBosses.
- Tracking more than 1 of the same V Blood boss is not supported yet.

## Credits

The V Rising Mod Community has been a great help in developing this mod. Thank you for collaborating with me on Discord.

This mod shares a lot of code from existing mods from the community, especially from the following mods.

- [Bloodcraft](https://github.com/mfoltz/Bloodcraft) by [mfoltz (zfolmt)](https://github.com/mfoltz)
- [KindredCommands](https://github.com/Odjit/KindredCommands) by [odjit](https://github.com/Odjit)
- [BloodyNotify](https://github.com/oscarpedrero/BloodyNotify) by [oscarpedrero (Trodi)](https://github.com/oscarpedrero)
