# Decoder

## Preview

<p>
  <img src="https://github.com/Pix/logic_utils/blob/main/logic_utils/docs/assets/decoder/front.jpg" alt="Decoder Front" width="300"/>
  <img src="https://github.com/Pix/logic_utils/blob/main/logic_utils/docs/assets/decoder/back.jpg" alt="Decoder Back" width="300"/>
  <img src="https://github.com/Pix/logic_utils/blob/main/logic_utils/docs/assets/decoder/menu.png" alt="Decoder Menu" width="600"/>
</p>

## Description

A binary-to-decimal decoder component that activates one output based on binary input.

**Features:**
- Configurable input count (1-9 bits)
- Outputs: 2^(input count)
- Flippable component
- Fine rotation allowed

**Location:** [`src/client/Decoder/DecoderPrefab.cs`](https://github.com/Pix/logic_utils/blob/main/logic_utils/src/client/Decoder/DecoderPrefab.cs)

## **Configuration**

### in game menu

- Input Count: 1-9 (default: 2, step: 1)

### in code

[Config.cs](https://github.com/Pix/logic_utils/blob/main/logic_utils/src/shared/Config.cs#L22)

- **BlockColor**: Sets the color of the decoder block. Default is `Color24.AbsoluteZero`.
- **BlockHeight**: Height of the decoder block. Default is `1f`.
- **BlockDepth**: Depth of the decoder block. Default is `1f`.
- **DataPinLength**: Length of the data pin, derived from `CGlobal.DataPinLength`.
- **DataPinLengthStep**: Step size for the data pin length, derived from `CGlobal.DataPinLengthStep`.
- **DefaultInput**: Default number of input bits. Default is `2`.
- **DefaultOutput**: Default number of outputs, calculated as 2^DefaultInput. Default is `4`.
- **MaxInput**: Maximum input count. Default is `9`.
- **MinInput**: Minimum input count. Default is `1`.
- **StepInput**: Step size for input count. Default is `1`.
