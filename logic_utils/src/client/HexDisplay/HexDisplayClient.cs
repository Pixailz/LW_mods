using LogicWorld.Rendering.Components;
using LogicWorld.Rendering.Chunks;
using LogicWorld.ClientCode;
using LogicWorld.ClientCode.Resizing;
using JimmysUnityUtilities;
using UnityEngine;
using System;
using LogicWorld.Interfaces;

using PixLogicUtils.Shared.Utils;
using PixLogicUtils.Shared.Config;
using PixLogicUtils.Shared.CustomData;

namespace PixLogicUtils.Client
{
	public class HexDisplayClient : ComponentClientCode<IHexDisplayData>,
		IResizableX
	{
		public int SizeX { get => Data.Size; set => Data.Size = value; }
		public int MaxX => CHexDisplay.MaxSize;
		public int MinX => CHexDisplay.MinSize;
		public float GridIntervalX => 1f;

		private Texture2D	displayTexture;
		private Color[]		pixelBuffer;
		private int			currentSize = 0;

		protected override void Initialize()
		{
			EnsureTexture();
			EnsurePixelBuffer();
		}

		private void EnsureTexture()
		{
			if (displayTexture != null)
				return;

			displayTexture = new Texture2D((int)CHexDisplay.OriginalWidth, (int)CHexDisplay.OriginalHeight);
			displayTexture.filterMode = FilterMode.Point;
		}

		private void EnsurePixelBuffer()
		{
			if (pixelBuffer != null)
				return ;
			pixelBuffer = new Color[(int)(CHexDisplay.OriginalWidth * CHexDisplay.OriginalHeight)];
		}

		protected override void SetDataDefaultValues()
		{
			Data.Initialize();
		}

		public float GetCurrentScale()
		{
			return CHexDisplay.OriginalScale * this.Data.Size;
		}

		protected override void DataUpdate()
		{
			UpdateScaleIfNeeded();
			QueueFrameUpdate();
		}

		protected override void FrameUpdate()
		{
			bool isOn = GetInputState(CHexDisplay.Pin.OnOff);
			byte bitmapIndex = GetInputValue();

			if (!isOn)
				bitmapIndex = 0;
			else if (bitmapIndex <= 15 && bitmapIndex >= 0)
				bitmapIndex += 1;
			else if (bitmapIndex > 15)
				bitmapIndex = 16;

			RenderBitmapToTexture(bitmapIndex);
		}

		private byte GetInputValue()
		{
			byte value = 0;
			for (int i = 0; i < 4; i++)
			{
				if (GetInputState((byte)(CHexDisplay.Pin.DataStart + i)))
				{
					value |= (byte)(1 << i);
				}
			}
			return value;
		}

		private Color[] getColorFromConfig()
		{
			Color[] retv = Converter.ToColor([
				Color24.Black,
				Color24.White
			]);

			if (Instances.MainWorld?.Renderer?.DisplayConfigurations != null)
			{
				var displayConfigs = Instances.MainWorld.Renderer.DisplayConfigurations;
				var configAddress = new DisplayConfigurationAddress(
					CHexDisplay.FixedBPP,
					this.Data.ConfigurationIndex
				);
				displayConfigs.RunOnConfiguration(configAddress, (colors) =>
				{
					if (colors != null && colors.Length == 2)
					{
						retv = Converter.ToColor(colors);
					}
					else
					{
						Logger.Warn($"[HexDisplay] ConfigIndex={this.Data.ConfigurationIndex}, BPP={CHexDisplay.FixedBPP}, Colors array is null or has invalid length (length={colors?.Length ?? 0})");
					}
				});
			}
			return retv;
		}

		private void RenderBitmapToTexture(int bitmapIndex)
		{
			byte[] bitmap = CHexDisplay.HexBitmap[
				bitmapIndex % CHexDisplay.HexBitmap.Length
			];

			Color[] currentColors = this.getColorFromConfig();

			for (int y = 0; y < CHexDisplay.OriginalHeight; y++)
			{
				byte row = bitmap[y];
				for (int x = 0; x < CHexDisplay.OriginalWidth; x++)
				{
					pixelBuffer[(int)(y * CHexDisplay.OriginalWidth + x)] = currentColors[(row >> x) & 1];
				}
			}

			displayTexture.SetPixels(pixelBuffer);
			displayTexture.Apply();
		}

		protected override IDecoration[] GenerateDecorations(Transform parentToCreateDecorationsUnder)
		{
			EnsureTexture();

			Material material = new Material(Shader.Find("Unlit/Texture"));
			material.mainTexture = displayTexture;

			GameObject quadObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
			quadObject.GetComponent<Renderer>().material = material;
			quadObject.transform.SetParent(parentToCreateDecorationsUnder);

			return
			[
				new Decoration
				{
					LocalPosition = new Vector3(0f, 0f, 0f),
					LocalRotation = Quaternion.Euler(0f, 180f, 180f),
					DecorationObject = quadObject,
					AutoSetupColliders = true,
					IncludeInModels = true
				}
			];
		}

		private void UpdateScaleIfNeeded()
		{
			if (this.currentSize == SizeX)
				return;

			this.currentSize = SizeX;
			float scale = GetCurrentScale();
			float halfWidth = (scale * CHexDisplay.OriginalWidth / 2f) - CGlobal.Offset;
			float halfHeight = scale * CHexDisplay.OriginalHeight / 2f;

			// Inputs
			SetInputPosition(
				(byte)CHexDisplay.Pin.OnOff,
				new Vector3(
					CGlobal.LSBDir * (
						(scale * (CHexDisplay.OriginalWidth - 1f)) - (0.6f - scale * 0.6f)
					),
					1f,
					-0.25f
				)
			);

			for (
				int i = CHexDisplay.Pin.DataStart;
				i < 4 + CHexDisplay.Pin.DataStart;
				i++
			)
			{
				byte inputIndex = Convert.ToByte(i);
				SetInputPosition(inputIndex, new Vector3(
					(
						((i - CHexDisplay.Pin.DataStart) * scale) - (0.4f - scale * 0.4f)
					) * CGlobal.LSBDir,
					0.25f + (0.5f * CHexDisplay.OriginalScale),
					-0.25f
				));
			}

			// Base block
			SetBlockPosition(0, new Vector3(
				CGlobal.LSBDir * halfWidth,
				0f,
				0f
			));
			SetBlockScale(0, new Vector3(
				scale * CHexDisplay.OriginalWidth,
				scale * CHexDisplay.OriginalHeight,
				CHexDisplay.BlockDepth
			));

			// Decoration (texture)
			SetDecorationPosition(0, new Vector3(
				CGlobal.LSBDir * halfWidth * CGlobal.DecorationScale,
				halfHeight * CGlobal.DecorationScale,
				(0.25f * CGlobal.DecorationScale) + 0.0005f
			));
			SetDecorationScale(0, new Vector3(
				scale * CHexDisplay.OriginalWidth * CGlobal.DecorationScale,
				scale * CHexDisplay.OriginalHeight * CGlobal.DecorationScale,
				1f
			));
		}
	}
}