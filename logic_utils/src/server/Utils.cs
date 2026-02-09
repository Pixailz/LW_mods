global using t_inlist = System.Collections.Generic.IReadOnlyList<
	LogicAPI.Server.Components.IInputPeg
>;
global using t_outlist = System.Collections.Generic.IReadOnlyList<
	LogicAPI.Server.Components.IOutputPeg
>;

namespace PixLogicUtils.Server
{
	public class Utils
	{
		// Input
		public static t_data InputToByte(
			t_inlist Inputs,
			t_width size,
			t_pin start = 0
		)
		{
			t_data tmp = 0;

			for (int i = start + size - 1; i >= start; i--)
			{
				tmp <<= 1;
				if (Inputs[i].On)
					tmp++;
			}
			return tmp;
		}

		// Output
		public static void ByteToOutput(
			t_outlist Outputs,
			t_data value,
			t_width size,
			t_pin start = 0
		)
		{
			for (int i = start; i < start + size; i++)
			{
				if ((value & 1) > 0)
					Outputs[i].On = true;
				else
					Outputs[i].On = false;
				value >>= 1;
			}
		}

		public static void ResetOutput(t_outlist Outputs)
		{
			foreach (var Output in Outputs)
				Output.On = false;
		}

		// Bit Operation
		public static byte BitSwap(byte octet)
		{
			return (byte)(
				((octet & 0x0f) << 4) |
				((octet & 0xf0) >> 4)
			);
		}

		public static t_data UpperWidth(t_width width)
		{
			t_data div = (t_data)width / 8;
			t_data mod = width % 8 > 0 ? 1ul : 0ul;

			return div + mod;
		}

		public static t_data GetMask(t_width size)
		{
			if (size >= 64)
				return t_data.MaxValue;
			return (1ul << size) - 1;
		}
	}
}