using JimmysUnityUtilities;
using UnityEngine;
using System;

namespace PixLogicUtils.Shared.Config
{
	public static class CGlobal
	{
		// public static readonly int	LSBDir = 1; // LSB Left
		public static readonly int	LSBDir = -1; // LSB Right

		public static readonly float	Offset = 0.5f;

		public static readonly float	DataPinLength = 0.3f;
		public static readonly float	DataPinLengthStep = 0.02f;
		public static readonly float	ActionPinLength = 0.5f;

		public static readonly float	DecorationScale = 0.3f;
	}

	public static class CDecoder
	{
		public static readonly Color24	BlockColor = Color24.AbsoluteZero;
		public static readonly float	BlockHeight = 1f;
		public static readonly float	BlockDepth = 1f;

		public static readonly float	DataPinLength = CGlobal.DataPinLength;
		public static readonly float	DataPinLengthStep = CGlobal.DataPinLengthStep;

		public static readonly t_pin	DefaultInput = 2;
		public static readonly t_data	DefaultOutput = 1ul << DefaultInput;

		public static readonly t_width	MaxInput = 9;
		public static readonly t_width	MinInput = 1;
		public static readonly t_width	StepInput = 1;
	}

	public static class CRegister
	{
		public static readonly Color24	BlockColor = Color24.Acajou;
		public static readonly float	BlockHeight = 2f;
		public static readonly float	BlockDepth = 2f;

		public static readonly t_width	DefaultDataWidth = 8;
		public static readonly t_pin	DefaultInput = DefaultDataWidth + Pin.DataStart;
		public static readonly t_pin	DefaultOutput = DefaultDataWidth;

		public static readonly float	DataPinLength = CGlobal.DataPinLength;
		public static readonly float	DataPinLengthStep = CGlobal.DataPinLengthStep;
		public static readonly float	ActionPinLength = CGlobal.ActionPinLength;

		public static readonly t_pin	MaxInput = 64;
		public static readonly t_pin	MinInput = 2;
		public static readonly t_pin	StepInput = 2;

		public static class Pin
		{
			public static readonly t_pin	Clock		= 0;
			public static readonly t_pin	Write		= 1;
			public static readonly t_pin	Read		= 2;
			public static readonly t_pin	Low			= 3;
			public static readonly t_pin	High		= 4;
			public static readonly t_pin	Plus		= 5;
			public static readonly t_pin	Minus		= 6;
			public static readonly t_pin	DataStart	= 7;
		}

	}

	public static class CMultiReadRam
	{
		public static readonly Color24	BlockColor = Color24.AcidGreen;
		public static readonly float	BlockDepth = 2f;

		public static readonly t_width	DefaultAddressWidth = 4;
		public static readonly t_width	DefaultDataWidth = 4;

		public static readonly float	DataPinLength = CGlobal.DataPinLength;
		public static readonly float	DataPinLengthStep = CGlobal.DataPinLengthStep;
		public static readonly float	ActionPinLength = CGlobal.ActionPinLength;


		public static class Pin
		{
			public static readonly t_pin	Load		= 0;
			public static readonly t_pin	Write		= 1;
			public static readonly t_pin	DataStart	= 2;
		}
	}

	public static class CSevenSegment
	{
		public static readonly Color24	BlockColor = Color24.Black;
		public static readonly Color24	OffColor = new Color24(20, 20, 20);

		public static readonly t_pin	DefaultInput = 7;
		public static readonly t_pin	DefaultOutput = 0;

		public static readonly float	DataPinLength = CGlobal.DataPinLength;
		public static readonly float	DataPinLengthStep = CGlobal.DataPinLengthStep;

		public static readonly float	OriginalScale = 0.5f;
		public static readonly float	OriginalWidth = 4f;
		public static readonly float	OriginalHeight = 7f;
		public static readonly float	DepthOffset = 0.375f;

		public static readonly int	MaxSize = 32;
		public static readonly int	MinSize = 1;
		public static readonly int	DefaultSize = (int)Math.Round(1f / OriginalScale);

		public static class Pos
		{
			public static readonly float Width = OriginalWidth * OriginalScale;
			public static readonly float Height = OriginalHeight * OriginalScale;

