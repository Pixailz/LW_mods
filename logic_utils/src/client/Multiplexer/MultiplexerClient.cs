using LogicWorld.Rendering.Components;

using PixLogicUtils.Shared.CustomData;

namespace PixLogicUtils.Client
{
	public class MultiplexerClient : ComponentClientCode<IMultiplexerData>
	{
		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}
	}
}
