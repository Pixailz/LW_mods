using LogicWorld.Rendering.Components;

using PixLogicUtils.Shared.CustomData;

namespace PixLogicUtils.Client
{
	public class DemultiplexerClient : ComponentClientCode<IDemultiplexerData>
	{
		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}
	}
}
