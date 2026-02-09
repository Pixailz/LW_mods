using LogicWorld.Rendering.Components;
using LogicWorld.ClientCode;
using LogicWorld.ClientCode.Resizing;
using LogicAPI.Data;
using JimmysUnityUtilities;
using UnityEngine;
using System;
using PixLogicUtils.Shared.CustomData;
using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client
{
	public class SevenSegmentClient :
		ComponentClientCode<ISevenSegmentData>,
		IColorableClientCode,
		IResizableX
	{
		public Color24 Color { get => Data.Color; set => Data.Color = value; }

		public string ColorsFileKey => "SevenSegment";
		public float MinColorValue => 0.0f;

		public int SizeX { get => Data.Size; set => Data.Size = value; }
		public int MaxX => CSevenSegment.MaxSize;
		public int MinX => CSevenSegment.MinSize;
		public float GridIntervalX => 1.0f;

		private readonly GpuColor OffColor = CSevenSegment.OffColor.ToGpuColor();

		public int size = CSevenSegment.DefaultSize;
		public float scale = CSevenSegment.DefaultSize * CSevenSegment.OriginalScale;

		private float width = CSevenSegment.Pos.Width;
		private float height = CSevenSegment.Pos.Height;
		private float offsetY = CSevenSegment.Pos.OffsetY;

		private Vector3[][] segmentPositions;

		public void rescale_value()
		{
			this.scale = this.size * CSevenSegment.OriginalScale;
			this.width = CSevenSegment.OriginalWidth * this.scale;
			this.height = CSevenSegment.OriginalHeight * this.scale;

			this.offsetY = 1f * this.scale;

			this.segmentPositions = CSevenSegment.Pos.GetSegmentPositions(
				this.width,
				this.height,
				this.offsetY,
				this.scale
			);
		}

		protected override void DataUpdate()
		{
			// Logger.Info($"{SizeX} != {this.size} ? {SizeX != this.size}");
			if (SizeX != this.size)
			{
				this.size = SizeX;
				this.rescale_value();

				// Scale the 7 segment blocks
				for (int i = 0; i < CSevenSegment.DefaultInput; i++)
				{
					SetBlockPosition(CSevenSegment.DefaultInput - i - 1, this.segmentPositions[i][0]);
					SetBlockScale(i, new Vector3(
						this.width / 2f,
						1f * this.scale,
						0.25f
					));
				}

				// Scale base block (block 7)
				SetBlockPosition(7, new Vector3(
					CGlobal.LSBDir * ((this.width / 2f) - CGlobal.Offset),
					0f,
					0f
				));
				SetBlockScale(7, new Vector3(
					this.width,
					this.height,
					0.5f
				));

				// Scale input positions
				for (int i = CSevenSegment.DefaultInput - 1; i >= 0; i--)
				{
					byte ii = Convert.ToByte(i);
					SetInputPosition(ii, new Vector3(
						(
							((CSevenSegment.DefaultInput - i) * 0.5f * this.scale) - CGlobal.Offset
						) * CGlobal.LSBDir,
						0.5f,
						-0.25f
					));
				}
			}
			QueueFrameUpdate();
		}

		private void debug_get_segment_state(bool state, int segment_id)
		{
			var segment = "Unknown";

			segment = "Unknown";

			switch (segment_id)
			{
				case 0:
					segment = "A";
					break;
				case 1:
					segment = "B";
					break;
				case 2:
					segment = "C";
					break;
				case 3:
					segment = "D";
					break;
				case 4:
					segment = "E";
					break;
				case 5:
					segment = "F";
					break;
				case 6:
					segment = "G";
					break;
				default:
              		Logger.Warn($"Unknown segment ID: {segment_id}");
					break;
			}

			Logger.Info($"Segment {segment} is {(state ? "ON" : "OFF")}");
		}

		protected override void FrameUpdate()
		{
			GpuColor OnColor = Data.Color.ToGpuColor();

			for (int i = 0; i < 7; i++)
			{
				bool isOn = GetInputState(i);

				// this.debug_get_segment_state(isOn, i);
				SetBlockColor(isOn ? OnColor : this.OffColor, i);
			}

			base.FrameUpdate();
		}

		protected override void SetDataDefaultValues()
		{
			Data.Initialize();
		}
	}
}
