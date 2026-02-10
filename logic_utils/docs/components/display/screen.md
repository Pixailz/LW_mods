# Screen Display

## Preview

<p>
  <img src="https://github.com/Pixailz/LW_mods/blob/main/logic_utils/docs/assets/screen/front.jpg" alt="Screen Display Front" width="300"/>
  <img src="https://github.com/Pixailz/LW_mods/blob/main/logic_utils/docs/assets/screen/back.jpg" alt="Screen Display Back" width="300"/>
  <img src="https://github.com/Pixailz/LW_mods/blob/main/logic_utils/docs/assets/screen/menu.png" alt="Screen Display Menu" width="600"/>
</p>

## Description

A WIP component

A programmable pixel display with configurable resolution and color depth.

**Features:**
- Configurable resolution (64x64 to 1024x1024)
- Multiple bits per pixel (1-8 bpp)
- Real-time pixel updates
- Customizable color schemes
- Clock-driven pixel data input

**Location:** [`src/client/Screen/ScreenClient.cs`](https://github.com/Pix/logic_utils/blob/main/logic_utils/src/client/Screen/ScreenClient.cs)

## **Configuration**

### in game menu

- Resolution X: 64-1024 (default: 64)
- Resolution Y: 64-1024 (default: 64)
- Bits Per Pixel: 1-8 (default: 1)
- Color Configuration: Customizable

### in code

[Config.cs](https://github.com/Pix/logic_utils/blob/main/logic_utils/src/shared/Config.cs#L437)

- **BlockColor**: Sets the base color of the screen block. Default is `Color24.Black`.
- **BlockDepth**: Depth of the screen block. Default is `0.5f`.
- **DataPinLength**: Length of the data pin, derived from `CGlobal.DataPinLength`.
- **DataPinLengthStep**: Step size for the data pin length, derived from `CGlobal.DataPinLengthStep`.
- **ActionPinLength**: Length of the action pin, derived from `CGlobal.ActionPinLength`.
- **OriginalScale**: Original scale of the display. Default is `0.25f`.
- **DefaultSize**: Default size, calculated from the original scale. Default is `4`.
- **DefaultDataSize**: Default size for data. Default is `64`.
- **MinBPP**: Minimum bits per pixel. Default is `1`.
- **MaxBPP**: Maximum bits per pixel. Default is `8`.
- **DefaultBPP**: Default bits per pixel. Default is `1`.
- **MinResolutionX**: Minimum horizontal resolution. Default is `64`.
- **MaxResolutionX**: Maximum horizontal resolution. Default is `1024`.
- **DefaultResolutionX**: Default horizontal resolution. Default is `64`.
- **MinResolutionY**: Minimum vertical resolution. Default is `64`.
- **MaxResolutionY**: Maximum vertical resolution. Default is `1024`.
- **DefaultResolutionY**: Default vertical resolution. Default is `64`.

#### Pin Configuration

- **EndPulse/Clock**: Pin for end pulse or clock signal, assigned as `0`.
- **DataStart**: Pin for the start of data, assigned as `1`.
