![](logo.png)
# SanguineArchives for V Rising

SanguineArchives is a server mod for V Rising. It keeps a record of the combat times of players defeating V Blood bosses. Only solo fights are recorded.

You can reach out to me on the V Rising Mod Community Discord (dokebi) for any questions or suggestions.

## Commands

- `.archives boss [BossName]` or `.sga b [BossName]`
  - Show records for the specified V Blood boss.
- `.archives player (0-4) (PlayerName)` or `.sga p (0-4) (PlayerName)`
  - Show player records for V Blood bosses in the given Act. Shows recent records for Act 0 or when no Act is specified. If no player is provided, it defaults to yourself.
- `.archives top (0-4)` or `.sga t (0-4)`
  - Show the top records for V Blood bosses in the given Act. Shows recent records for Act 0 or when no Act is specified.

## Future Plans

- [] Allow owners to configure messages.
- [] Allow players to opt-out of messages.
- [] Staff commands to clear records. Currently, you must edit the data file.
- [] Localization

## Known Issues
- Messaging for Beatrice in Brutal mode is confusing, but combat times are recorded. This will be fixed this soon. 
- I don't know how this works with BloodyBosses.
