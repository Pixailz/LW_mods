# Seven Segment Display

## Preview

<p>
  <img src="https://github.com/Pix/logic_utils/blob/main/logic_utils/docs/assets/seven_segment/front.jpg" alt="Seven Segment Display Front" width="300"/>
  <img src="https://github.com/Pix/logic_utils/blob/main/logic_utils/docs/assets/seven_segment/back.jpg" alt="Seven Segment Display Back" width="300"/>
  <img src="https://github.com/Pix/logic_utils/blob/main/logic_utils/docs/assets/seven_segment/menu.png" alt="Seven Segment Display Menu" width="600"/>
</p>

## Description

A classic seven-segment display for showing single digits or characters.

**Features:**
- 7 input pins (segments A-G)
- Customizable color
- Resizable (1-32 units)
- Standard seven-segment layout

**Location:** [`src/client/SevenSegment/SevenSegmentClient.cs`](https://github.com/Pix/logic_utils/blob/main/logic_utils/src/client/SevenSegment/SevenSegmentClient.cs)

## **Configuration**

### in game menu

- Size: 1-32 (default: 2)
- Color: Customizable

### in code

[Config.cs](https://github.com/Pix/logic_utils/blob/main/logic_utils/src/shared/Config.cs#L92)

- **BlockColor**: Sets the base color of the display block. Default is `Color24.Black`.
- **OffColor**: Color when segments are off. Default is `new Color24(20, 20, 20)`.
- **DefaultInput**: Default number of input pins. Default is `7`.
- **DataPinLength**: Length of the data pin, derived from `CGlobal.DataPinLength`.
- **DataPinLengthStep**: Step size for the data pin length, derived from `CGlobal.DataPinLengthStep`.
- **OriginalScale**: Original scale of the display. Default is `0.5f`.
- **OriginalWidth**: Width of the original segment layout. Default is `4f`.
- **OriginalHeight**: Height of the original segment layout. Default is `7f`.
- **DepthOffset**: Offset for depth positioning. Default is `0.375f`.
- **MaxSize**: Maximum display size. Default is `32`.
- **MinSize**: Minimum display size. Default is `1`.
- **DefaultSize**: Default size, calculated from the original scale. Default is `2`.
