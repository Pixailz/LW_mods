using LogicAPI.Server.Components;

namespace PixLogicUtils.Server
{
	public class DoubleDabbleServer : LogicComponent
	{
		private void DoDoubleDabble(t_data n)
		{
			Utils.ResetOutput(Outputs);
			for (int i = 1; n > 0; i++, n /= 10)
			{
				Utils.ByteToOutput(Outputs, n % 10, 4, Outputs.Count - (4 * i));
			}
		}

		protected override void DoLogicUpdate()
		{
			t_data n = Utils.InputToByte(Inputs, Inputs.Count);

			DoDoubleDabble(n);
		}
	}
}
