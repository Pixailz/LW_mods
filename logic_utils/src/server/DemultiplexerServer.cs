using LogicWorld.Server.Circuitry;

using PixLogicUtils.Shared.CustomData;

namespace PixLogicUtils.Server
{
	public class DemultiplexerServer : LogicComponent<IMultiplexerData>
	{
		public override bool	HasPersistentValues => true;

		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}

		protected override void DoLogicUpdate()
		{
			t_data data = Utils.InputToByte(Inputs,
				this.Data.DataWidth,
				this.Data.SelectorWidth
			);
			int index = (int)Utils.InputToByte(Inputs,
				this.Data.SelectorWidth
			);
			Utils.ResetOutput(Outputs);
			Utils.ByteToOutput(Outputs,
				data,
				this.Data.DataWidth,
				index * this.Data.DataWidth
			);
			Logger.Info($"data {data}, index {index}");
		}
	}
}
