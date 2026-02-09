# Hexadecimal Display

## Preview

<p>
  <img src="https://github.com/Pix/logic_utils/blob/main/logic_utils/docs/assets/hex_display/front.jpg" alt="Hexadecimal Display Front" width="300"/>
  <img src="https://github.com/Pix/logic_utils/blob/main/logic_utils/docs/assets/hex_display/back.jpg" alt="Hexadecimal Display Back" width="300"/>
  <img src="https://github.com/Pix/logic_utils/blob/main/logic_utils/docs/assets/hex_display/menu.png" alt="Hexadecimal Display Menu" width="600"/>
</p>

## Description

A 5×9 pixel matrix display for showing hexadecimal digits (0-9, A-F).

**Features:**
- 4-bit input for hex values
- Built-in character patterns (0-F)
- Customizable 1bpp color
- Resizable width
- On/Off control pin

**Location:** [`src/client/HexDisplay/HexDisplayClient.cs`](https://github.com/Pix/logic_utils/blob/main/logic_utils/src/client/HexDisplay/HexDisplayClient.cs)

## **Configuration**

### in game menu

- Size: 1-32 (default: 5)
- Color: Customizable

### in code

[Config.cs](https://github.com/Pix/logic_utils/blob/main/logic_utils/src/shared/Config.cs#L200)

- **BlockColor**: Sets the base color of the display block. Default is `Color24.Black`.
- **BlockDepth**: Depth of the display block. Default is `0.5f`.
- **DefaultInput**: Default number of input pins. Default is `5`.
- **FixedBPP**: Fixed bits per pixel value. Default is `1`.
- **DataPinLength**: Length of the data pin, derived from `CGlobal.DataPinLength`.
- **DataPinLengthStep**: Step size for the data pin length, derived from `CGlobal.DataPinLengthStep`.
- **ActionPinLength**: Length of the action pin, derived from `CGlobal.ActionPinLength`.
- **OriginalScale**: Original scale of the display. Default is `0.2f`.
- **OriginalWidth**: Width of the original pixel layout. Default is `5f`.
- **OriginalHeight**: Height of the original pixel layout. Default is `9f`.
- **MaxSize**: Maximum display size. Default is `32`.
- **MinSize**: Minimum display size. Default is `1`.
- **DefaultSize**: Default size, calculated from the original scale. Default is `5`.

#### Pin Configuration

- **OnOff**: Pin for display on/off control, assigned as `0`.
- **DataStart**: Pin for the start of data, assigned as `1`.
