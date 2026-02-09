# Index

- [Index](#index)
- [Introduction](#introduction)
- [Components](#components)
	- [Memory Components](#memory-components)
	- [Display Components](#display-components)
	- [Multiplexer Components](#multiplexer-components)
- [Class Documentation](#class-documentation)
- [Dependencies](#dependencies)
- [Credits](#credits)

# Introduction

This is a collection of utility components for Logic World, providing enhanced memory management, display capabilities, and data manipulation tools.

> [!NOTE]
> Pictures of components may change as the mod is developed, but the core functionality will remain consistent.

# Components

## Memory Components

- [RAM](docs/components/memory/ram.md) - Random Access Memory with configurable address and data widths
- [Double Read RAM](docs/components/memory/double_read_ram.md) - Enhanced RAM with dual simultaneous read addresses
- [Register](docs/components/memory/register.md) - Configurable register with clock, write, read, preset, and increment/decrement pins

## Display Components

- [Seven Segment Display](docs/components/display/seven_segment_display.md) - Classic seven-segment display for digits and characters
- [Hexadecimal Display](docs/components/display/hex_display.md) - 5×9 pixel matrix for hexadecimal digits (0-F)
- [Screen Display (WIP)](docs/components/display/screen.md) - Programmable pixel display with configurable resolution and color depth

## Multiplexer Components

- [Decoder](docs/components/multiplexer/decoder.md) - Binary-to-decimal decoder with configurable input count

# Class Documentation

- [DisplayConfigurationMenuBase](docs/class/display_configurations_menu.md) - Abstract base class for display configuration menus

# Dependencies

This mod requires the following dependencies:
- [EccsGuiBuilder](https://github.com/Ecconia/Ecconia-LogicWorld-Mods/tree/master/EccsGuiBuilder) - For GUI functionality
- [EccsLogicWorldAPI](https://github.com/Ecconia/Ecconia-LogicWorld-Mods) - API extensions

# Credits

This mod is inspired by and includes concepts from:
- **Cheese3660's CheeseUtilMod**
  - [MultiReadRam concept](https://github.com/cheese3660/CheeseUtilMod/blob/master/cheeseutil/src/client/RamResizableClient.cs)
  - [Texture2D implementation](https://github.com/cheese3660/CheeseUtilMod/blob/02903eb4e047f653fb05b0518042cce8f25baf13/cheeseutil/src/client/TextDisplay.cs#L19)

Special thanks to:
- **Ecconia** for the EccsGuiBuilder and EccsLogicWorldAPI frameworks
- **Cheese3660** for inspiration and technical approaches