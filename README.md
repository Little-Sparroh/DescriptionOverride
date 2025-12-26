# DescriptionOverride

A BepInEx mod for MycoPunk that overrides upgrade descriptions to use the serialized `_description` field from asset files.

## Description

This mod patches the `Upgrade.Description` getter to return the raw `_description` field from `GearUpgrade` and `PlayerUpgrade` assets instead of the processed TextBlocks. This allows modders to use custom descriptions in their upgrade assets, including support for Hashetty font toggles.

## Installation

1. Download the mod from Thunderstore or build from source.
2. Place `DescriptionOverride.dll` in `<MycoPunk Directory>/BepInEx/plugins/`
3. The mod loads automatically through BepInEx when the game starts.

## Configuration

The mod includes a configuration file located at `<MycoPunk Directory>/BepInEx/config/sparroh.descriptionoverride.cfg`:

- **General > EnableDescriptionOverride**: Enables or disables the description override functionality. Default: true

## Features

- Overrides descriptions for GearUpgrade and PlayerUpgrade types
- Supports Hashetty font toggles in descriptions
- Configurable toggle to enable/disable the override

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for version history.

## Authors

- Sparroh
- funlennysub (BepInEx template)
- [@DomPizzie](https://twitter.com/dompizzie) (README template)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
