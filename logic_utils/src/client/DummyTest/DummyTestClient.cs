using LogicWorld.Rendering.Components;
using PixLogicUtils.Shared.CustomData;

namespace PixLogicUtils.Client
{
	public class DummyTestClient : ComponentClientCode<IDummyData>
	{
		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}
	}
}
