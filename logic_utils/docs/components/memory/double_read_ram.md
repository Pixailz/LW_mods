# Double Read RAM

## Preview

<p>
  <img src="https://github.com/Pixailz/LW_mods/blob/main/logic_utils/docs/assets/double_read_ram/front.jpg" alt="Double Read RAM Front" width="300"/>
  <img src="https://github.com/Pixailz/LW_mods/blob/main/logic_utils/docs/assets/double_read_ram/back.jpg" alt="Double Read RAM Back" width="300"/>
  <img src="https://github.com/Pixailz/LW_mods/blob/main/logic_utils/docs/assets/double_read_ram/menu.png" alt="Double Read RAM Menu" width="600"/>
</p>

## Description

An enhanced RAM component that can read from two addresses simultaneously.

**Features:**
- Same configuration as standard RAM
- Two independent read addresses
- Dual output buses
- Two read pins (R1 and R2)
- File loading support

**Location:** [`src/client/MultiReadRam/MultiReadRamClient.cs`](src/client/MultiReadRam/MultiReadRamClient.cs)

## **Configuration**

### in game menu

- Address Lines: 1-24 (default: 4)
- Bit Width: 1-64 (default: 4)

### in code

[Config.cs](https://github.com/Pix/logic_utils/blob/main/logic_utils/src/shared/Config.cs#71)

- **BlockColor**: Sets the color of the RAM block. Default is `Color24.AcidGreen`.
- **BlockDepth**: Defines the depth of the RAM block in the game. Default is `2f`.
- **DefaultAddressWidth**: Specifies the default number of address lines. Default is `4`.
- **DefaultDataWidth**: Specifies the default bit width for data. Default is `4`.
- **DataPinLength**: Length of the data pin, derived from `CGlobal.DataPinLength`.
- **DataPinLengthStep**: Step size for the data pin length, derived from `CGlobal.DataPinLengthStep`.
- **ActionPinLength**: Length of the action pin, derived from `CGlobal.ActionPinLength`.

#### Pin Configuration

- **Load**: Pin for loading data, assigned as `0`.
- **Write**: Pin for writing data, assigned as `1`.
- **DataStart**: Pin for the start of data, assigned as `2`.

