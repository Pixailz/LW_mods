using LogicAPI.Server.Components;

using PixLogicUtils.Server;

namespace PixLogicUtils.Server
{
	public class DecoderServer : LogicComponent
	{
		protected override void DoLogicUpdate()
		{
			int	input = (int)Utils.InputToByte(Inputs, Inputs.Count);

			Utils.ResetOutput(Outputs);
			Outputs[input].On = true;
		}
	}
}