			public static readonly float OffsetY = 1f * OriginalScale;

			public static readonly Vector3[][] segmentPositions = GetSegmentPositions(
				OriginalWidth,
				OriginalHeight,
				1f,
				1f
			);

			/* Display Layout:
			*   AAA
			*  F   B
			*  F   B
			*   GGG
			*  E   C
			*  E   C
			*   DDD
			*/
			public static Vector3[][] GetSegmentPositions(
				float w,
				float h,
				float offY,
				float scale
			)
			{
				// Branchless condition
				float x_off = (1 - CGlobal.LSBDir) / 2 * ((OriginalWidth * scale) - 1);

				float x_a = (w / 2f) - CGlobal.Offset;
				float y_a = h - offY;
				x_a -= x_off;

				float x_b = (1f * scale) - CGlobal.Offset;
				float y_b = (6f * scale) - offY;
				x_b -= x_off;
				float x_c = x_b;
				float y_c = (3f * scale) - offY;
				float x_d = x_a;
				float y_d = (1f * scale) - offY;
				float x_e = w - CGlobal.Offset;
				x_e -= x_off;
				float y_e = y_c;
				float x_f = x_e;
				float y_f = y_b;
				float x_g = x_a;
				float y_g = (4f * scale) - offY;

				Vector3[][] retv = [
					[
						new Vector3(x_a, y_a, DepthOffset),
						new Vector3(0f, 0f, 0f)
					],
					[
						new Vector3(x_b, y_b, DepthOffset),
						new Vector3(0f, 0f, 90f)
					],
					[
						new Vector3(x_c, y_c, DepthOffset),
						new Vector3(0f, 0f, 90f)
					],
					[
						new Vector3(x_d, y_d, DepthOffset),
						new Vector3(0f, 0f, 0f)
					],
					[
						new Vector3(x_e, y_e, DepthOffset),
						new Vector3(0f, 0f, 90f)
					],
					[
						new Vector3(x_f, y_f, DepthOffset),
						new Vector3(0f, 0f, 90f)
					],
					[
						new Vector3(x_g, y_g, DepthOffset),
						new Vector3(0f, 0f, 0f)
					]
				];

				return retv;
			}
		}
	}

	public static class CHexDisplay
	{
		public static readonly Color24	BlockColor = Color24.Black;
		public static readonly float	BlockDepth = 0.5f;

		public static readonly t_pin	DefaultInput = 5;
		public static readonly t_pin	DefaultOutput = 0;

		public static readonly int	FixedBPP = 1;

		public static readonly float	DataPinLength = CGlobal.DataPinLength;
		public static readonly float	DataPinLengthStep = CGlobal.DataPinLengthStep;
		public static readonly float	ActionPinLength = CGlobal.ActionPinLength;

		public static readonly float	OriginalScale = 0.2f;
		public static readonly float	OriginalWidth = 5f;
		public static readonly float	OriginalHeight = 9f;

		public static readonly int	MaxSize = 32;
		public static readonly int	MinSize = 1;
		public static readonly int	DefaultSize = (int)Math.Round(1f / OriginalScale);

		public static class Pin
		{
			public static readonly t_pin	OnOff = 0;
			public static readonly t_pin	DataStart = 1;
		}

