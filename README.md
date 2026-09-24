# Item Stand Fix

Experimental BepInEx plugin for Valheim 1.0.15. It preserves the quality and
variant metadata used by an item stand's periodic visual refresh. The same DLL
supports client-side fallback and dedicated-server enforcement. Visual metadata
is removed again when a stand becomes empty so later items cannot inherit stale
quality or style values.

## Build

Place the required game and mod-loader reference assemblies in `lib`, then run:

```powershell
dotnet build .\src\ItemStandFix\ItemStandFix.csproj -c Release
```

The plugin is produced at
`src\ItemStandFix\bin\Release\ItemStandFix.dll`.

## Install

Copy only `ItemStandFix.dll` into Valheim's `BepInEx\plugins` directory. Keep the
reference assemblies out of that directory because Valheim and BepInEx already
provide them at runtime.

For server-wide behavior, install the DLL in the dedicated server's
`BepInEx\plugins` directory. Clients do not need the plugin. A client installation
remains useful for singleplayer or when connecting to an unmodded server.

## Test

1. Back up the test character and world.
2. Start vanilla Valheim with only this plugin enabled.
3. Place a styled or upgraded item on an item stand.
4. Wait at least ten seconds and confirm its visual style and quality remain.
5. Remove the item and confirm its variant and upgrade level remain intact.
6. Place an item with a different quality and style on the same stand and confirm
   it displays its own values.
7. Optionally enable BepInEx debug logging and check `BepInEx\LogOutput.log` for
   `Stored item stand visual metadata`.

For a server-only test, remove the plugin from the client, restart both processes,
and place a styled or upgraded item from the vanilla client. With BepInEx debug
logging enabled, the dedicated server log contains
`Server repaired item stand visual metadata`.
