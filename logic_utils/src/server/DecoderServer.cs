using LogicAPI.Server.Components;

using System;

using LICC;

namespace PixLogicUtils.Server
{
    public class DecoderServer : LogicComponent
    {
		public int	inputToByte(int start, int size)
		{
			int tmp = 0;

			// for (var i = start; i < start + size; i++)
			for (int i = start + size - 1; i >= start; i--)
			{
				tmp <<= 1;
				if (Inputs[i].On)
					tmp++;
			}
			return tmp;
		}

		public void	resetOutput()
		{
			foreach (var Output in Outputs)
				Output.On = false;
		}

		public void setOutput(int index)
		{
			Outputs[index].On = true;
		}

        protected override void DoLogicUpdate()
        {
			int	n_input = Inputs.Count;
			int	input = inputToByte(0, n_input);

			LConsole.WriteLine("N Input " + n_input);
			LConsole.WriteLine("Input   " + input);

			resetOutput();
			setOutput(input);
        }
    }
}