		public static readonly byte[][] HexBitmap =
		[
			// OFF - blank display
			[
				0b00000,
				0b00000,
				0b00000,
				0b00000,
				0b00000,
				0b00000,
				0b00000,
				0b00000,
				0b00000,
			],
			// 0
			[
				0b01110,
				0b10001,
				0b10001,
				0b10001,
				0b10101,
				0b10001,
				0b10001,
				0b10001,
				0b01110,
			],
			// 1
			[
				0b00100,
				0b01100,
				0b00100,
				0b00100,
				0b00100,
				0b00100,
				0b00100,
				0b00100,
				0b01110,
			],
			// 2
			[
				0b01110,
				0b10001,
				0b00001,
				0b00001,
				0b00110,
				0b01000,
				0b10000,
				0b10000,
				0b11111,
			],
			// 3
			[
				0b01110,
				0b10001,
				0b00001,
				0b00001,
				0b01110,
				0b00001,
				0b00001,
				0b10001,
				0b01110,
			],
			// 4
			[
				0b00000,
				0b01000,
				0b01010,
				0b01010,
				0b10010,
				0b10010,
				0b11111,
				0b00010,
				0b00010,
			],
			// 5
			[
				0b11111,
				0b10000,
				0b10000,
				0b10000,
				0b11110,
				0b00001,
				0b00001,
				0b00001,
				0b11110,
			],
			// 6
			[
				0b01110,
				0b10001,
				0b10000,
				0b10000,
				0b11110,
				0b10001,
				0b10001,
				0b10001,
				0b01110,
			],
			// 7
			[
				0b11111,
				0b00001,
				0b00001,
				0b00010,
				0b00010,
				0b00100,
				0b00100,
				0b00100,
				0b00100,
			],
			// 8
			[
				0b01110,
				0b10001,
				0b10001,
				0b10001,
				0b01110,
				0b10001,
				0b10001,
				0b10001,
				0b01110,
			],
			// 9
			[
				0b01110,
				0b10001,
				0b10001,
				0b10001,
				0b01111,
				0b00001,
				0b00001,
				0b10001,
				0b01110,
			],
			// A
			[
				0b01110,
				0b10001,
				0b10001,
				0b10001,
				0b11111,
				0b10001,
				0b10001,
				0b10001,
				0b10001,
			],
			// B
			[
				0b11110,
				0b10001,
				0b10001,
				0b10001,
				0b11110,
				0b10001,
				0b10001,
				0b10001,
				0b11110,
			],
			// C
			[
				0b01110,
				0b10001,
				0b10000,
				0b10000,
				0b10000,
				0b10000,
				0b10000,
				0b10001,
				0b01110,
			],
			// D
			[
				0b11110,
				0b10001,
				0b10001,
				0b10001,
				0b10001,
				0b10001,
				0b10001,
				0b10001,
				0b11110,
			],
			// E
			[
				0b11111,
				0b10000,
				0b10000,
				0b10000,
				0b11110,
				0b10000,
				0b10000,
				0b10000,
				0b11111,
			],
			// F
			[
				0b11111,
				0b10000,
				0b10000,
				0b10000,
				0b11110,
				0b10000,
				0b10000,
				0b10000,
				0b10000,
			]
		];
	}

	public static class CScreen
	{
		public static readonly Color24	BlockColor = Color24.Black;
		public static readonly float	BlockDepth = 0.5f;

		public static readonly float	DataPinLength = CGlobal.DataPinLength;
		public static readonly float	DataPinLengthStep = CGlobal.DataPinLengthStep;
		public static readonly float	ActionPinLength = CGlobal.ActionPinLength;

		public static readonly float	OriginalScale = 0.25f;
		public static readonly int		DefaultSize = (int)Math.Round(1f / OriginalScale);

		public static readonly t_pin	DefaultDataSize = 64;
		public static readonly t_pin	DefaultInput = Pin.DataStart + DefaultDataSize;
		public static readonly t_pin	DefaultOutput = 1;

		public static readonly int	MinBPP = 1;
		public static readonly int	MaxBPP = 8;
		public static readonly int	DefaultBPP = 1;

		public static readonly int	MinResolutionX = 64;
		public static readonly int	MaxResolutionX = 1024;
		public static readonly int	DefaultResolutionX = 64;

		public static readonly int	MinResolutionY = MinResolutionX;
		public static readonly int	MaxResolutionY = MaxResolutionX;
		public static readonly int	DefaultResolutionY = DefaultResolutionX;

		public static readonly int	MaxDelayOnEndPulse = 16;
		public static readonly int	MinDelayOnEndPulse = -MaxDelayOnEndPulse;
		public static readonly int	DefaultDelayOnEndPulse = 0;

		public static class Pin
		{
			public static readonly t_pin	EndPulse = 0;
			public static readonly t_pin	Clock = 0;
			public static readonly t_pin	DataStart = 1;
		}
	}

	public static class CDoubleDabble
	{
		public static readonly Color24	BlockColor = Color24.Aero;
		public static readonly float	BlockDepth = 1f;
		public static readonly float	BlockHeight = 1f;

