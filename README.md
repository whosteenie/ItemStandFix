# Item Stand Fix

Experimental BepInEx plugin for Valheim 1.0.15. It preserves the quality and
variant metadata used by an item stand's periodic visual refresh.

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

## Test

1. Back up the test character and world.
2. Start vanilla Valheim with only this plugin enabled.
3. Place a styled or upgraded item on an item stand.
4. Wait at least ten seconds and confirm its visual style and quality remain.
5. Remove the item and confirm its variant and upgrade level remain intact.
6. Check `BepInEx\LogOutput.log` for `Stored item stand visual metadata`.
