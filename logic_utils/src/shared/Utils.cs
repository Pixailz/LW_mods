global using t_data = ulong;
global using t_width = int;

global using t_pin = int;

using JimmysUnityUtilities;
using UnityEngine;
using LogicAPI.Data;

namespace PixLogicUtils.Shared.Utils
{
	static public class Converter
	{
		static public Color ToColor(Color24 color24)
		{
			return new Color(
				color24.r / 255f,
				color24.g / 255f,
				color24.b / 255f
			);
		}

		static public Color ToColor(GpuColor gpuColor)
		{
			return new Color(
				gpuColor.r / 255f,
				gpuColor.g / 255f,
				gpuColor.b / 255f
			);
		}

		static public Color[] ToColor(GpuColor[] gpuColor)
		{
			Color[] retv = new Color[gpuColor.Length];

			for (int i = 0; i < gpuColor.Length; i++)
			{
				retv[i] = Converter.ToColor(gpuColor[i]);
			}
			return retv;
		}

		static public Color[] ToColor(Color24[] color24)
		{
			Color[] retv = new Color[color24.Length];

			for (int i = 0; i < color24.Length; i++)
			{
				retv[i] = Converter.ToColor(color24[i]);
			}
			return retv;
		}

		static public string ToString(bool value)
		{
			if (value)
				return "True";
			return "False";
		}
	}
}