		public static readonly t_width	DefaultDataWidth	= 4;
		public static readonly t_width	MaxDataWidth		= 64;
		public static readonly t_pin	MinDataWidth		= 4;
		public static readonly t_pin	StepDataWidth		= 4;

		public static readonly float	DataPinLength		= CGlobal.DataPinLength;
		public static readonly float	DataPinLengthStep	= CGlobal.DataPinLengthStep;
		public static readonly float	ActionPinLength		= CGlobal.ActionPinLength;

		public static readonly t_pin	DefaultInput	= DefaultDataWidth;
		public static readonly t_pin	DefaultOutput	= WidthToOutput(DefaultInput);

		public static class Pin
		{
			public static readonly t_pin	DataStart	= 0;
		}

		public static t_pin WidthToOutput(t_width width)
		{
			if (width == 64)
				return 80;
			double n = Math.Pow(2, width) - 1;
			return ((ulong)n).ToString().Length * 4;
		}

		// public static t_width OutputToWidth(t_pin output)
		// {
		// 	int len_dec = output / 4;
		// 	double min_value = Math.Pow(10, len_dec - 1);
		// 	int width_raw = (int)Math.Ceiling(Math.Log2(min_value + 1));

		// 	t_width div = width_raw / 4;
		// 	t_width mod = width_raw % 4 > 0 ? 1 : 0;

		// 	return (div + mod) * 4;
		// }
	}

	public static class CPulser
	{
		public static readonly Color24	BlockColor = Color24.AfricanViolet;

		public static readonly float	BlockDepth	= 1f;
		public static readonly float	BlockHeight	= 1f;
		public static readonly float	BlockWidth	= 1f;

		public static readonly int	DefaultInput	= 2;
		public static readonly int	DefaultOutput	= 0;

		public static readonly float	ActionPinLength		= CGlobal.ActionPinLength;

		public static class Pin
		{
			public static readonly t_pin	In = 0;
			public static readonly t_pin	Out = 1;
		}
	}

	public static class CMultiplexer
	{
		public static readonly Color24	BlockColor = Color24.AirForceBlueRAF;

		public static readonly float	BlockDepth	= 1f;

		public static readonly float	DataPinLength		= CGlobal.DataPinLength;
		public static readonly float	DataPinLengthStep	= CGlobal.DataPinLengthStep;
		public static readonly float	ActionPinLength		= CGlobal.ActionPinLength;

		public static readonly t_width	DefaultDataWidth	= 4;
		public static readonly t_width	MaxDataWidth		= 64;
		public static readonly t_pin	MinDataWidth		= 1;
		public static readonly t_pin	StepDataWidth		= 1;

		public static readonly t_width	DefaultSelectorWidth	= 1;
		public static readonly t_width	MaxSelectorWidth		= 4;
		public static readonly t_pin	MinSelectorWidth		= 1;
		public static readonly t_pin	StepSelectorWidth		= 1;

		public static readonly int	DefaultInput	= (
			DefaultDataWidth * (1 << DefaultSelectorWidth)
		) + DefaultSelectorWidth;
		public static readonly int	DefaultOutput	= DefaultDataWidth;

	}

	public static class CDemultiplexer
	{
		public static readonly Color24	BlockColor = Color24.AlabamaCrimson;

		public static readonly float	BlockDepth	= 1f;

		public static readonly float	DataPinLength		= CGlobal.DataPinLength;
		public static readonly float	DataPinLengthStep	= CGlobal.DataPinLengthStep;
		public static readonly float	ActionPinLength		= CGlobal.ActionPinLength;

		public static readonly t_width	DefaultDataWidth	= 4;
		public static readonly t_width	MaxDataWidth		= 64;
		public static readonly t_pin	MinDataWidth		= 2;
		public static readonly t_pin	StepDataWidth		= 1;

		public static readonly t_width	DefaultSelectorWidth	= 1;
		public static readonly t_width	MaxSelectorWidth		= 4;
		public static readonly t_pin	MinSelectorWidth		= 1;
		public static readonly t_pin	StepSelectorWidth		= 1;

		public static readonly int	DefaultInput	= DefaultDataWidth + DefaultSelectorWidth;
		public static readonly int	DefaultOutput	= DefaultDataWidth * (1 << DefaultSelectorWidth);
	}
}
