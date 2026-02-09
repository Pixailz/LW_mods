using LogicWorld.Rendering.Components;
using LogicWorld.Rendering.Chunks;
using LogicWorld.ClientCode.Resizing;
using JimmysUnityUtilities;
using UnityEngine;
using System;
using LogicWorld.Interfaces;

using PixLogicUtils.Shared.Utils;
using PixLogicUtils.Shared.CustomData;
using PixLogicUtils.Shared.Config;

namespace PixLogicUtils.Client
{
	public class ScreenClient :
		ComponentClientCode<IScreenData>,
		IResizableX
	{
		public int SizeX { get => Data.Size; set => Data.Size = value; }
		public int MinX => 1;
		public int MaxX => 32;
		public float GridIntervalX => 1f;

		private int currentSize = 0;

		private Texture2D displayTexture;
		private Color[] pixelBuffer;

		protected override void Initialize()
		{
			// syncIfNeeded();
		}

		protected override void SetDataDefaultValues()
		{
			Data.Initialize();
		}

		public void createTexture(int width, int height)
		{
			if (displayTexture != null)
			{
				// displayTexture = new Texture2D(width, height)
				// {
				// 	filterMode = FilterMode.Point
				// };
				displayTexture.Reinitialize(width, height);
			}
			else
			{
				displayTexture = new Texture2D(width, height)
				{
					filterMode = FilterMode.Point
				};
			}
		}

		public void syncIfNeeded()
		{
			bool pixelBufferMismatch =
				pixelBuffer == null
				|| pixelBuffer.Length != this.Data.ResolutionX * this.Data.ResolutionY;
			bool textureMismatch =
				displayTexture == null
				|| displayTexture.width != this.Data.ResolutionX
				|| displayTexture.height != this.Data.ResolutionY;
			bool pixelDataMismatch =
				this.Data.PixelData == null
				|| this.Data.PixelData.Length != this.Data.ResolutionX * this.Data.ResolutionY;
			int res_x =
				this.Data.ResolutionX != 0
				? this.Data.ResolutionX
				: CScreen.DefaultResolutionX;
			int res_y =
				this.Data.ResolutionY != 0
				? this.Data.ResolutionY
				: CScreen.DefaultResolutionY;
			if (pixelBufferMismatch)
			{
				Logger.Info(
					$"Pixel buffer mismatch: {(
						pixelBuffer == null
						? "null"
						: $"length={pixelBuffer.Length}"
					)}, expected length={this.Data.ResolutionX * this.Data.ResolutionY}"
				);
				pixelBuffer = new Color[res_x * res_y];
			}
			if (textureMismatch)
			{
				Logger.Info(
					$"Display texture mismatch: {(
						displayTexture == null
						? "null"
						: $"{displayTexture.width}x{displayTexture.height}"
					)}, expected size={Data.ResolutionX}x{Data.ResolutionY}"
				);
				createTexture(res_x, res_y);
			}
			if (pixelDataMismatch)
			{
				Logger.Info(
					$"Pixel data mismatch: {(
						Data.PixelData == null
						? "null"
						: $"length={Data.PixelData.Length}"
					)}, expected length={res_x * res_y}"
				);
				this.Data.PixelData = new byte[res_x * res_y];
			}
			if (pixelBufferMismatch || textureMismatch)
			{
				currentSize = 0;
			}
		}

		private void debug_put_pixel_in_corner(Color[] currentColors)
		{
			Color upper_right = Converter.ToColor(Color24.Red);
			Color lower_left = Converter.ToColor(Color24.Blue);
			Color upper_left = Converter.ToColor(Color24.Green);
			Color lower_right = Converter.ToColor(Color24.Yellow);

			if (currentColors != null && currentColors.Length >= 4)
			{
				upper_right = currentColors[0];
				lower_left = currentColors[1];
				upper_left = currentColors[2];
				lower_right = currentColors[3];
			}
			// upper right
			pixelBuffer[0] = upper_right;
			// lower left
			pixelBuffer[Data.ResolutionY * Data.ResolutionX - 1] = lower_left;
			// upper left
			pixelBuffer[Data.ResolutionX - 1] = upper_left;
			// lower right
			pixelBuffer[Data.ResolutionX * (Data.ResolutionY - 1)] = lower_right;
		}

		private void debug_put_data_in_corner(Color[] currentColors)
		{
			byte upper_right = 1;
			byte lower_left = 1;
			byte upper_left = 1;
			byte lower_right = 1;

			if (currentColors != null && currentColors.Length >= 4)
			{
				upper_right = 1;
				lower_left = 1;
				upper_left = 2;
				lower_right = 3;
			}
			this.Data.PixelData[0] = upper_right;
			this.Data.PixelData[this.Data.ResolutionX * this.Data.ResolutionY - 1] = lower_left;
			this.Data.PixelData[Data.ResolutionX - 1] = upper_left;
			this.Data.PixelData[Data.ResolutionX * (Data.ResolutionY - 1)] = lower_right;
		}

		protected override void DataUpdate()
		{
			syncIfNeeded();
			UpdateScaleIfNeeded();
			QueueFrameUpdate();
		}

		protected override void FrameUpdate()
		{
			RenderPixelBufferToTexture();
		}

		private Color[] getColorFromConfig()
		{
			Color[] retv = Converter.ToColor([
				Color24.Black,
				Color24.White
			]);

			// Check if MainWorld is available (not available during prefab generation)
			var displayConfigs = Instances.MainWorld?.Renderer?.DisplayConfigurations;
			if (displayConfigs != null)
			{
				var configAddress = new DisplayConfigurationAddress(
					Data.BitsPerPixel,
					Data.ConfigurationIndex
				);
				displayConfigs.RunOnConfiguration(configAddress, (colors) =>
				{
					if (colors != null && colors.Length == (1 << Data.BitsPerPixel))
					{
						retv = Converter.ToColor(colors);
					}
					else
					{
						Logger.Warn($"[Screen] ConfigIndex={Data.ConfigurationIndex}, BPP={Data.BitsPerPixel}, Colors array is null or has invalid length (length={colors?.Length ?? 0})");
					}
				});
			}
			return retv;
		}

		public void RenderPixelBufferToTexture()
		{
			Color[] currentColors = getColorFromConfig();

			syncIfNeeded();

			if (currentColors == null || currentColors.Length == 0)
			{
				int colorCount = 1 << Data.BitsPerPixel;
				currentColors = new Color[colorCount];
				for (int i = 0; i < colorCount; i++)
				{
					float grayscale = i / (float)(colorCount - 1);
					currentColors[i] = new Color(grayscale, grayscale, grayscale);
				}
			}

			// this.debug_put_data_in_corner(currentColors);

			for (int y = 0; y < Data.ResolutionY; y++)
			{
				for (int x = 0; x < Data.ResolutionX; x++)
				{
					// int index = y * Data.ResolutionX + x;
					// if (index < pixelBuffer.Length)
					// {
					// 	int invertedIndex = (Data.ResolutionY - 1 - y) * Data.ResolutionX + x;
					// 	pixelBuffer[invertedIndex] = pixelBuffer[index];
					// }
					int index = y * Data.ResolutionX + x;
					int color_index = Data.PixelData[index] % currentColors.Length;
					pixelBuffer[index] = currentColors[color_index];
				}
			}

			// this.debug_put_pixel_in_corner(currentColors);

			displayTexture.SetPixels(pixelBuffer);
			displayTexture.Apply();
		}

		protected override IDecoration[] GenerateDecorations(Transform parentToCreateDecorationsUnder)
		{
			syncIfNeeded();
			if (displayTexture != null)
			{
				displayTexture.Reinitialize(Data.ResolutionX, Data.ResolutionY);
			}
			float currentScale = CScreen.OriginalScale * this.Data.Size;
			Material material = new Material(Shader.Find("Unlit/Texture"));
			material.mainTexture = displayTexture;

			GameObject quadObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
			quadObject.transform.localScale = new Vector3(
				currentScale * Data.ResolutionX * CGlobal.DecorationScale,
				currentScale * Data.ResolutionY * CGlobal.DecorationScale,
				1f
			);
			quadObject.GetComponent<Renderer>().material = material;
			quadObject.transform.SetParent(parentToCreateDecorationsUnder);

			return
			[
				new Decoration
				{
					// LocalPosition = new Vector3(
					// 	(currentScale * Data.ResolutionX / 2f - 0.5f) * CGlobal.DecorationScale,
					// 	(currentScale * Data.ResolutionY / 2f) * CGlobal.DecorationScale,
					// 	(-0.25f * CGlobal.DecorationScale) - 0.0005f
					// ),
					LocalRotation = Quaternion.Euler(0f, 180f, 180f),
					DecorationObject = quadObject,
					AutoSetupColliders = true,
					IncludeInModels = true
				}
			];
		}

		public void UpdateScaleIfNeeded()
		{
			if (currentSize == SizeX)
				return;

			currentSize = SizeX;

			float scale = CScreen.OriginalScale * currentSize;

			SetOutputPosition((byte)CScreen.Pin.EndPulse, new Vector3(
				0f,
				1.25f + 0.5f,
				-0.25f
			));

			SetInputPosition((byte)CScreen.Pin.Clock, new Vector3(
				0f,
				2.25f + 0.5f,
				-0.25f
			));

			for (
				int i = CScreen.Pin.DataStart;
				i < CScreen.DefaultDataSize + CScreen.Pin.DataStart;
				i++
			)
			{
				byte inputIndex = Convert.ToByte(i);
				SetInputPosition(inputIndex, new Vector3(
					CGlobal.LSBDir * ((i - CScreen.Pin.DataStart) * scale),
					0.75f,
					-0.25f
				));
			}

			// Base block
			SetBlockPosition(0, new Vector3(
				CGlobal.LSBDir * ((Data.ResolutionX * scale / 2f) - CGlobal.Offset),
				0f,
				0f
			));
			SetBlockScale(0, new Vector3(
				scale * Data.ResolutionX,
				scale * Data.ResolutionY,
				CScreen.BlockDepth
			));

			// Decoration (texture)
			SetDecorationPosition(0, new Vector3(
				((Data.ResolutionX * scale / 2f) - CGlobal.Offset) * CGlobal.LSBDir * CGlobal.DecorationScale,
				(Data.ResolutionY * scale / 2f) * CGlobal.DecorationScale,
				(0.25f * CGlobal.DecorationScale) + 0.0005f
			));
			SetDecorationScale(0, new Vector3(
				scale * Data.ResolutionX * CGlobal.DecorationScale,
				scale * Data.ResolutionY * CGlobal.DecorationScale,
				1f
			));
		}
	}
}
