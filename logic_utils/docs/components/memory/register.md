
# Register

## Preview

<p>
	<img src="https://github.com/Pixailz/LW_mods/blob/main/logic_utils/docs/assets/register/front.jpg" alt="Register Front" width="300"/>
	<img src="https://github.com/Pixailz/LW_mods/blob/main/logic_utils/docs/assets/register/back.jpg" alt="Register Back" width="300"/>
	<img src="https://github.com/Pixailz/LW_mods/blob/main/logic_utils/docs/assets/register/menu.png" alt="Register Menu" width="600"/>
</p>

## Description

A configurable register component with various control pins and flexible bit width.

**Features:**
- Clock-based operation
- Write enable pin
- Read enable pin
- Low/High preset pins
- Increment/Decrement pins
- Configurable width (2-64 bits in 2-bit steps)

**Location:** [`src/client/RegisterPrefab.cs`](https://github.com/Pixailz/LW_mods/blob/main/logic_utils/src/client/RegisterPrefab.cs)

## **Configuration**

### in game menu

- Register Width: 2-64 bits (default: 8, step: 2)

### in code

[Config.cs](https://github.com/Pixailz/LW_mods/blob/main/logic_utils/src/shared/Config.cs#L36)

- **BlockColor**: Sets the color of the register block. Default is `Color24.Acajou`.
- **BlockHeight**: Height of the register block. Default is `2f`.
- **BlockDepth**: Depth of the register block. Default is `2f`.
- **DefaultDataWidth**: Default bit width for the register. Default is `8`.
- **DataPinLength**: Length of the data pin, derived from `CGlobal.DataPinLength`.
- **DataPinLengthStep**: Step size for the data pin length, derived from `CGlobal.DataPinLengthStep`.
- **ActionPinLength**: Length of the action pin, derived from `CGlobal.ActionPinLength`.
- **MaxInput**: Maximum register width. Default is `64`.
- **MinInput**: Minimum register width. Default is `2`.
- **StepInput**: Step size for register width. Default is `2`.

#### Pin Configuration

- **Clock**: Pin for clock signal, assigned as `0`.
- **Write**: Pin for write enable, assigned as `1`.
- **Read**: Pin for read enable, assigned as `2`.
- **Low**: Pin for preset low, assigned as `3`.
- **High**: Pin for preset high, assigned as `4`.
- **Plus**: Pin for increment, assigned as `5`.
- **Minus**: Pin for decrement, assigned as `6`.
- **DataStart**: Pin for the start of data, assigned as `7`.
