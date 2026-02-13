using LogicWorld.Rendering.Components;
using LogicWorld.Rendering.Chunks;
using LogicWorld.ClientCode.Resizing;
using JimmysUnityUtilities;
using UnityEngine;
using System;
using System.Timers;

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

		private Texture2D	displayTexture;
		private Color[]		pixelBuffer;
		private Color[]		currentColors;
		private int			pixelCoverage;

		private Timer		renderFrameTimer = null;

		protected override void Initialize()
		{
			if (renderFrameTimer == null)
			{
				renderFrameTimer = new Timer
				{
					Interval = 1_000 / 60,
					AutoReset = true
				};

				renderFrameTimer.Elapsed += (sender, e) =>
				{
					QueueRenderPixelBuffer();
				};
				renderFrameTimer.Start();
			}
		}

		protected override void SetDataDefaultValues()
		{
			Data.Initialize();
		}

		public void createTexture(int width, int height)
		{
			if (displayTexture != null)
			{
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
			if (
				displayTexture == null
				|| displayTexture.width != this.Data.ResolutionX
				|| displayTexture.height != this.Data.ResolutionY
			)
			{
				int res_x =
					this.Data.ResolutionX != 0
					? this.Data.ResolutionX
					: CScreen.DefaultResolutionX;
				int res_y =
					this.Data.ResolutionY != 0
					? this.Data.ResolutionY
					: CScreen.DefaultResolutionY;
				createTexture(res_x, res_y);
				pixelBuffer = new Color[res_x * res_y];
				currentSize = 0;
			}
		}

		private void debug_put_data_in_corner()
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
		private bool can_render = false;

		private void QueueRenderPixelBuffer()
		{
			if (this.can_render)
				return ;
			this.can_render = true;
		}

		protected override void FrameUpdate()
		{
			if (!this.can_render)
				return ;
			currentColors = getColorFromConfig();
			pixelCoverage = (int)Math.Floor(
				(float)(CScreen.DefaultDataSize / this.Data.BitsPerPixel)
			);
			RenderPixelBufferToTextureNew();
			this.can_render = false;
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
					this.Data.BitsPerPixel,
					this.Data.ConfigurationIndex
				);
				displayConfigs.RunOnConfiguration(configAddress, (colors) =>
				{
					if (colors != null && colors.Length == (1 << this.Data.BitsPerPixel))
					{
						retv = Converter.ToColor(colors);
					}
				});
			}
			return retv;
		}

		public void RenderPixelBufferToTextureOld()
		{
			// this.debug_put_data_in_corner(currentColors);

			for (int i = 0; i < this.Data.PixelData.Length; i++)
			{
				int color_index = Data.PixelData[i] % currentColors.Length;
				pixelBuffer[i] = currentColors[color_index];
			}

			// this.debug_put_pixel_in_corner(currentColors);

			displayTexture.SetPixels(pixelBuffer);
			displayTexture.Apply();
			Logger.Info("Frame Updated");
		}

		private t_data lastAddress = 0;

		public void RenderPixelBufferToTexture(int start, int end)
		{
			for (
				int i = start;
				i < end && i < this.Data.PixelData.Length;
				i++
			)
			{
				int color_index = this.Data.PixelData[i] % currentColors.Length;
				int x = i % this.Data.ResolutionX;
				int y = i / this.Data.ResolutionX;

				displayTexture.SetPixel(x, y, currentColors[color_index]);
			}
		}

		public void RenderPixelBufferToTextureNewNormal()
		{
			this.RenderPixelBufferToTexture(
				(int)this.lastAddress,
				(int)this.Data.CurrentAddress
			);
		}

		public void RenderPixelBufferToTextureNewCutted(t_data maxAddress)
		{
			this.RenderPixelBufferToTexture(
				(int)this.lastAddress,
				(int)maxAddress
			);
			this.RenderPixelBufferToTexture(
				0,
				(int)this.Data.CurrentAddress
			);
		}

		public void RenderPixelBufferToTextureNew()
		{
			t_data maxAddress =
				(t_data)(this.Data.ResolutionX * this.Data.ResolutionY);

			if (this.lastAddress < this.Data.CurrentAddress)
				this.RenderPixelBufferToTextureNewNormal();
			else
				this.RenderPixelBufferToTextureNewCutted(maxAddress);

			// this.debug_put_data_in_corner();

			displayTexture.Apply();
			lastAddress = this.Data.CurrentAddress;
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
				CGlobal.LSBDir * ((this.Data.ResolutionX * scale / 2f) - CGlobal.Offset),
				0f,
				0f
			));
			SetBlockScale(0, new Vector3(
				scale * this.Data.ResolutionX,
				scale * this.Data.ResolutionY,
				CScreen.BlockDepth
			));

			// Decoration (texture)
			SetDecorationPosition(0, new Vector3(
				((this.Data.ResolutionX * scale / 2f) - CGlobal.Offset) * CGlobal.LSBDir * CGlobal.DecorationScale,
				(this.Data.ResolutionY * scale / 2f) * CGlobal.DecorationScale,
				(0.25f * CGlobal.DecorationScale) + 0.0005f
			));
			SetDecorationScale(0, new Vector3(
				scale * this.Data.ResolutionX * CGlobal.DecorationScale,
				scale * Data.ResolutionY * CGlobal.DecorationScale,
				1f
			));
		}
	}
}
