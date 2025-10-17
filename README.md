# DescriptionOverride

A BepInEx mod utility for MycoPunk that allows using raw description text from upgrade asset files instead of processed TextBlocks.

## Description

This client-side utility mod modifies how upgrade descriptions are displayed in MycoPunk by bypassing the game's TextBlock processing system and using the raw text from the serialized `_description` field in upgrade asset files. This is particularly useful for modders who want complete control over upgrade descriptions without GameObjects interfering with text formatting.

The mod patches the `Upgrade.Description` getter to return the raw serialized description when enabled, supporting both `GearUpgrade` and `PlayerUpgrade` types. It also includes configurable options for Hashetty font toggles and can be completely disabled if needed.

## Getting Started

### Dependencies

* MycoPunk (base game)
* [BepInEx](https://github.com/BepInEx/BepInEx) - Version 5.4.2403 or compatible
* .NET Framework 4.8

### Building/Compiling

1. Clone this repository
2. Open the solution file in Visual Studio, Rider, or your preferred C# IDE
3. Build the project in Release mode

Alternatively, use dotnet CLI:
```bash
dotnet build --configuration Release
```

### Installing

**Option 1: Via Thunderstore (Recommended)**
1. Download and install using the Thunderstore Mod Manager
2. Search for "DescriptionOverride" under MycoPunk community
3. Install and enable the mod

**Option 2: Manual Installation**
1. Ensure BepInEx is installed for MycoPunk
2. Copy `DescriptionOverride.dll` from the build folder
3. Place it in `<MycoPunk Game Directory>/BepInEx/plugins/`
4. Launch the game

### Executing program

Once installed, the mod works automatically. Upgrade descriptions will use raw text from asset files instead of processed TextBlocks.

### Configuration

The mod can be configured through BepInEx Configuration Manager or by editing the config file:

**General Settings:**
- `EnableDescriptionOverride`: (Default: true) If enabled, uses the serialized _description field instead of TextBlocks
- `EnableHashettyOverride`: (Default: true) If enabled, applies Hashetty font toggles in descriptions

### Usage

**For Modders/Customizers:**
1. Modify the `_description` field in your upgrade asset files directly
2. Raw text will be displayed as-is, preserving your formatting
3. Use Hashetty font toggle codes if `EnableHashettyOverride` is enabled

**For Players:**
- The mod has no direct player-facing features
- It's a utility that affects how upgrade descriptions are rendered in the game

## Help

* **Descriptions look different?** This mod intentionally changes how descriptions are processed - raw text from assets is used instead of TextBlocks
* **Configuration not working?** Changes require a game restart to take effect
* **Conflicts with other mods?** This mod patches Upgrade.Description getter. Other mods modifying text processing may interfere
* **For modders: TextBlocks not working?** The mod bypasses TextBlocks entirely when enabled - use raw text in _description field
* **Performance impact?** Minimal - only patches one getter method and doesn't run during gameplay
* **Hashetty fonts not working?** Ensure `EnableHashettyOverride` is enabled and your text contains proper Hashetty toggle codes

## Authors

* Sparroh
* funlennysub (original mod template)
* [@DomPizzie](https://twitter.com/dompizzie) (README template)

## License

* This project is licensed under the MIT License - see the LICENSE.md file for details
